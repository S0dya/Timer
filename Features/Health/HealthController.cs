using Microsoft.AspNetCore.Mvc;

namespace timer.Features.Health;

[ApiController]
[Route("health")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public ActionResult<string> Check()
    {
        return Ok("Ok");
    }
}
