using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CompClubAPI.Models;

public partial class Account
{
    public int Id { get; set; }

    public int IdClient { get; set; }

    public decimal? Balance { get; set; }

    [JsonIgnore] public virtual Client IdClientNavigation { get; set; } = null!;
}
