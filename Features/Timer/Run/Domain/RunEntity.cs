using System.ComponentModel.DataAnnotations;

namespace timer.Features.Timer.Run.Domain;

public class RunEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    
    public DateTime RunStartTime { get; set; }
    public DateTime? RunEndTime { get; set; }
    public DateTime? CurrentSessionStartTime { get; set; }
    
    public int CompletedSessions { get; set; }
    public int PlannedSessionsAmount { get; set; }
    public int SessionDuration { get; set; }
    
    [Required]
    [MaxLength(500)]
    public string? Description { get; set; }
    public bool IsCancelled { get; set; }
}
