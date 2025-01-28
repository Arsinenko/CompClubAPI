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
    public class UserActionLogsController : ControllerBase
    {
        private readonly CollegeTaskContext _context;

        public UserActionLogsController(CollegeTaskContext context)
        {
            _context = context;
        }

        // GET: api/UserActionLogs
        [Authorize(Roles = "Employee")]
        [HttpGet("get_userlogs")]
        public async Task<ActionResult<IEnumerable<BalanceHistory>>> GetUserActionLogs()
        {
            return await _context.UserActionLogs.ToListAsync();
        }

        // GET: api/UserActionLogs/5
        [Authorize(Roles = "Employee")]
        [HttpGet("get_log/{id}")]
        public async Task<ActionResult<BalanceHistory>> GetUserActionLog(int id)
        {
            var userActionLog = await _context.UserActionLogs.FindAsync(id);

            if (userActionLog == null)
            {
                return NotFound();
            }

            return userActionLog;
        }

        // PUT: api/UserActionLogs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("update_log/{id}")]
        public async Task<IActionResult> PutUserActionLog(int id, BalanceHistory balanceHistory)
        {
            if (id != balanceHistory.Id)
            {
                return BadRequest();
            }

            _context.Entry(balanceHistory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserActionLogExists(id))
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

        // POST: api/UserActionLogs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("create_log")]
        public async Task<ActionResult<BalanceHistory>> PostUserActionLog(BalanceHistory balanceHistory)
        {
            _context.UserActionLogs.Add(balanceHistory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserActionLog", new { id = balanceHistory.Id }, balanceHistory);
        }

        
        private bool UserActionLogExists(int id)
        {
            return _context.UserActionLogs.Any(e => e.Id == id);
        }
    }
}
