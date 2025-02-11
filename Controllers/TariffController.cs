using CompClubAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CompClubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TariffController : ControllerBase
    {
        public readonly CollegeTaskContext _context;

        public TariffController(CollegeTaskContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Owner,Admin,Marketer")]
        [HttpGet("get_tariffs")]
        public async Task<ActionResult<IEnumerable<Tariff>>> GetTariffs()
        {
            return await _context.Tariffs.ToListAsync();
        }
        [Authorize(Roles = "Owner,Admin,Marketer")]
        [HttpPost("create_tariff")]
        public async Task<ActionResult<Tariff>> CreateTariff(Tariff tariff)
        {
            _context.Tariffs.Add(tariff);
            await _context.SaveChangesAsync();
            return CreatedAtAction("", new { id = tariff.Id });
        }
        [Authorize(Roles = "Owner,Admin,Marketer")]
        [HttpPost("update_tariff/{id}")]
        public async Task<ActionResult<Tariff>> UpdateTariff(int id, Tariff updateTariff)
        {
            Tariff? tariff = await _context.Tariffs.FindAsync(id);
            if (tariff == null)
            {
                return BadRequest(new { error = "Tariff not found!" });
            }
            tariff.Name = updateTariff.Name;
            tariff.PricePerMinute = updateTariff.PricePerMinute;
            
            _context.Tariffs.Update(tariff);
            await _context.SaveChangesAsync();
            
            return Ok(new { message = "Tariff updated successfully!" });
        }
        [Authorize(Roles = "Owner,Admin,Marketer")]
        [HttpDelete("delete_tariff/{id}")]
        public async Task<ActionResult<Tariff>> DeleteTariff(int id)
        {
            Tariff? tariff = await _context.Tariffs.FindAsync(id);
            if (tariff == null)
            {
                return BadRequest(new { error = "Tariff not found!" });
            }
            _context.Tariffs.Remove(tariff);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Tariff deleted successfully!" });
        }
    }
}
