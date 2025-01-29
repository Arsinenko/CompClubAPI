using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

using System.Text.Json.Serialization;

namespace CompClubAPI.Models;

public partial class Client
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string LastName { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsActive { get; set; }

    [JsonIgnore] public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    [JsonIgnore] public virtual ICollection<BalanceHistory> BalanceHistories { get; set; } = new List<BalanceHistory>();

    [JsonIgnore] public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    [JsonIgnore] public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    [JsonIgnore] public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
