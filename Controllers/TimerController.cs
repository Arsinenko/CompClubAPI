using CompClubAPI.Models;
using CompClubAPI.Schemas;
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
        private readonly CollegeTaskContext _context;
        

        public TimerController(TimerService timerService, CollegeTaskContext context)
        {
            _timerService = timerService;
            _context = context;
        }
        [Authorize(Roles = "Client")]
        [HttpPost("start")]
        public async Task<ActionResult> StartTimer([FromQuery]int interval, CreateBookingModel model)
        {
            //create booking
            Booking booking = new Booking
            {
                IdWorkingSpace = model.IdWorkingSpace,
                StartTime = DateTime.Now
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            
            int accountId = Convert.ToInt32(User.FindFirst("account_id")?.Value);
            int timerId = await _timerService.StartTimer(interval, accountId);
            return Ok(new {timerId=timerId, message = "Timer started"});
        }

        [HttpPost("stop")]
        public async Task<ActionResult> StopTimer([FromQuery] int timerId)
        {
            bool isStopped = await _timerService.StopTimer(timerId);
            return Ok(new {isStopped=isStopped, message = "Timer stopped"});
        }
    }
}
