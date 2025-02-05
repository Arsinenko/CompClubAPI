using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CompClubAPI.Models;

public partial class BalanceHistory
{
    public int Id { get; set; }

    public string? Action { get; set; }

    public decimal? Price { get; set; }

    public decimal? PreviousBalance { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int AccountId { get; set; }

    [JsonIgnore] public virtual Account Account { get; set; } = null!;
}
