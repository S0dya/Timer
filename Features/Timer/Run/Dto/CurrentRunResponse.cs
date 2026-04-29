namespace timer.Features.Timer.Run.Dto;

public class CurrentRunResponse
{
    public int CompletedSessions { get; set; }
    public int PlannedSessionsAmount { get; set; }
    public int SessionDuration { get; set; }
    
    public DateTime? CurrentSessionStartTime { get; set; }
}