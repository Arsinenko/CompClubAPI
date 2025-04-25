namespace CompClubAPI.ResponseSchema;

public class ClientBookingsResponse
{
    public List<BookingDto> Bookings { get; set; } = new();
}

public class BookingDto
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public decimal? TotalCost { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public ClientWorkingSpaceDto WorkingSpace { get; set; } = null!;
}

public class ClientWorkingSpaceDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string Status { get; set; } = null!;
    public WorkingSpaceClubDto Club { get; set; } = null!;
}

public class WorkingSpaceClubDto
{
    public string Address { get; set; } = null!;
}