using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CompClubAPI.Models;

public partial class UserActionLog
{
    public int Id { get; set; }

    public int? ClientId { get; set; }

    public string? Action { get; set; }

    public DateTime? ActionDate { get; set; }

    public decimal? Price { get; set; }

    [JsonIgnore] public virtual Client? Client { get; set; }
}
