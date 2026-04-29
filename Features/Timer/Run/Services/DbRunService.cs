using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using timer.Database;
using timer.Features.Timer.ActiveRun.Services;
using timer.Features.Timer.Exceptions;
using timer.Features.Timer.Run.Domain;
using timer.Features.Timer.Run.Dto;
using timer.Features.Timer.Settings.Services;
using timer.Options;

namespace timer.Features.Timer.Run.Services;

public class DbRunService : IRunService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<DbRunService> _logger;
    private readonly IActiveRunService _activeRunService;
    private readonly ISettingsService _settingsService;
    private readonly TimerSettingsOptions _timerOptions;

    public DbRunService(AppDbContext dbContext,
        ILogger<DbRunService> logger,
        IActiveRunService activeRunService,
        ISettingsService settingsService,
        IOptions<TimerSettingsOptions> timerOptions)
    {
        _dbContext = dbContext;
        _logger = logger;
        _activeRunService = activeRunService;
        _settingsService = settingsService;
        _timerOptions = timerOptions.Value;
    }
    
    public async Task<SessionStartResponse> StartSession(Guid userId)
    {
        var currentRun = await _activeRunService.GetCurrentRun(userId);

        if (currentRun == null)
        {
            currentRun = await CreateNewRun(userId);
            
            await _dbContext.RunEntities.AddAsync(currentRun);
        }

        if (currentRun.CurrentSessionStartTime != null)
        {
            throw new BusinessException("Active session currently running");
        }
        
        currentRun.CurrentSessionStartTime = DateTime.UtcNow;
            
        await _dbContext.SaveChangesAsync();

        return new SessionStartResponse()
        {
            RunId = currentRun.Id,
            SessionStartedAt = currentRun.CurrentSessionStartTime.Value,
            
            SessionDuration = currentRun.SessionDuration,
            PlannedSessionsAmount = currentRun.PlannedSessionsAmount,
            CompletedSessions = currentRun.CompletedSessions,
        };
    }

    public async Task<SessionFinishedResponse> FinishSession(Guid userId)
    {
        var currentRun = await GetRunOrThrow(userId);
        
        if (currentRun.CurrentSessionStartTime == null)
        {
            throw new BusinessException("No active session to finish");
        }

        var elapsed = DateTime.UtcNow - currentRun.CurrentSessionStartTime.Value;
        var expected = TimeSpan.FromSeconds(currentRun.SessionDuration); 
        
        if (elapsed < expected - TimeSpan.FromSeconds(_timerOptions.SessionStopTimerDifferenceOffset))
        {
            throw new ArgumentOutOfRangeException(nameof(expected), "Session duration is out of time difference offset");
        }
        
        currentRun.CurrentSessionStartTime = null;
        currentRun.CompletedSessions++;

        await _dbContext.SaveChangesAsync();
        
        return new SessionFinishedResponse()
        {
            RunId = currentRun.Id,

            SessionDuration = currentRun.SessionDuration,
            PlannedSessionsAmount = currentRun.PlannedSessionsAmount,
            CompletedSessions = currentRun.CompletedSessions,
        };
    }
    
    public async Task CancelSession(Guid userId)
    {
        var currentRun = await GetRunOrThrow(userId);

        if (currentRun.CurrentSessionStartTime == null)
        {
            throw new BusinessException("No active session to cancel");
        }
        
        currentRun.CurrentSessionStartTime = null;
        
        await _dbContext.SaveChangesAsync();
    }

    public async Task<RunFinishResponse> FinishRun(Guid userId, RunFinishRequest request)
    {
        var currentRun = await GetRunOrThrow(userId);

        if (currentRun.CurrentSessionStartTime != null)
        {
            throw new BusinessException("Active session currently running");
        }
        
        currentRun.RunEndTime = DateTime.UtcNow;
        currentRun.Description = request.RunDescription;
        
        await _dbContext.SaveChangesAsync();

        return new RunFinishResponse()
        {
            CompletedSessions = currentRun.CompletedSessions,
            PlannedSessionsAmount = currentRun.PlannedSessionsAmount,
            SessionDuration = currentRun.SessionDuration,
            Description = currentRun.Description,
        };
    }
    
    public async Task CancelRun(Guid userId)
    {
        var currentRun = await GetRunOrThrow(userId);
        
        currentRun.RunEndTime = DateTime.UtcNow;
        currentRun.IsCancelled = true;
        
        await _dbContext.SaveChangesAsync();
        
        // return new RunCanceledResponse()
    }

    public async Task<CurrentRunResponse> GetCurrentRun(Guid userId)
    {
        var currentRun = await GetRunOrThrow(userId);

        return new CurrentRunResponse()
        {
            CompletedSessions = currentRun.CompletedSessions,
            PlannedSessionsAmount = currentRun.PlannedSessionsAmount,
            SessionDuration = currentRun.SessionDuration,
            
            CurrentSessionStartTime = currentRun.CurrentSessionStartTime,
        };
    }

    public async Task<List<RunHistoryResponse>> GetRunHistory(Guid userId, RunHistoryRequest request)
    {
        if (request.Limit > _timerOptions.MaxRunsHistoryLimit)
        {
            request.Limit = _timerOptions.MaxRunsHistoryLimit;
            
            _logger.LogWarning("requested run history and limit exceeds max limit. UserId: {UserId}, RequestedLimit: {TimerRequestedLimit}, MaxLimit: {TimerMaxLimit}",
                userId, request.Limit, _timerOptions.MaxRunsHistoryLimit);
        }
        else if (request.Limit <= 0) 
            throw new ArgumentOutOfRangeException(nameof(request.Limit), "Limit must be greater than zero");
        
        var runEntities = await _dbContext.RunEntities
            .AsNoTracking()
            .Where(x => x.UserId == userId && !x.IsCancelled && x.RunEndTime != null)
            .OrderByDescending(r => r.RunEndTime)
            .Skip(request.Offset)
            .Take(request.Limit)
            .ToListAsync();

        var runHistories = runEntities.Select(x => new RunHistoryResponse()
        {
            RunStartTime = x.RunStartTime,
            RunEndTime = x.RunEndTime.Value,
            CompletedSessions = x.CompletedSessions,
            PlannedSessionsAmount = x.PlannedSessionsAmount,
            SessionDuration = x.SessionDuration,
            Description = x.Description,
        }).ToList();
        
        return runHistories;
    }
    
    private async Task<RunEntity> GetRunOrThrow(Guid userId)
    {
        var run = await _activeRunService.GetCurrentRun(userId);

        if (run == null) throw new BusinessException("No active run");

        return run;
    }

    private async Task<RunEntity> CreateNewRun(Guid userId)
    {
        var settings = await _settingsService.GetTimerSettings(userId);

        return new RunEntity()
        {
            Id = Guid.NewGuid(),
            UserId = userId,

            CurrentSessionStartTime = null,
            RunStartTime = DateTime.UtcNow,
            RunEndTime = null,

            CompletedSessions = 0,
            SessionDuration = settings.SessionDuration,
            PlannedSessionsAmount = settings.SessionsAmount,
            
            IsCancelled = false,
        };
    }
}
