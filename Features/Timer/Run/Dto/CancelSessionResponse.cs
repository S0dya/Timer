namespace timer.Features.Timer.Run.Dto;

public class CancelSessionResponse
{
    public DateTime StartedAt { get; set; }
    public int SessionProgressionSeconds { get; set; }
}