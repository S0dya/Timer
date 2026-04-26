using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using timer.Features.Auth.CurrentUser;
using timer.Features.Timer.Settings.Dto;
using timer.Features.Timer.Settings.Services;

namespace timer.Features.Timer.Settings;

[ApiController]
[Route("timer/settings")]
[Authorize]
public class SettingsController : ControllerBase
{
    private readonly ICurrentUser _currentUser;
    private readonly ISettingsService _settingsService;

    public SettingsController(ICurrentUser currentUser,
        ISettingsService settingsService)
    {
        _currentUser = currentUser;
        _settingsService = settingsService;
    }
    
    [HttpGet]
    public async Task<ActionResult<SettingsResponse>> GetTimerSettings()
    {
        var userId = _currentUser.UserId;

        var response = await _settingsService.GetTimerSettings(userId);

        return Ok (response);
    }

    [HttpPost]
    public async Task<ActionResult<SettingsResponse>> SetTimerSettings([FromBody] SettingsRequest request)
    {
        var userId = _currentUser.UserId;
        
        var response = await _settingsService.SetTimerSettings(userId, request);
        
        return Ok (response);
    }
}
