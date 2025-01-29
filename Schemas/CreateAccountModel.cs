namespace CompClubAPI.Schemas;

public class CreateAccountModel
{
    public int ClientId { get; set; }
    public decimal Balance { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
}