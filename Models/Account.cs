using System;
using System.Collections.Generic;

using System.Text.Json.Serialization;

namespace CompClubAPI.Models;

public partial class Account
{
    public int Id { get; set; }

    public int IdClient { get; set; }

    public decimal? Balance { get; set; }

    public string Login { get; set; } = null!;

    public byte[] Password { get; set; } = null!;

    public DateTime? LastLogin { get; set; }

    public DateTime? PasswordChangedAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsActive { get; set; }

    [JsonIgnore] public virtual Client IdClientNavigation { get; set; } = null!;
}
