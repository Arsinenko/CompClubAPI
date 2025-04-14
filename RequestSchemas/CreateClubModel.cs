namespace CompClubAPI.Schemas;

public class CreateClubModel
{
    public string Address { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? WorkingHours { get; set; }
    
    public decimal Finances { get; set; }
}