using System;
using System.Collections.Generic;

namespace CompClubAPI.Models;

public partial class Payment
{
    public int Id { get; set; }

    public byte[]? EncryptedCardNumber { get; set; }

    public byte[]? EncryptedCvv { get; set; }

    public int AccountId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Account Account { get; set; } = null!;
}
