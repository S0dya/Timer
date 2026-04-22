using timer.Features.Auth.CurrentUser;
using timer.Features.Auth.Domain;

namespace timer.Features.Auth.Jwt;

public class JwtUser
{
    public Guid Id { get; set; }
    // public string Password { get; set; }
    public string Username { get; set; }
    public UserRole UserRole { get; set; }
}