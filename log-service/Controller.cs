using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GameServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogController : ControllerBase
    {
        private readonly ILogService _logService;

        public LogController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLog()
        {
            var logContent = await _logService.GetLogContentAsync();
            if (logContent == "Log file not found.")
            {
                return NotFound(logContent);
            }
            return Ok(logContent);
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet("welcome")]
        public IActionResult Welcome()
        {
            return Ok("Welcome to the Home API!");
        }
    }
}
