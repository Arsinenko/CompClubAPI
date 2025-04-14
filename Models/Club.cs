using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization;

namespace CompClubAPI.Models;

public partial class Club
{
    public int Id { get; set; }

    public string Address { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? WorkingHours { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsAlive { get; set; }

    [JsonIgnore] public virtual ICollection<CostRevenue> CostRevenues { get; set; } = new List<CostRevenue>();

    [JsonIgnore] public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    [JsonIgnore] public virtual ICollection<Equipment> Equipment { get; set; } = new List<Equipment>();

    [JsonIgnore] public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    [JsonIgnore] public virtual ICollection<Statistic> Statistics { get; set; } = new List<Statistic>();

    [JsonIgnore] public virtual ICollection<WorkingSpace> WorkingSpaces { get; set; } = new List<WorkingSpace>();
}
