namespace CompClubAPI.Schemas;

public record CreateEquipmentModel(string Type, string Name, string Specification, decimal PurchasePrice, int IdClub);