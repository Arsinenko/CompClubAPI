using CompClubAPI.Models;

namespace CompClubAPI.ResponseSchema;

public class GetEquipmentMaintenanceResponse
{
    public List<EquipmentMaintenance> EquipmentMaintenances { get; set; }
}