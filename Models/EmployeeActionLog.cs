using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CompClubAPI.Models;

public partial class EmployeeActionLog
{
    public int Id { get; set; }

    public int IdEmployee { get; set; }

    public string ActionType { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    [JsonIgnore] public virtual Employee IdEmployeeNavigation { get; set; } = null!;
}
