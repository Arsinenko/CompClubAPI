using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CompClubAPI.Models;

namespace CompClubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameLaunchersController : ControllerBase
    {
        private readonly CollegeTaskContext _context;

        public GameLaunchersController(CollegeTaskContext context)
        {
            _context = context;
        }

        // GET: api/GameLaunchers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameLauncher>>> GetGameLaunchers()
        {
            return await _context.GameLaunchers.ToListAsync();
        }

        // GET: api/GameLaunchers/5
        [HttpGet("get_info/{id}")]
        public async Task<ActionResult<GameLauncher>> GetGameLauncher(int id)
        {
            var gameLauncher = await _context.GameLaunchers.FindAsync(id);

            if (gameLauncher == null)
            {
                return NotFound();
            }

            return gameLauncher;
        }

        // PUT: api/GameLaunchers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("update_data/{id}")]
        public async Task<IActionResult> PutGameLauncher(int id, GameLauncher gameLauncher)
        {
            if (id != gameLauncher.Id)
            {
                return BadRequest();
            }

            _context.Entry(gameLauncher).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameLauncherExists(id))
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

        // POST: api/GameLaunchers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("add_launcher")]
        public async Task<ActionResult<GameLauncher>> PostGameLauncher(GameLauncher gameLauncher)
        {
            _context.GameLaunchers.Add(gameLauncher);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGameLauncher", new { id = gameLauncher.Id }, gameLauncher);
        }

        // DELETE: api/GameLaunchers/5
        [HttpDelete("delete_launcher/{id}")]
        public async Task<IActionResult> DeleteGameLauncher(int id)
        {
            var gameLauncher = await _context.GameLaunchers.FindAsync(id);
            if (gameLauncher == null)
            {
                return NotFound();
            }

            _context.GameLaunchers.Remove(gameLauncher);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GameLauncherExists(int id)
        {
            return _context.GameLaunchers.Any(e => e.Id == id);
        }
    }
}
