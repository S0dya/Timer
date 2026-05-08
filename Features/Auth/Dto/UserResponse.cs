using timer.Features.Auth.Domain;

namespace timer.Features.Auth.Dto;

public class UserResponse
{
    public Guid UserId { get; set; }
    public string Username { get; set; }
    public UserRole Role { get; set; } = UserRole.User;
}