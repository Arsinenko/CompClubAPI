using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CompClubAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace CompClubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkingSpacesController : ControllerBase
    {
        private readonly CollegeTaskContext _context;

        public WorkingSpacesController(CollegeTaskContext context)
        {
            _context = context;
        }

        // GET: api/WorkingSpaces
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkingSpace>>> GetWorkingSpaces()
        {
            return await _context.WorkingSpaces.ToListAsync();
        }

        // GET: api/WorkingSpaces/5
        [Authorize]
        [HttpGet("get_status/{id}")]
        public async Task<ActionResult<WorkingSpace>> GetWorkingSpace(int id)
        {
            var workingSpace = await _context.WorkingSpaces.FindAsync(id);

            if (workingSpace == null)
            {
                return NotFound();
            }

            return workingSpace;
        }

        // PUT: api/WorkingSpaces/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("update_status/{id}")]
        public async Task<IActionResult> PutWorkingSpace(int id, WorkingSpace workingSpace)
        {
            if (id != workingSpace.Id)
            {
                return BadRequest();
            }

            _context.Entry(workingSpace).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkingSpaceExists(id))
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

        // POST: api/WorkingSpaces
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Employee")]
        [HttpPost("create_workspace")]
        public async Task<ActionResult<WorkingSpace>> PostWorkingSpace(WorkingSpace workingSpace)
        {
            try
            {
                _context.WorkingSpaces.Add(workingSpace);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetWorkingSpace", new { id = workingSpace.Id }, workingSpace);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        // DELETE: api/WorkingSpaces/5
        [Authorize(Roles = "Employee")]
        [HttpDelete("delete_workspace/{id}")]
        public async Task<IActionResult> DeleteWorkingSpace(int id)
        {
            var workingSpace = await _context.WorkingSpaces.FindAsync(id);
            if (workingSpace == null)
            {
                return NotFound();
            }

            _context.WorkingSpaces.Remove(workingSpace);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WorkingSpaceExists(int id)
        {
            return _context.WorkingSpaces.Any(e => e.Id == id);
        }
    }
}
