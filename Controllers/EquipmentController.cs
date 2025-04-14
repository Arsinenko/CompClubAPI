using CompClubAPI.Context;
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
    public class EquipmentController : ControllerBase
    {
        private readonly CollegeTaskV2Context _context;

        public EquipmentController(CollegeTaskV2Context context)
        {
            _context = context;
        }
        [Authorize(Roles = "Owner,Admin,System_administrator")]
        [HttpPost("create_equipment")]
        public async Task<ActionResult> CreateEquipment(CreateEquipmentModel model)
        {
            Equipment equipment = new Equipment
            {
                Type = model.Type,
                Name = model.Name,
                Specification = model.Specification,
                PurchasePrice = model.PurchasePrice,
                PurchaseDate = DateOnly.FromDateTime(DateTime.Now),
                IdClub = model.IdClub,
                Status = 1,
                Quantity = 1
            };
            Statistic? statistic = await _context.Statistics.Where(s => s.IdClub == model.IdClub).FirstOrDefaultAsync();
            if (statistic == null)
            {
                return BadRequest(new {error = "Statistic not found!"});
            }
            statistic.Finances -= model.PurchasePrice;
            _context.Update(statistic);
            CostRevenue costRevenue = new CostRevenue
            {
                IdClub = model.IdClub,
                Amount = model.PurchasePrice,
                Revenue = false,
                CreatedAt = DateTime.Now
            };
            await _context.CostRevenues.AddAsync(costRevenue);
            await _context.Equipment.AddAsync(equipment);
            await _context.SaveChangesAsync();
            return Created("", new { message = "Equipment created successfully!", id = equipment.Id });
        }
        [Authorize(Roles = "Owner,Admin,System_administrator")]
        [HttpGet("info/{id}")]
        public async Task<ActionResult> GetEquipmentInfo(int id)
        {
            var equipment = await _context.Equipment.FindAsync(id);
            if (equipment == null)
            {
                return BadRequest(new { message = "Equipment not found!" });
            }

            return Ok(equipment);
        }
        [Authorize(Roles = "Owner,Admin,System_administrator")]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateEquipment(int id, Equipment equipment)
        {
            if (id != equipment.Id)
            {
                return BadRequest(new { error = "Equipment not found!" });
            }

            _context.Update(equipment);
            await _context.SaveChangesAsync();
            return Ok(new { message = $"Equipment with id {equipment.Id} updated successfully!" });
        }
        [Authorize(Roles = "Owner,Admin,System_administrator")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteEquipment(int id)
        {
            Equipment? equipment = await _context.Equipment.FindAsync(id);
            if (equipment == null)
            {
                return BadRequest(new { error = "Equipment not found!" });
            }

            _context.Equipment.Remove(equipment);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Equipment deleted successfully!" });
        }
        
        [Authorize(Roles = "Owner,Admin,System_administrator")]
        [HttpGet("EquipmentMaintenances")]
        public async Task<ActionResult<List<EquipmentMaintenance>>> GetEquipmentMaintenances()
        {
            List<EquipmentMaintenance> equipmentMaintenances = await _context.EquipmentMaintenances.ToListAsync();
            return Ok(equipmentMaintenances);
        }
        
        [Authorize(Roles = "Owner,Admin,System_administrator")]
        [HttpPost("create_equipment_maintenance")]
        public async Task<ActionResult> CreateEquipmentMaintenance(EquipmentMaintenance equipmentMaintenance)
        {
            _context.EquipmentMaintenances.Add(equipmentMaintenance);
            await _context.SaveChangesAsync();
            return Created("",
                new { message = "Equipment maintenance created successfully!", id = equipmentMaintenance.Id });
        }
        
        [Authorize(Roles = "Owner,Admin,System_administrator")]
        [HttpPut("update_equipment_maintenance/{id}")]
        public async Task<IActionResult> UpdateEquipmentMaintenance(int id, EquipmentMaintenance equipmentMaintenance)
        {
            if (id != equipmentMaintenance.Id)
            {
                return BadRequest(new { error = "Equipment maintenance not found!" });
            }

            _context.Update(equipmentMaintenance);
            await _context.SaveChangesAsync();
            return Ok(new
                { message = $"Equipment maintenance with id {equipmentMaintenance.Id} updated successfully!" });
        }
    }
}
