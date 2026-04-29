using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using timer.Database;
using timer.Features.Timer.ActiveRun.Services;
using timer.Features.Timer.Settings.Domain;
using timer.Features.Timer.Settings.Dto;
using timer.Options;

namespace timer.Features.Timer.Settings.Services;

public class DbSettingsService : ISettingsService
{
    private readonly ILogger<DbSettingsService> _logger;
    private readonly AppDbContext _db;
    private readonly TimerSettingsOptions _options;
    private readonly IActiveRunService _activeRunService;

    public DbSettingsService(ILogger<DbSettingsService> logger,
        AppDbContext db,
        IOptions<TimerSettingsOptions> options,
        IActiveRunService activeRunService)
    {
        _logger = logger;
        _db = db;
        _options = options.Value;
        _activeRunService = activeRunService;
    }

    public async Task<SettingsResponse> GetTimerSettings(Guid userId)
    {
        var settingEntity = await _db.TimerSettings.FirstOrDefaultAsync(setting => setting.UserId == userId);

        if (settingEntity == null)
        {
            settingEntity = CreateEntity(userId);          
            
            await _db.TimerSettings.AddAsync(settingEntity);
            await _db.SaveChangesAsync();
        }

        var settingsResponse = new SettingsResponse()
        {
            SessionDuration = settingEntity.SessionDuration,
            SessionsAmount = settingEntity.SessionsAmount,
        };
        
        return settingsResponse;
    }
    
    public async Task<SettingsResponse> SetTimerSettings(Guid userId, SettingsRequest request)
    {
        await _activeRunService.EnsureNoActiveSession(userId);
        
        var settingEntity = await _db.TimerSettings.FirstOrDefaultAsync(setting => setting.UserId == userId);

        if (settingEntity == null)
        {
            settingEntity = CreateEntity(userId);          
            await _db.TimerSettings.AddAsync(settingEntity);
        }
        
        var clampedDuration = Math.Clamp(request.SessionDuration, _options.MinDuration, _options.MaxDuration);
        var clampedAmount = Math.Clamp(request.SessionsAmount, _options.MinAmount, _options.MaxAmount);
        
        _logger.LogInformation("Setting: session duration {TimerSessionDuration}, " +
                               "sessions amount {TimerSessionsAmount}", clampedDuration, clampedAmount);
        
        settingEntity.SessionDuration = clampedDuration; 
        settingEntity.SessionsAmount = clampedAmount;
        
        await _db.SaveChangesAsync();

        var response = new SettingsResponse()
        {
            SessionDuration = clampedDuration,
            SessionsAmount = clampedAmount,
        };
        
        return response;
    }

    private SettingsEntity CreateEntity(Guid userId)
    {
        _logger.LogInformation("Creating new TimerSettings entity for {userId}", userId);

        return new SettingsEntity()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            SessionDuration = _options.DefaultDuration,
            SessionsAmount = _options.DefaultAmount,
        };    
    }
}
