namespace timer.Features.Timer.Run.Dto;

public class SessionStartResponse
{
    public Guid RunId { get; set; }
    public DateTime SessionStartedAt { get; set; }
    
    public int CompletedSessions { get; set; }
    
    public int SessionDuration { get; set; }
    public int PlannedSessionsAmount { get; set; }
}