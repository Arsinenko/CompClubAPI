using CompClubAPI.Context;
using CompClubAPI.Models;
using CompClubAPI.ResponseSchema;
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
        private readonly CollegeTaskV2Context _context;

        public EmployeeRoleController(CollegeTaskV2Context context)
        {
            _context = context;
        }
        /// <summary>
        /// Создание роли. Только для владельцев.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [Authorize(Roles = "Owner")]
        [HttpPost("create_role")]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult> CreateRole(string name)
        {
            _context.Roles.Add(new Role { Name = name });
            await _context.SaveChangesAsync();
            return Ok(new { message = "Role created successfully!" });
        }
        /// <summary>
        /// Удаление роли. Только для владельцев.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Owner")]
        [HttpDelete("delete_role/{id}")]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
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
        /// <summary>
        /// Обновление роли. Только для владельцев.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [Authorize(Roles = "Owner")]
        [HttpPut("update_role/{id}")]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
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
        /// <summary>
        /// Получение ролей. Для сотрудников.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Owner,Admin,System_administrator")]
        [HttpGet("get_roles")]
        [ProducesResponseType(typeof(GetRolesResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<GetRolesResponse>> GetRoles()
        {
            List<Role> roles = await _context.Roles.ToListAsync();
            GetRolesResponse response = new GetRolesResponse
            {
                Roles = roles
            };
            return Ok(response);
        }
    }
}
