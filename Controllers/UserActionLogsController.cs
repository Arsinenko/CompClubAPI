﻿using System;
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
    public class UserActionLogsController(CollegeTaskContext context) : ControllerBase
    {
        private readonly CollegeTaskContext _context = context;

        // GET: api/UserActionLogs
        [Authorize(Roles = "Employee")]
        [HttpGet("get_user_logs")]
        public async Task<ActionResult<IEnumerable<UserActionLog>>> GetUserActionLogs()
        {
            return await _context.UserActionLogs.ToListAsync();
        }

        // GET: api/UserActionLogs/5
        [Authorize(Roles = "Client")]
        [HttpGet("get_logs")]
        public async Task<IActionResult> GetUserActionLog()
        {
            int clientId = Convert.ToInt32(User.FindFirst("client_id")?.Value);
            var userActionLogs = await _context.UserActionLogs.Where(log => log.ClientId == clientId).ToListAsync();
            if (userActionLogs == null)
            {
                return NotFound(new {message = "there is no logs"});
            }
            return Ok(userActionLogs);
        }

        // PUT: api/UserActionLogs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("update_log/{id}")]
        public async Task<IActionResult> PutUserActionLog(int id, UserActionLog userActionLog)
        {
            if (id != userActionLog.Id)
            {
                return BadRequest();
            }

            _context.Entry(userActionLog).State = EntityState.Modified;

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
        public async Task<ActionResult<UserActionLog>> PostUserActionLog(UserActionLog userActionLog)
        {
            _context.UserActionLogs.Add(userActionLog);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserActionLog", new { id = userActionLog.Id }, userActionLog);
        }

        
        private bool UserActionLogExists(int id)
        {
            return _context.UserActionLogs.Any(e => e.Id == id);
        }
    }
}
