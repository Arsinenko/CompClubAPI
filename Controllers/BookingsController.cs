using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CompClubAPI.Models;
using CompClubAPI.Schemas;
using Microsoft.AspNetCore.Authorization;

namespace CompClubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly CollegeTaskContext _context;

        public BookingsController(CollegeTaskContext context)
        {
            _context = context;
        }

        // GET: api/Bookings
        [Authorize(Roles = "Employee")]
        [HttpGet("get_bookings")]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            return await _context.Bookings.ToListAsync();
        }

        // GET: api/Bookings/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }

        // PUT: api/Bookings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Client")]
        [HttpPut("update_booking/{id}")]
        public async Task<IActionResult> PutBooking(int id, Booking booking)
        {
            int clientID = Convert.ToInt32(User.FindFirst("client_id")?.Value);
            Booking? bookingResult = await _context.Bookings.Where(b => b.IdClient == clientID && b.Id == id).FirstOrDefaultAsync();

            if (bookingResult == null)
            {
                return BadRequest(new { error = "Booking not found or it not yours!" });
            }

            bookingResult.StartTime = booking.StartTime;
            bookingResult.EndTime = booking.EndTime;
            bookingResult.TotalCost = booking.TotalCost;
            bookingResult.Status = booking.Status;
            bookingResult.PaymentMethod = booking.PaymentMethod;

            _context.Entry(bookingResult).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Bookings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Client")]
        [HttpPost("create_booking")]
        public async Task<ActionResult<Booking>> PostBooking(CreateBookingModel bookingModel)
        {
            int clientId = Convert.ToInt32(User.FindFirst("client_id")?.Value);

            var booking = new Booking
            {
                IdClient = clientId,
                StartTime = bookingModel.StartTime,
                EndTime = bookingModel.EndTime,
                TotalCost = bookingModel.TotalCost,
                Status = bookingModel.Status,
                PaymentMethod = bookingModel.PaymentMethod
            };
            
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBooking", new { id = booking.Id }, booking);
        }

        // DELETE: api/Bookings/5
        [Authorize]
        [HttpDelete("delete_booking/{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.Id == id);
        }
    }
}
