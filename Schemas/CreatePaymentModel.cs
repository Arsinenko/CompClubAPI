namespace CompClubAPI.Schemas;

public class CreatePaymentModel
{
    public string CardNumber { get; set; }
    public string Cvv { get; set; }
    public int PaymentMethodId { get; set; }
}