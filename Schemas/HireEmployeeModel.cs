namespace CompClubAPI.Schemas;

public class HireEmployeeModel
{
    public DateOnly HireDate { get; set; }
    public static string Login { get; set; }
    public static string Password { get; set; }
    public static string PasspordData { get; set; }
    
    public static int IdRole { get; set; }
    public static int IdClub { get; set; }
    public static int Salary { get; set; }
   
}