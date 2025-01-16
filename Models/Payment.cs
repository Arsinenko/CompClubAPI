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

    [JsonIgnore] public virtual Client Client { get; set; } = null!;
}
