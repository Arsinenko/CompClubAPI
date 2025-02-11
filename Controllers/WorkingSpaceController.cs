using CompClubAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CompClubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkingSpaceController : ControllerBase
    {
        private readonly CollegeTaskContext _context;

        public WorkingSpaceController(CollegeTaskContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Owner,Admin,System_administrator")]
        [HttpPost("create_working_space")]
        public async Task<ActionResult> CreateWorkingSpace(WorkingSpace workingSpace)
        {
            _context.Add(workingSpace);
            await _context.SaveChangesAsync();
            return Created("", new { message = "Working space created successfully!", id = workingSpace.Id });
        }
        [Authorize(Roles = "Owner,Admin,System_administrator")]
        [HttpGet("working_spaces")]
        public async Task<ActionResult> GetWorkingSpaces()
        {
            List<WorkingSpace> workingSpaces = await _context.WorkingSpaces.ToListAsync();
            return Ok(workingSpaces);
        }
        
        [HttpGet("working_spaces_by_club/{idClub}")]
        public async Task<ActionResult> GetWorkingSpacesByClub(int idClub)
        {
            List<WorkingSpace> workingSpaces = await _context.WorkingSpaces.Where(ws => ws.IdClub == idClub).ToListAsync();
            return Ok(workingSpaces);
        }

        [HttpGet("get_info/{id}")]
        public async Task<ActionResult> GetWorkingSpace(int id)
        {
            WorkingSpace? workingSpace = await _context.WorkingSpaces.FindAsync(id);
            return Ok(workingSpace);
        }
        [Authorize(Roles = "Owner,Admin,System_administrator")]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateWorkingSpace(int id, WorkingSpace workingSpace)
        {
            WorkingSpace? workingSpaceExists = await _context.WorkingSpaces.FindAsync(id);

            if (workingSpaceExists == null)
            {
                return BadRequest(new { error = "Working space not found!" });
            }

            workingSpaceExists.IdClub = workingSpace.IdClub;
            workingSpaceExists.Name = workingSpace.Name;
            workingSpaceExists.Status = workingSpace.Status;
            workingSpace.UpdatedAt = DateTime.Now;

            _context.WorkingSpaces.Update(workingSpaceExists);
            await _context.SaveChangesAsync();
            return Ok(new { message = $"Working space with id {workingSpaceExists.Id} updated successfully!" });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteWorkingSpace(int id)
        {
            WorkingSpace? workingSpace = await _context.WorkingSpaces.FindAsync(id);
            if (workingSpace == null)
            {
                return BadRequest(new { error = "Working space not found!" });
            }
            _context.WorkingSpaces.Remove(workingSpace);
            await _context.SaveChangesAsync();
            return Ok(new { message = $"Working space with id {workingSpace.Id} deleted successfully!" });
        }

        [HttpPost("add_equipment")]
        public async Task<ActionResult> AddEquipmentToWorkingSpace(WorkingSpaceEquipment workingSpaceEquipment)
        {
            _context.Add(workingSpaceEquipment);
            await _context.SaveChangesAsync();
            return Created("", new { message = "Equipment added to working space successfully!" });
        }

        [HttpDelete("delete_equipment/{id}")]
        public async Task<ActionResult> DeleteEquipmentFromWorkingSpace(int id)
        {
            WorkingSpaceEquipment? workingSpaceEquipment = await _context.WorkingSpaceEquipments.FindAsync(id);
            if (workingSpaceEquipment == null)
            {
                return BadRequest(new { error = "Working space equipment not found!" });
            }
            _context.WorkingSpaceEquipments.Remove(workingSpaceEquipment);
            await _context.SaveChangesAsync();
            return Ok(new { message = $"Working space equipment with id {workingSpaceEquipment.Id} deleted successfully!" });
        }
    }
}
