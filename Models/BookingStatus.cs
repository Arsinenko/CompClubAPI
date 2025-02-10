using System;
using System.Collections.Generic;

using System.Text.Json.Serialization;

namespace CompClubAPI.Models;

public partial class BookingStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    [JsonIgnore] public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
