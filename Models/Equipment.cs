using System;
using System.Collections.Generic;

namespace CompClubAPI.Models;

public partial class Equipment
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Specification { get; set; }

    public DateOnly? PurchaseDate { get; set; }

    public decimal? PurchasePrice { get; set; }

    public int IdClub { get; set; }

    public int Status { get; set; }

    public int? Quantity { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<EquipmentMaintenance> EquipmentMaintenances { get; set; } = new List<EquipmentMaintenance>();

    public virtual Club IdClubNavigation { get; set; } = null!;

    public virtual EquipmentStatus StatusNavigation { get; set; } = null!;

    public virtual ICollection<WorkingSpaceEquipment> WorkingSpaceEquipments { get; set; } = new List<WorkingSpaceEquipment>();
}
