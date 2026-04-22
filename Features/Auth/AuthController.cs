using Microsoft.AspNetCore.Mvc;
using timer.Features.Auth.Dto;
using timer.Features.Auth.Services;

namespace timer.Features.Auth;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        var response = await _authService.Login(request);
        return Ok(response);
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody]RegisterRequest request)
    {
        var response = await _authService.Register(request);
        return Ok(response);
    }
}