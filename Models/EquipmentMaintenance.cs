using System;
using System.Collections.Generic;

using System.Text.Json.Serialization;

namespace CompClubAPI.Models;

public partial class EquipmentMaintenance
{
    public int Id { get; set; }

    public int EquipmentId { get; set; }

    public DateTime? MaintenanceDate { get; set; }

    public string? Description { get; set; }

    public decimal? Cost { get; set; }

    [JsonIgnore] public virtual Equipment Equipment { get; set; } = null!;
}
