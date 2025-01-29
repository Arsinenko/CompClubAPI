using System;
using System.Collections.Generic;

using System.Text.Json.Serialization;

namespace CompClubAPI.Models;

public partial class PaymentMethod
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool? IsActive { get; set; }

    [JsonIgnore] public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    [JsonIgnore] public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
