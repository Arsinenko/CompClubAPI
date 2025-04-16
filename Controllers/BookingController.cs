using CompClubAPI.Context;
using CompClubAPI.Models;
using CompClubAPI.ResponseSchema;
using CompClubAPI.Schemas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CompClubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly CollegeTaskV2Context _context;

        public BookingController(CollegeTaskV2Context context)
        {
            _context = context;
        }

        /// <summary>
        /// Создание предварительного бронирования (одного активного на аккаунт).
        /// Доступно только для клиентов.
        /// </summary>
        [Authorize(Roles = "Client")]
        [HttpPost("create_advanced_booking")]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateAdvancedBooking(CreateAdvancedBookingModel model)
        {
            int accountId = Convert.ToInt32(User.FindFirst("account_id")?.Value);
            var bookingExist = await _context.Bookings.Where(b => b.AccountId == accountId && b.IdStatus == 1).FirstOrDefaultAsync();
            if (bookingExist != null)
            {
                return BadRequest(new {error = "You already have an advanced booking! You can only have one advanced booking at a time."});
            }
            Booking booking = new Booking
            {
                AccountId = accountId,
                IdWorkingSpace = model.IdWorkingSpace,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                IdStatus = 1
            };
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return Ok(new {message = "Advanced booking created successfully!"});
        }

        /// <summary>
        /// Отмена предварительного бронирования клиента.
        /// </summary>
        [Authorize]
        [HttpDelete("advanced_booking_cancellation")]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CancelAdvancedBooking()
        {
            int accountId = Convert.ToInt32(User.FindFirst("account_id")?.Value);
            var bookingExist = await _context.Bookings.Where(b => b.AccountId == accountId && b.IdStatus == 1).FirstOrDefaultAsync();
            if (bookingExist == null)
            {
                return BadRequest(new {error = "You don't have an advanced booking!"});
            }

            _context.Remove(bookingExist);
            await _context.SaveChangesAsync();
            return Ok(new {message = "Advanced booking cancelled successfully!"});
        }

        /// <summary>
        /// Получение списка всех бронирований.
        /// Доступно для владельцев, администраторов и маркетологов.
        /// </summary>
        [Authorize(Roles = "Owner,Admin,Marketer")]
        [HttpGet("get_bookings")]
        [ProducesResponseType(typeof(BookingsResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            List<Booking> bookings = await _context.Bookings.ToListAsync();
            BookingsResponse response = new BookingsResponse
            {
                Bookings = bookings
            };
            
            return Ok(response);
        }

        /// <summary>
        /// Получение информации о конкретном бронировании по ID.
        /// Доступно для владельцев, администраторов и маркетологов.
        /// </summary>
        [Authorize(Roles = "Owner,Admin,Marketer")]
        [HttpGet("get_booking/{id}")]
        [ProducesResponseType(typeof(Booking), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetBookingById(int id)
        {
            Booking? booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return BadRequest(new {error = "Booking not found!"});
            }
            
            return Ok(new {booking});
        }

        /// <summary>
        /// Получение всех бронирований текущего клиента.
        /// </summary>
        [Authorize(Roles = "Client")]
        [HttpGet("get_client_bookings")]
        [ProducesResponseType(typeof(BookingsResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetClientBookings()
        {
            int accountId = Convert.ToInt32(User.FindFirst("account_id")?.Value);
            List<Booking> bookings = await _context.Bookings.Where(b => b.AccountId == accountId).ToListAsync();
            return Ok(new {bookings});
        }

        /// <summary>
        /// Удаление конкретного бронирования клиента по ID.
        /// </summary>
        [Authorize(Roles = "Client")]
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            Booking? booking = await _context.Bookings.Where(b =>
                b.Id == id && b.AccountId == Convert.ToInt32(User.FindFirst("account_id")!.Value))
                .FirstOrDefaultAsync();
            if (booking == null)
            {
                return BadRequest(new {message = "Booking not found or not yours!"});
            }
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return Ok(new {message = "Booking deleted successfully!"});
        }

        /// <summary>
        /// Получение информации о конкретном бронировании текущего клиента по ID.
        /// </summary>
        [Authorize(Roles = "Client")]
        [HttpGet("get_info/{id}")]
        [ProducesResponseType(typeof(BookingResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetBooking(int id)
        {
            int accountId = Convert.ToInt32(User.FindFirst("account_id")?.Value);
            Booking? booking = await _context.Bookings.Where(b => b.Id == id && b.AccountId == accountId).FirstOrDefaultAsync();
            if (booking == null)
            {
                return BadRequest(new {error = "Booking not found or not yours!"});
            }
            return Ok(new {booking});
        }
    }
}
