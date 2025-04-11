using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CompClubAPI.Models;

public partial class EquipmentStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    [JsonIgnore] public virtual ICollection<Equipment> Equipment { get; set; } = new List<Equipment>();
}
