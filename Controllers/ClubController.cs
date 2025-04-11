using System.Security.Claims;
using CompClubAPI.Models;
using CompClubAPI.Schemas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CompClubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClubController : ControllerBase
    {
        private readonly CollegeTaskV2Context _context;

        public ClubController(CollegeTaskV2Context context)
        {
            _context = context;
        }
        [Authorize(Roles = "Owner,Admin")]
        [HttpPost("create_club")]
        public async Task<ActionResult<Club>> CreateClub(CreateClubModel model)
        {
            Club club = new Club
            {
                Address = model.Address,
                Name = model.Name,
                Phone = model.Phone,
                WorkingHours = model.WorkingHours,
            };
            _context.Clubs.Add(club);
            await _context.SaveChangesAsync();
            Statistic statistic = new Statistic
            {
                IdClub = club.Id,
                Finances = model.Finances
            };
            _context.Statistics.Add(statistic);
            await _context.SaveChangesAsync();
            return Created("", new
            {
                message = "Club created successfully!",
                idClub = club.Id,
                idStatisctic = statistic.Id
            });
        }
        
        [HttpGet("get_clubs")]
        public async Task<ActionResult> GetClubs()
        {
            List<Club> clubs = await _context.Clubs.ToListAsync();
            return Ok(new {clubs});
        }

        [Authorize(Roles = "Client")]
        [HttpGet("get_visited_clubs")]
        public async Task<ActionResult> GetVisitedClubs()
        {
            int accountId = Convert.ToInt32(User.FindFirst("accountId")!.Value);
            List<Club> clubs = await _context.Bookings.Where(b => b.TotalCost > 0 && b.AccountId == accountId)
                .Select(b => b.IdWorkingSpaceNavigation)
                .Select(ws => ws.IdClubNavigation)
                .Distinct()
                .ToListAsync();
            return Ok(new { clubs });
        }
        
        [HttpGet("get_club_info/{id}")]
        public async Task<ActionResult<Club>> GetClubInfo(int id)
        {
            Club? club = await _context.Clubs.FindAsync(id);
            if (club == null)
            {
                return BadRequest(new { error = "Club not found!" });
            }
            return Ok(new
            {
                address = club.Address,
                name = club.Name,
                phone = club.Phone,
                workingHours = club.WorkingHours,
                createdAt = club.CreatedAt
            });
        }
        [Authorize(Roles = "Owner")]
        [HttpDelete("delete_club/{id}")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            Club? club = await _context.Clubs.FindAsync(id);
            if (club == null)
            {
                return BadRequest(new { error = "Club not found!" });
            }
            _context.Clubs.Remove(club);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Club deleted successfully!" });
        }

        [Authorize(Roles = "Admin, Owner")]
        [HttpPut("update_club/{id}")]
        public async Task<ActionResult<Club>> UpdateClub(int id, UpdateClubModel model)
        {
            // var role = FindFristValue(Claim.Types)
            Club? club = await _context.Clubs.FindAsync(id);
            if (club == null)
            {
                return BadRequest(new { error = "Club not found!" });
            }
            club.Address = model.Address;
            club.Name = model.Name;
            club.Phone = model.Phone;
            club.WorkingHours = model.WorkingHours;
            club.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Club updated successfully!" });
        }
        
    }
}
