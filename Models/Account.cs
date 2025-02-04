using System;
using System.Collections.Generic;

namespace CompClubAPI.Models;

public partial class Account
{
    public int Id { get; set; }

    public int IdClient { get; set; }

    public decimal? Balance { get; set; }

    public string Login { get; set; } = null!;

    public byte[] Password { get; set; } = null!;

    public DateTime? LastLogin { get; set; }

    public DateTime? PasswordChangedAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<BalanceHistory> BalanceHistories { get; set; } = new List<BalanceHistory>();

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual Client IdClientNavigation { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
