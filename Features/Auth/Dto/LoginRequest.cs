using System.ComponentModel.DataAnnotations;

namespace timer.Features.Auth.Dto;

public class LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}