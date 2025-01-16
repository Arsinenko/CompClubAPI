using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CompClubAPI.Models;

public partial class GameLauncher
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    [JsonIgnore] public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}
