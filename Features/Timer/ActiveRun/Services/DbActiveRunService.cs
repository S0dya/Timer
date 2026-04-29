using Microsoft.EntityFrameworkCore;
using timer.Database;
using timer.Features.Timer.Run.Domain;

namespace timer.Features.Timer.ActiveRun.Services;

public class DbActiveRunService : IActiveRunService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<DbActiveRunService> _logger;

    public DbActiveRunService(AppDbContext dbContext,
        ILogger<DbActiveRunService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public async Task EnsureNoActiveSession(Guid userId)
    {
        var currentRun = await GetCurrentRun(userId);

        if (currentRun != null && currentRun.CurrentSessionStartTime != null)
        {
            throw new ArgumentException(nameof(currentRun), "Active session currently running");
        }
    }

    public async Task<RunEntity?> GetCurrentRun(Guid userId)
    {
        return await _dbContext.RunEntities.FirstOrDefaultAsync(
            x => x.UserId == userId && x.RunEndTime == null);
    }
}