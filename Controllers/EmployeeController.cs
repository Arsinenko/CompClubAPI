using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CompClubAPI.Models;
using CompClubAPI.Schemas;
using Microsoft.AspNetCore.Authorization;
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
        private readonly CollegeTaskV2Context _context;

        public EmployeeController(CollegeTaskV2Context context)
        {
            _context = context;
        }
        
        
        [HttpPost("hire_employee")]
        public async Task<ActionResult> HireEmployee(HireEmployeeModel employeeModel)
        {
            Employee employee = new Employee
            {
                Login = employeeModel.Login,
                Password = HashHelper.GenerateHash(employeeModel.Password),
                PassportData = employeeModel.PasspordData,
                HireDate = DateOnly.FromDateTime(DateTime.Now),
                IdRole = employeeModel.IdRole,
                Salary = employeeModel.Salary,
                IdClub = employeeModel.IdClub,
                CreatedAt = DateTime.Now
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return Created("", new { message = "Employee hired successfully!", id = employee.Id });
        }
        [Authorize(Roles = "Owner")]
        [HttpGet("get_employees")]
        public async Task<ActionResult> GetEmployees()
        {
            List<Employee> employees = await _context.Employees.ToListAsync();
            return Ok(new {employees = employees});
            
        }
        [Authorize(Roles = "Owner,Admin")]
        [HttpGet("get_employees_by_club/{id}")]
        public async Task<ActionResult> GetEmployeesByClub(int id)
        {
            List<Employee> employees = await _context.Employees.Where(e => e.IdClub == id).ToListAsync();
            return Ok(new { employees = employees });
        }
        [Authorize(Roles = "Owner")]
        [HttpPost("fire_employee/{id}")]
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
        //update employee password
        [HttpPut("update_employee_password")]
        public async Task<ActionResult> UpdatePassword([FromBody]string password)
        {
            int id = Convert.ToInt32(User.FindFirst("employee_id")?.Value);
            Employee? employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return BadRequest(new { message = "Employee not found!" });
            }
            var hash = HashHelper.GenerateHash(password);
            employee.Password = hash;
            employee.PasswordChangedAt = DateTime.Now;
            _context.Entry(employee).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Password updated successfully!" });
        }

        [Authorize(Roles = "Admin,Owner")]
        [HttpPut("update_employee/{id}")]
        public async Task<ActionResult> UpdateEmployee(int id, UpdateEmployeeModel employeeModel)
        {
            Employee? employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return BadRequest(new { message = "Employee not found!" });
            }
            employee.Login = employeeModel.Login;
            employee.Salary = employeeModel.Salary;
            employee.IdRole = employeeModel.IdRole;
            employee.IdClub = employeeModel.IdClub;
            _context.Entry(employee).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Employee updated successfully!" });
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
                    new Claim("employee_id", employee.Id.ToString()),
                    new Claim("employee_login", employee.Login.ToString()),
                    new Claim(ClaimTypes.Role, role)
                },

                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
