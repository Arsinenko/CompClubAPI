using CompClubAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("create_role")]
        public async Task<ActionResult> CreateRole(string Name)
        {
            _context.Roles.Add(new Role { Name = Name });
            await _context.SaveChangesAsync();
            return Ok(new { message = "Role created successfully!" });
        }

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
        
        [HttpPut("update_role/{id}")]
        public async Task<ActionResult> UpdateRole(int id, string Name)
        {
            Role? role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return BadRequest(new { error = "Role not found!" });
            }
            role.Name = Name;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Role updated successfully!" });
        }
    }
}
