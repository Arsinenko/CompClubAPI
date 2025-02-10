using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CompClubAPI.Models;

public partial class Statistic
{
    public int Id { get; set; }

    public decimal Finances { get; set; }

    public int? ClientNumber { get; set; }
}
