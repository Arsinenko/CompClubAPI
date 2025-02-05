using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CompClubAPI.Models;

public partial class WorkingSpaceEquipment
{
    public int Id { get; set; }

    public int IdWorkingSpace { get; set; }

    public int IdEquipment { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore] public virtual Equipment IdEquipmentNavigation { get; set; } = null!;

    [JsonIgnore] public virtual WorkingSpace IdWorkingSpaceNavigation { get; set; } = null!;
}
