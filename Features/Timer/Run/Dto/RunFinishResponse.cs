namespace timer.Features.Timer.Run.Dto;

public class RunFinishResponse
{
    public int CompletedSessions { get; set; }
    public int PlannedSessionsAmount { get; set; }
    public int SessionDuration { get; set; }
    
    public string? Description { get; set; }
}