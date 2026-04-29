using timer.Features.Timer.Run.Dto;

namespace timer.Features.Timer.Run.Services;

public interface IRunService
{
    Task<SessionStartResponse> StartSession(Guid userId);
    Task<SessionFinishedResponse> FinishSession(Guid userId);
    Task CancelSession(Guid userId);
    Task<RunFinishResponse> FinishRun(Guid userId, RunFinishRequest request);
    Task CancelRun(Guid userId);
    Task<CurrentRunResponse> GetCurrentRun(Guid userId);
    Task<List<RunHistoryResponse>> GetRunHistory(Guid userId, RunHistoryRequest request);
}
