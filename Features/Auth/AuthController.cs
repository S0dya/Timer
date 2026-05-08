using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using timer.Features.Auth.CurrentUser;
using timer.Features.Auth.Dto;
using timer.Features.Auth.Services;

namespace timer.Features.Auth;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ICurrentUser _currentUser;
    
    public AuthController(IAuthService authService,
        ICurrentUser currentUser)
    {
        _authService = authService;
        _currentUser = currentUser;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody]LoginRequest request)
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

    [Authorize]
    [HttpGet("get-current-user")]
    public ActionResult<UserResponse> Me()
    {
        return Ok(new UserResponse
        {
            UserId = _currentUser.UserId,
            Username  = _currentUser.Username,
        });
    }
}