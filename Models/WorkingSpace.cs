using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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

    [JsonIgnore] public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    [JsonIgnore] public virtual Club IdClubNavigation { get; set; } = null!;

    [JsonIgnore] public virtual Tariff? Tariff { get; set; }

    [JsonIgnore] public virtual ICollection<WorkingSpaceEquipment> WorkingSpaceEquipments { get; set; } = new List<WorkingSpaceEquipment>();
}
