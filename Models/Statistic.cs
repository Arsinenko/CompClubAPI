using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CompClubAPI.Models;

public partial class Statistic
{
    public int Id { get; set; }

    public decimal Finances { get; set; }

    public int? ClientNumber { get; set; }

    public int IdClub { get; set; }

    [JsonIgnore] public virtual Club IdClubNavigation { get; set; } = null!;
}
