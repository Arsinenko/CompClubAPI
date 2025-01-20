namespace CompClubAPI.Schemas;

public class HireEmployeeModel
{
    public string login { get; set; }
    public string password { get; set; }
    public string passpordData { get; set; }
    public DateOnly? hire_date { get; set; }
    public int idRole { get; set; }
    public int idClub { get; set; }
    public int salary { get; set; }
}