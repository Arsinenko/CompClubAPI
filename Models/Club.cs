using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CompClubAPI.Models;

public partial class Club
{
    public int Id { get; set; }

    public string Address { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? WorkingHours { get; set; }

    public int Employees { get; set; }

    [JsonIgnore] public virtual Employee EmployeesNavigation { get; set; } = null!;

    [JsonIgnore] public virtual ICollection<Equipment> Equipment { get; set; } = new List<Equipment>();

    [JsonIgnore] public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
}
