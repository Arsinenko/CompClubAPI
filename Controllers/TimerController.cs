using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CompClubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimerController : ControllerBase
    {
        private readonly TimerService _timerService;

        public TimerController(TimerService timerService)
        {
            _timerService = timerService;
        }
        [Authorize(Roles = "Client")]
        [HttpPost("start")]
        public async Task<ActionResult> StartTimer([FromQuery]int interval)
        {
            int accountId = Convert.ToInt32(User.FindFirst("account_id")?.Value);
            int timerId = _timerService.StartTimer(interval, accountId);
            return Ok(new {timerId=timerId, message = "Timer started"});
        }

        [HttpPost("stop")]
        public async Task<ActionResult> StopTimer([FromQuery] int timerId)
        {
            bool isStopped = _timerService.StopTimer(timerId);
            return Ok(new {isStopped=isStopped, message = "Timer stopped"});
        }
    }
}
