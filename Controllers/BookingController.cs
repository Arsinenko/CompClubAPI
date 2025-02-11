using CompClubAPI.Models;
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
        private readonly CollegeTaskContext _context;

        public BookingController(CollegeTaskContext context)
        {
            _context = context;
        }
        // [Authorize(Roles = "Client")]
        // [HttpPost("create_booking")]
        // public async Task<ActionResult> CreateBooking(CreateBookingModel bookingModel)
        // {
        //     Booking booking = new Booking();
        //     
        //     booking.AccountId = Convert.ToInt32(User.FindFirst("account_id")?.Value);
        //     booking.IdWorkingSpace = bookingModel.IdWorkingSpace;
        //     booking.StartTime = bookingModel.StartTime;
        //     booking.EndTime = bookingModel.EndTime;
        //     booking.TotalCost = bookingModel.TotalCost;
        //     booking.IdPaymentMethod = bookingModel.IdPaymentMethod;
        //     
        //     _context.Bookings.Add(booking);
        //     await _context.SaveChangesAsync();
        //     return Ok(new {message = "Booking created successfully!"});
        // }
        [Authorize(Roles = "Owner,Admin,Marketer")]
        [HttpGet("get_bookings")]
        public async Task<ActionResult> GetBookings()
        {
            List<Booking> bookings = await _context.Bookings.ToListAsync();
            return Ok(bookings);
        }

        [Authorize(Roles = "Owner,Admin,Marketer")]
        [HttpGet("get_booking/{id}")]
        public async Task<ActionResult> GetBookingById(int id)
        {
            Booking? booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound("Booking not found");
            }
            return Ok(booking);
        }
        
        [Authorize(Roles = "Client")]
        [HttpGet("get_client_bookings")]
        public async Task<ActionResult> GetClientBookings()
        {
            int accountId = Convert.ToInt32(User.FindFirst("account_id")?.Value);
            List<Booking> bookings = await _context.Bookings.Where(b => b.AccountId == accountId).ToListAsync();
            return Ok(bookings);
        }
        
        [Authorize(Roles = "Client")]
        [HttpDelete("delete/{id}")]
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
        
        [Authorize(Roles = "Client")]
        [HttpGet("get_info/{id}")]
        public async Task<ActionResult> GetBooking(int id)
        {
            int accountId = Convert.ToInt32(User.FindFirst("account_id")?.Value);
            Booking? booking = await _context.Bookings.Where(b => b.Id == id && b.AccountId == accountId).FirstOrDefaultAsync();
            return Ok(booking);
        }
        
    }
}
