using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CompClubAPI.Models;

public partial class Tariff
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal PricePerMinute { get; set; }

    [JsonIgnore] public virtual ICollection<WorkingSpace> WorkingSpaces { get; set; } = new List<WorkingSpace>();
}
