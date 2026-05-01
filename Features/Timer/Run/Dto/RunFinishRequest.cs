using System.ComponentModel.DataAnnotations;

namespace timer.Features.Timer.Run.Dto;

public class RunFinishRequest
{
    [MaxLength(500)]
    public string? RunDescription { get; set; }
}