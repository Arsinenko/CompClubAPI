namespace CompClubAPI.ResponseSchema;

public class EmployeeResponseForAdmin
{
    public int Id { get; set; }
    public string Login { get; set; }
    public int Salary { get; set; }
    public DateTime HireDate { get; set; }
}