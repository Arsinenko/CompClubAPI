namespace CompClubAPI.Schemas;

public class HireEmployeeModel
{
    public string Login { get; set; }
    public string Password { get; set; }
    public string PasspordData { get; set; }
    
    public int IdRole { get; set; }
    public int IdClub { get; set; }
    public int Salary { get; set; }
   
}