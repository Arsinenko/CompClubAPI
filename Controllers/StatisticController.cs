using CompClubAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CompClubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
        private readonly CollegeTaskContext _context;

        public StatisticController(CollegeTaskContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Owner,Admin,Marketer")]
        [HttpPut("update_finance_statistic/{id}")] // id - id клуба
        public async Task<ActionResult> UpdateFinanceStatistic(int id, decimal money, bool revenue)
        {
            Statistic? statistic = (await _context.Statistics.Where(s => s.IdClub == id).FirstOrDefaultAsync());
            if (statistic == null)
            {
                return BadRequest("Statistic not found!");
            }

            statistic.Finances = revenue ? statistic.Finances += money : statistic.Finances -= money;
            _context.Statistics.Update(statistic);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Statistic updated successfully!" });
        }
        [Authorize(Roles = "Owner,Admin,Marketer")]
        [HttpGet("get_club_finance_statistic/{id}")] // id - id клуба
        public async Task<ActionResult> GetClubStatistic(int id)
        {
            List<CostRevenue> costRevenues = await _context.CostRevenues.Where(cr => cr.IdClub == id).ToListAsync();
            decimal currentFinances = await _context.Statistics.Where(s => s.IdClub == id).Select(s => s.Finances).FirstOrDefaultAsync();
            return Ok(new { costRevenues, currentFinances });
        }
        [Authorize(Roles = "Owner,Admin,Marketer")]
        [HttpGet("statistics")]
        public async Task<ActionResult> GetStatistics()
        {
            List<Statistic> statistics = await _context.Statistics.ToListAsync();
            return Ok(statistics);
        }
        
        
    }   
}
