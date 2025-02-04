using System;
using System.Collections.Generic;

namespace CompClubAPI.Models;

public partial class WorkingSpaceEquipment
{
    public int Id { get; set; }

    public int IdWorkingSpace { get; set; }

    public int IdEquipment { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Equipment IdEquipmentNavigation { get; set; } = null!;

    public virtual WorkingSpace IdWorkingSpaceNavigation { get; set; } = null!;
}
