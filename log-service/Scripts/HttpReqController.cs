using Microsoft.AspNetCore.Mvc;

namespace GameServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HttpReqController : ControllerBase
    {
        private readonly WriteLog _writeLog;

        public HttpReqController(WriteLog writeLog)
        {
            _writeLog = writeLog;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _writeLog.WriteLogEntry("Received HTTP GET request at /HttpReq");
            return Ok("This is an HTTP request response");
        }
    }
}
