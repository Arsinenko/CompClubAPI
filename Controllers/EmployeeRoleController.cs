using CompClubAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CompClubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeRoleController : ControllerBase
    {
        private readonly CollegeTaskContext _context;

        public EmployeeRoleController(CollegeTaskContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Owner,Admin,System_administrator")]
        [HttpPost("create_role")]
        public async Task<ActionResult> CreateRole(string name)
        {
            _context.Roles.Add(new Role { Name = name });
            await _context.SaveChangesAsync();
            return Ok(new { message = "Role created successfully!" });
        }
        [Authorize(Roles = "Owner,Admin,System_administrator")]
        [HttpDelete("delete_role/{id}")]
        public async Task<ActionResult> DeleteRole(int id)
        {
            Role? role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return BadRequest(new { error = "Role not found!" });
            }
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Role deleted successfully!" });
        }
        [Authorize(Roles = "Owner,Admin,System_administrator")]
        [HttpPut("update_role/{id}")]
        public async Task<ActionResult> UpdateRole(int id, string name)
        {
            Role? role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return BadRequest(new { error = "Role not found!" });
            }
            role.Name = name;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Role updated successfully!" });
        }
        [Authorize(Roles = "Owner,Admin,System_administrator")]
        [HttpGet("get_roles")]
        public async Task<ActionResult> GetRoles()
        {
            return Ok(await _context.Roles.ToListAsync());
        }
    }
}
