using System;
using System.Collections.Generic;

namespace CompClubAPI.Models;

public partial class Shift
{
    public int Id { get; set; }

    public int IdEmployee { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Employee IdEmployeeNavigation { get; set; } = null!;
}
