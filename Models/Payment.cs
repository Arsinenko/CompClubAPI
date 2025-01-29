using System;
using System.Collections.Generic;

using System.Text.Json.Serialization;

namespace CompClubAPI.Models;

public partial class Payment
{
    public int Id { get; set; }

    public byte[]? EncryptedCardNumber { get; set; }

    public byte[]? EncryptedCvv { get; set; }

    public DateOnly? LinkDate { get; set; }

    public int ClientId { get; set; }

    public int PaymentMethodId { get; set; }

    public DateTime? CreatedAt { get; set; }

    [JsonIgnore] public virtual Client Client { get; set; } = null!;

    [JsonIgnore] public virtual PaymentMethod PaymentMethod { get; set; } = null!;
}
