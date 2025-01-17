using System;
using System.Collections.Generic;

using System.Text.Json.Serialization;

namespace CompClubAPI.Models;

public partial class Game
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal? Price { get; set; }

    public int? IdLauncher { get; set; }

    [JsonIgnore] public virtual GameLauncher? IdLauncherNavigation { get; set; }
}
