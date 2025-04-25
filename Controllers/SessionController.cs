using CompClubAPI.Context;
using CompClubAPI.Models;
using CompClubAPI.Schemas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CompClubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly SessionService _sessionService;
        private readonly CollegeTaskV2Context _context;
        

        public SessionController(SessionService sessionService, CollegeTaskV2Context context)
        {
            _sessionService = sessionService;
            _context = context;
        }

        /// <summary>
        /// Запуск таймера сессии и создание бронирования.
        /// </summary>
        /// <param name="interval">Интервал времени</param>
        /// <param name="model">Модель бронирования</param>
        /// <returns>Идентификатор таймера и сообщение о запуске</returns>
        /// <remarks>Доступно только для роли Client</remarks>
        [Authorize(Roles = "Client")]
        [HttpPost("start")]
        public async Task<ActionResult> StartTimer(CreateBookingModel model)
        {
            var accountId = Convert.ToInt32(User.FindFirst("account_id")?.Value);
            
            int interval = 60;
            //create booking
            Booking booking = new Booking
            {
                AccountId = accountId,
                IdWorkingSpace = model.IdWorkingSpace,
                StartTime = DateTime.Now,
                EndTime = null,
                IdStatus = 1
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            
            
            int timerId = await _sessionService.StartTimer(interval, accountId);
            return Ok(new {timerId=timerId, message = "Timer started"});
        }

        /// <summary>
        /// Остановка таймера сессии.
        /// </summary>
        /// <param name="timerId">Идентификатор таймера</param>
        /// <returns>Статус остановки и сообщение</returns>
        [HttpPost("stop")]
        public async Task<ActionResult> StopTimer([FromQuery] int timerId)
        {
            bool isStopped = await _sessionService.StopTimer(timerId);
            return Ok(new {isStopped=isStopped, message = "Timer stopped"});
        }
    }
}
