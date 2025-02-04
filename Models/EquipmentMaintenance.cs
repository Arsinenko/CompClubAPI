using System;
using System.Collections.Generic;

namespace CompClubAPI.Models;

public partial class EquipmentMaintenance
{
    public int Id { get; set; }

    public int EquipmentId { get; set; }

    public DateTime? MaintenanceDate { get; set; }

    public string? Description { get; set; }

    public decimal? Cost { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Equipment Equipment { get; set; } = null!;
}
