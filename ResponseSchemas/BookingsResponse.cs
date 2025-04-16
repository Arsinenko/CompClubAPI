using CompClubAPI.Models;

namespace CompClubAPI.ResponseSchema;

public class BookingsResponse
{
    public List<Booking> Bookings { get; set; }
}