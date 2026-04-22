using timer.Features.Auth.Dto;

namespace timer.Features.Auth.Services;

public interface IAuthService
{
    Task<AuthResponse> Login(LoginRequest request);
    Task<AuthResponse> Register(RegisterRequest request);
}