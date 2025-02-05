using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CompClubAPI.Models;

public partial class Feedback
{
    public int Id { get; set; }

    public int IdClub { get; set; }

    public int Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int AccountId { get; set; }

    [JsonIgnore] public virtual Account Account { get; set; } = null!;

    [JsonIgnore] public virtual Club IdClubNavigation { get; set; } = null!;
}
