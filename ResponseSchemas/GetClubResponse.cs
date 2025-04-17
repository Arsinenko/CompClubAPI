using CompClubAPI.Models;

namespace CompClubAPI.ResponseSchema;

public class GetClubResponse
{
    public string Address { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string WorkingHours { get; set; }
    public DateTime CreatedAt { get; set; }
}