namespace CompClubAPI.Schemas;

public class CreateBookingModel
{
    public int IdWorkingSpace { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public int IdStatus { get; set; }

    public decimal? TotalCost { get; set; }

    public int? IdPaymentMethod { get; set; }
}