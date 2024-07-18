using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CommonLibrary;

namespace GameServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class A001Controller : ControllerBase
    {
        private readonly WriteLog _writeLog;
        private readonly ILogService _logService;

        public A001Controller(WriteLog writeLog, ILogService logService)
        {
            _writeLog = writeLog;
            _logService = logService;
        }

        [HttpGet("httpreq")]
        public IActionResult HttpReq()
        {
            _writeLog.WriteLogEntry("Received HTTP GET request at /api/A001/httpreq");
            return Ok("This is an HTTP request response");
        }

        [HttpGet("log")]
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
}