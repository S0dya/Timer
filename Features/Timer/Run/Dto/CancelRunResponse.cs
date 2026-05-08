namespace timer.Features.Timer.Run.Dto;

public class CancelRunResponse
{
    public DateTime RunStartTime { get; set; }
    public DateTime CancelledAt { get; set; }
    public int CompletedSessions { get; set; }
    public int PlannedSessionsAmount { get; set; }
    public int SessionDuration { get; set; }
}