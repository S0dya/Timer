using timer.Features.Auth.CurrentUser;

namespace timer.Features.Auth.Jwt;

public interface IJwtTokenGenerator
{
    string GenerateToken(JwtUser user);
}