namespace CompClubAPI.Schemas;

public record CreateEquipmentMaintenanceModel(int EquipmentId, DateTime MaintenanceDate, string? Description, decimal Cost);