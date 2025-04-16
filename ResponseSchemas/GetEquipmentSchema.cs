using CompClubAPI.Models;

namespace CompClubAPI.ResponseSchema;

public class GetEquipmentSchema
{
    public List<Equipment> Equipments { get; set; }
}