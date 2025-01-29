using System;
using System.Collections.Generic;

using System.Text.Json.Serialization;

namespace CompClubAPI.Models;

public partial class BalanceHistory
{
    public int Id { get; set; }

    public int? ClientId { get; set; }

    public string? Action { get; set; }

    public DateTime? ActionDate { get; set; }

    public decimal? Price { get; set; }

    public decimal? PreviousBalance { get; set; }

    public DateTime? CreatedAt { get; set; }

    [JsonIgnore] public virtual Client? Client { get; set; }
}
