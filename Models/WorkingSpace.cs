using System;
using System.Collections.Generic;

using System.Text.Json.Serialization;

namespace CompClubAPI.Models;

public partial class WorkingSpace
{
    public int Id { get; set; }

    public int IdEquipment { get; set; }

    public string Status { get; set; } = null!;

    [JsonIgnore] public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    [JsonIgnore] public virtual Equipment IdEquipmentNavigation { get; set; } = null!;
}
