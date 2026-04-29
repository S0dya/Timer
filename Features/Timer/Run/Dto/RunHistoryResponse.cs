namespace timer.Features.Timer.Run.Dto;

public class RunHistoryResponse
{
    public DateTime RunStartTime { get; set; }
    public DateTime RunEndTime { get; set; }
    
    public int CompletedSessions { get; set; }
    public int PlannedSessionsAmount { get; set; }
    public int SessionDuration { get; set; }
    
    public string? Description { get; set; }
}