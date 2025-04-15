using CompClubAPI.Models;

namespace CompClubAPI.ResponseSchema;

public class GetRolesResponse
{
    public List<Role> Roles { get; set; }
}