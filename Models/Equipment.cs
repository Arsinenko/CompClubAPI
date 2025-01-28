using System;
using System.Collections.Generic;

using System.Text.Json.Serialization;

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

    public int? IdWorkingSpace { get; set; }

    [JsonIgnore] public virtual ICollection<EquipmentMaintenance> EquipmentMaintenances { get; set; } = new List<EquipmentMaintenance>();

    [JsonIgnore] public virtual Club IdClubNavigation { get; set; } = null!;

    [JsonIgnore] public virtual WorkingSpace? IdWorkingSpaceNavigation { get; set; }

    [JsonIgnore] public virtual EquipmentStatus StatusNavigation { get; set; } = null!;
}
