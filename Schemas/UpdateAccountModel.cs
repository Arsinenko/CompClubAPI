namespace CompClubAPI.Schemas;

public class UpdateAccountModel
{
    public decimal Balance { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
}