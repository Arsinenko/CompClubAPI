using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CompClubAPI.Models;

public partial class Role
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    [JsonIgnore] public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
