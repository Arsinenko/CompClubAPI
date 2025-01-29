using System;
using System.Collections.Generic;

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
}
