using timer.Features.Timer.Run.Domain;

namespace timer.Features.Timer.ActiveRun.Services;

public interface IActiveRunService
{
    Task EnsureNoActiveSession(Guid userId);
    Task<RunEntity?> GetCurrentRun (Guid userId);
}