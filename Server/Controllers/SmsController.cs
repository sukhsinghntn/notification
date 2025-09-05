using Microsoft.AspNetCore.Mvc;
using NDAProcesses.Shared.Models;
using NDAProcesses.Shared.Services;

namespace NDAProcesses.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SmsController : ControllerBase
    {
        private readonly ISmsService _smsService;

        public SmsController(ISmsService smsService)
        {
            _smsService = smsService;
        }

        [HttpPost]
        public async Task<ActionResult<bool>> Send([FromBody] SmsMessage message)
        {
            var success = await _smsService.SendSms(message);
            if (success)
            {
                return Ok(true);
            }
            return BadRequest(false);
        }
    }
}
