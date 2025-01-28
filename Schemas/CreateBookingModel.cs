namespace CompClubAPI.Schemas;

public class CreateBookingModel
{
    public int IdWorkingSpace { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public string Status { get; set; } = null!;

    public decimal? TotalCost { get; set; }

    public string? PaymentMethod { get; set; }
}