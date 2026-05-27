using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using timer.Features.Auth.CurrentUser;
using timer.Features.Timer.Run.Dto;
using timer.Features.Timer.Run.Services;
using Timer.Infrastructure.DependencyInjection.RateLimiting;

namespace timer.Features.Timer.Run;

[ApiController]
[Route("timer/run")]
[Authorize]
public class RunController : ControllerBase
{
    private readonly ICurrentUser _currentUser;
    private readonly IRunService _runService;

    public RunController(ICurrentUser currentUser,
        IRunService runService)
    {
        _currentUser = currentUser;
        _runService = runService;
    }
    
    [EnableRateLimiting(RateLimitPolicies.Writes)]
    [HttpPost("start-session")]
    public async Task<ActionResult<SessionStartResponse>> StartSession()
    {
        var response = await _runService.StartSession(_currentUser.UserId);
        
        return Ok(response);
    }
    
    [EnableRateLimiting(RateLimitPolicies.Writes)]
    [HttpPost("finish-session")]
    public async Task<ActionResult<SessionFinishedResponse>> FinishSession()
    {
        var response = await _runService.FinishSession(_currentUser.UserId);
        
        return Ok(response);
    }
    
    [EnableRateLimiting(RateLimitPolicies.Writes)]
    [HttpPost("cancel-session")]
    public async Task<ActionResult<CancelSessionResponse>> CancelSession()
    {
        var response = await _runService.CancelSession(_currentUser.UserId);

        return Ok(response);
    }
    
    [EnableRateLimiting(RateLimitPolicies.Writes)]
    [HttpPost("finish-run")]
    public async Task<ActionResult<RunFinishResponse>> FinishRun([FromBody]RunFinishRequest request)
    {
        var response = await _runService.FinishRun(_currentUser.UserId, request);

        return Ok(response);
    }
    
    [EnableRateLimiting(RateLimitPolicies.Writes)]
    [HttpPost("cancel-run")]
    public async Task<ActionResult<CancelRunResponse>> CancelRun()
    {
        var response = await _runService.CancelRun(_currentUser.UserId);

        return Ok(response);
    }
    
    [EnableRateLimiting(RateLimitPolicies.Reads)]
    [HttpGet("get-current-run")]
    public async Task<ActionResult<CurrentRunResponse>> GetCurrentRun()
    {
        var response = await _runService.GetCurrentRun(_currentUser.UserId);
        
        return Ok(response);
    }
    
    [EnableRateLimiting(RateLimitPolicies.History)]
    [HttpGet("get-run-history")]
    public async Task<ActionResult<List<RunHistoryResponse>>> GetRunHistory([FromQuery]RunHistoryRequest request)
    {
        var responseList = await _runService.GetRunHistory(_currentUser.UserId, request);

        if (responseList.Count == 0) return NoContent();
        
        return Ok(responseList);
    }
}
