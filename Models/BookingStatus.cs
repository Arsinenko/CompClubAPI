using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CompClubAPI.Models;

public partial class BookingStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    [JsonIgnore] public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
