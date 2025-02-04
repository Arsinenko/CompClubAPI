﻿using System;
using System.Collections.Generic;

namespace CompClubAPI.Models;

public partial class WorkingSpace
{
    public int Id { get; set; }

    public int IdClub { get; set; }

    public string? Name { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? TariffId { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Club IdClubNavigation { get; set; } = null!;

    public virtual Tariff? Tariff { get; set; }

    public virtual ICollection<WorkingSpaceEquipment> WorkingSpaceEquipments { get; set; } = new List<WorkingSpaceEquipment>();
}
