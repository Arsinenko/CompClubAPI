using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

using System.Text.Json.Serialization;

using System.Text.Json.Serialization;

namespace CompClubAPI.Models;

public partial class Feedback
{
    public int Id { get; set; }

    public int IdClub { get; set; }

    public int? IdClient { get; set; }

    public int Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime? FeedbackDate { get; set; }

    [JsonIgnore] public virtual Client? IdClientNavigation { get; set; }

    [JsonIgnore] public virtual Club IdClubNavigation { get; set; } = null!;
}
