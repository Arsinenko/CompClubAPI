using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CompClubAPI.Models;
using CompClubAPI.Schemas;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace CompClubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly CollegeTaskContext _context;

        public EmployeesController(CollegeTaskContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [Authorize(Roles = "Employee")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return await _context.Employees.ToListAsync();
        }

        // GET: api/Employees/5
        [Authorize(Roles = "Employee")]
        [HttpGet("get_info/{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Employee")]
        [HttpPut("update_info/{id}")]
        public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Employee")]
        [HttpPost("hire_employee")]
        public async Task<ActionResult<Employee>> PostEmployee(HireEmployeeModel model)
        {
            byte[] password = HashHelper.GenerateHash(model.password);
            Employee employee = new Employee
            {
                Login = model.login, Password = password, PassportData = model.passpordData, HireDate = model.hire_date,
                IdRole = model.idRole, Salary = model.salary,IdClub = model.idClub
            };
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
        }

        [HttpPost("auth")]
        public IActionResult Auth(AuthEmployeeModel model)
        {
            byte[] passwordHash = HashHelper.GenerateHash(model.password);

            Employee? employee = _context.Employees.FirstOrDefault(x => x.Login == model.login && x.Password == passwordHash);
            if (employee == null)
            {
                return NotFound(new {error = "Client not found!"});
            }

            string token = GenerateJWTToken(employee);
            return Ok(new { token });
        }

        [NonAction]
        public string GenerateJWTToken(Employee employee)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("df8f3c6058ce4d93b799b4d8dc0b5ff66e1eccf69aa29505c6c84a6339a914a4")); //TODO
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "CompClubAPI",
                audience: "CompClubAPI",
                claims: new[]
                {
                    new Claim("client_id", employee.Id.ToString()),
                    new Claim("client_login", employee.Login.ToString()),
                    new Claim(ClaimTypes.Role, "Employee")
                },

                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // DELETE: api/Employees/5
        [Authorize(Roles = "Employee")]
        [HttpDelete("fire_employee/{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
