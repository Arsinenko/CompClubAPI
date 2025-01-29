using System;
using System.Collections.Generic;

using System.Text.Json.Serialization;

namespace CompClubAPI.Models;

public partial class Booking
{
    public int Id { get; set; }

    public int IdClient { get; set; }

    public int IdWorkingSpace { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public int IdStatus { get; set; }

    public decimal? TotalCost { get; set; }

    public int? IdPaymentMethod { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore] public virtual Client IdClientNavigation { get; set; } = null!;

    [JsonIgnore] public virtual PaymentMethod? IdPaymentMethodNavigation { get; set; }

    [JsonIgnore] public virtual BookingStatus IdStatusNavigation { get; set; } = null!;

    [JsonIgnore] public virtual WorkingSpace IdWorkingSpaceNavigation { get; set; } = null!;
}
