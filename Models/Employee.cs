using System;
using System.Collections.Generic;

using System.Text.Json.Serialization;

namespace CompClubAPI.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public byte[] Password { get; set; } = null!;

    public string PassportData { get; set; } = null!;

    public DateOnly HireDate { get; set; }

    public int IdRole { get; set; }

    public decimal Salary { get; set; }

    public int IdClub { get; set; }

    public DateTime? LastLogin { get; set; }

    public DateTime? PasswordChangedAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore] public virtual ICollection<EmployeeActionLog> EmployeeActionLogs { get; set; } = new List<EmployeeActionLog>();

    [JsonIgnore] public virtual Club IdClubNavigation { get; set; } = null!;

    [JsonIgnore] public virtual Role IdRoleNavigation { get; set; } = null!;

    [JsonIgnore] public virtual ICollection<Shift> Shifts { get; set; } = new List<Shift>();
}
