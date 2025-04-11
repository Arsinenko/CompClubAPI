using System.Security.Claims;
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
        private readonly CollegeTaskV2Context _context;

        public StatisticController(CollegeTaskV2Context context)
        {
            _context = context;
        }
        [Authorize(Roles = "Owner,Admin,Marketer")]
        [HttpPut("update_finance_statistic/{id}")] // id - id клуба
        public async Task<ActionResult> UpdateFinanceStatistic(int id, decimal money, bool revenue)
        {
            var user = HttpContext.User;
            if (CheckAdmin(id, user, out ObjectResult result)) return result;
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
        [NonAction]
        private bool CheckAdmin(int id, ClaimsPrincipal user, out ObjectResult statusCode)
        {
            if (user.IsInRole("Admin"))
            {
                // получаем id клуба к которому принадлежит админ и сравниваем его с текущим id 
                int employeeId = int.Parse(user.Claims.FirstOrDefault(c => c.Type == "employee_id")!.Value);
                int clubId = _context.Employees.Where(e => e.Id == employeeId)
                    .Select(e => e.IdClub)
                    .FirstOrDefault();
                if (id != clubId)
                {
                    statusCode = StatusCode(StatusCodes.Status403Forbidden, new {error = "\"Access Denied. Admin can view only his own club.\""});
                    return true;
                    
                }
                    
            }

            statusCode = null!;
            return false;
        }

        [Authorize(Roles = "Owner,Admin,Marketer")]
        [HttpGet("get_club_finance_statistic/{id}")] // id - id клуба
        public async Task<ActionResult> GetClubStatistic(int id)
        {
            var user = HttpContext.User;
            if (CheckAdmin(id, user, out ObjectResult result)) return result;
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
