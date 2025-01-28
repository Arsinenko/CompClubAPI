﻿using System;
using System.Collections.Generic;

using System.Text.Json.Serialization;

namespace CompClubAPI.Models;

public partial class Client
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public byte[] Password { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string LastName { get; set; } = null!;

    [JsonIgnore] public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    [JsonIgnore] public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    [JsonIgnore] public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    [JsonIgnore] public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    [JsonIgnore] public virtual ICollection<BalanceHistory> UserActionLogs { get; set; } = new List<BalanceHistory>();
}
