using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CompClubAPI.Models;
using CompClubAPI.Schemas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CompClubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly CollegeTaskContext _context;

        public EmployeeController(CollegeTaskContext context)
        {
            _context = context;
        }

        [HttpPost("hire_employee")]
        public async Task<ActionResult> HireEmployee(HireEmployeeModel employeeModel)
        {
            Employee employee = new Employee
            {
                Login = HireEmployeeModel.Login,
                Password = HashHelper.GenerateHash(HireEmployeeModel.Password),
                PassportData = HireEmployeeModel.PasspordData,
                HireDate = DateOnly.FromDateTime(DateTime.Now),
                IdRole = HireEmployeeModel.IdRole,
                Salary = HireEmployeeModel.Salary,
                IdClub = HireEmployeeModel.IdClub,
                CreatedAt = DateTime.Now
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return Created("", new { message = "Employee hired successfully!", id = employee.Id });
        }

        [HttpGet("get_employees")]
        public async Task<ActionResult> GetEmployees()
        {
            return Ok(await _context.Employees.ToListAsync());
            
        }

        [HttpGet("get_employees_by_club/{id}")]
        public async Task<ActionResult> GetEmployeesByClub(int id)
        {
            return Ok(await _context.Employees.Where(e => e.IdClub == id).ToListAsync());
        }
        
        [HttpPost("fire_employee")]
        public async Task<ActionResult> FireEmployee(int id)
        {
            Employee? employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return BadRequest(new { message = "Employee not found!" });
            }
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Employee fired successfully!" });
        }

        [HttpPost("authorization")]
        public async Task<ActionResult> Authorization(AuthModel authModel)
        {
            byte[] passwordHash = HashHelper.GenerateHash(authModel.password);
            Employee? employee = await _context.Employees.FirstOrDefaultAsync(a =>
                a.Login == authModel.login && a.Password.SequenceEqual(passwordHash));
            if (employee == null)
            {
                return NotFound(new { error = "Employee not found!" });
            }
            string? role = await _context.Roles.Where(r => r.Id == employee.IdRole).Select(r => r.Name).FirstOrDefaultAsync();
            if (role == null)
            {
                return NotFound(new { error = "Role not found!" });
            }
            employee.LastLogin = DateTime.Now;
            _context.Update(employee);
            await _context.SaveChangesAsync();
            string token = GenerateJwtToken(employee, role);
            return Ok(new { token });
        }
        [NonAction]
        public string GenerateJwtToken(Employee employee, string role)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("df8f3c6058ce4d93b799b4d8dc0b5ff66e1eccf69aa29505c6c84a6339a914a4")); //TODO
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: "CompClubAPI",
                audience: "CompClubAPI",
                claims: new[]
                {
                    new Claim("account_id", employee.Id.ToString()),
                    new Claim("account_login", employee.Login.ToString()),
                    new Claim(ClaimTypes.Role, role)
                },

                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
