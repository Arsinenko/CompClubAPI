using CompClubAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CompClubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClubController : ControllerBase
    {
        private readonly CollegeTaskContext _context;

        public ClubController(CollegeTaskContext context)
        {
            _context = context;
        }
        
        [HttpPost("create_club")]
        public async Task<ActionResult<Club>> CreateClub(Club club)
        {
            _context.Add(club);
            await _context.SaveChangesAsync();
            return Created("", new {message = "Club created successfully!", id = club.Id});
        }
        
        [HttpGet("get_clubs")]
        public async Task<ActionResult> GetClubs()
        {
            List<Club> clubs = await _context.Clubs.ToListAsync();
            return Ok(clubs);
        }
        
        [HttpGet("get_club_info/{id}")]
        public async Task<ActionResult<Club>> GetClubInfo(int id)
        {
            Club? club = await _context.Clubs.FindAsync(id);
            return Ok(new
            {
                address = club.Address,
                name = club.Name,
                phone = club.Phone,
                workingHours = club.WorkingHours,
                createdAt = club.CreatedAt
            });
        }
        
    }
}
