using CompClubAPI.Models;

namespace CompClubAPI.ResponseSchema;

public class WorkingSpaceDto
{
    public int Id { get; set; }
    public int IdClub { get; set; }
    public string? Name { get; set; }
    public string Status { get; set; } = null!;
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}