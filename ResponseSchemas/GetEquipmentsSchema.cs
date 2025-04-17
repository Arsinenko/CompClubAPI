using CompClubAPI.Models;

namespace CompClubAPI.ResponseSchema;

public class GetEquipmentsSchema
{
    public List<Equipment> Equipments { get; set; }
}