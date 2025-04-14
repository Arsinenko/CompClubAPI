using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CompClubAPI.Models;

public partial class CostRevenue
{
    public int Id { get; set; }

    public int IdClub { get; set; }

    public decimal Amount { get; set; }

    public bool? Revenue { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    [JsonIgnore] public virtual Club IdClubNavigation { get; set; } = null!;
}
