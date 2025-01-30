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
        public readonly CollegeTaskContext _context;

        public BookingController(CollegeTaskContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Client")]
        [HttpPost("create_booking")]
        public async Task<ActionResult> CreateBooking(CreateBookingModel bookingModel)
        {
            Booking booking = new Booking();
            
            booking.AccountId = Convert.ToInt32(User.FindFirst("account_id")?.Value);
            booking.IdWorkingSpace = bookingModel.IdWorkingSpace;
            booking.StartTime = bookingModel.StartTime;
            booking.EndTime = bookingModel.EndTime;
            booking.TotalCost = bookingModel.TotalCost;
            booking.IdPaymentMethod = bookingModel.IdPaymentMethod;
            
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return Ok(new {message = "Booking created successfully!"});
        }

        [HttpGet]
        public async Task<ActionResult> GetBookings()
        {
            List<Booking> bookings = await _context.Bookings.ToListAsync();
            return Ok(bookings);
        }
    }
}
