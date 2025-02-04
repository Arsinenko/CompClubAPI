using System;
using System.Collections.Generic;

namespace CompClubAPI.Models;

public partial class Tariff
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal PricePerMinute { get; set; }

    public virtual ICollection<WorkingSpace> WorkingSpaces { get; set; } = new List<WorkingSpace>();
}
