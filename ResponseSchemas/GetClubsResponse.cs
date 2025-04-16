using CompClubAPI.Models;

namespace CompClubAPI.ResponseSchema;

public class GetClubsResponse
{
    public List<Club> Clubs { get; set; }
}