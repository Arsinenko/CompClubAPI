using System;
using System.Collections.Generic;

namespace CompClubAPI.Models;

public partial class Booking
{
    public int Id { get; set; }

    public int IdWorkingSpace { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public int IdStatus { get; set; }

    public decimal? TotalCost { get; set; }

    public int? IdPaymentMethod { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int AccountId { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual PaymentMethod? IdPaymentMethodNavigation { get; set; }

    public virtual BookingStatus IdStatusNavigation { get; set; } = null!;

    public virtual WorkingSpace IdWorkingSpaceNavigation { get; set; } = null!;
}
