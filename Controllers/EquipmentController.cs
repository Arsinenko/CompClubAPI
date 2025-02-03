using CompClubAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CompClubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentController : ControllerBase
    {
        private readonly CollegeTaskContext _context;

        private EquipmentController(CollegeTaskContext context)
        {
            _context = context;
        }

        [HttpPost("create_equipment")]
        public async Task<ActionResult> CreateEquipment(Equipment equipment)
        {
            _context.Add(equipment);
            await _context.SaveChangesAsync();
            return Created("", new { message = "Equipment created successfully!", id = equipment.Id });
        }

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

        [HttpGet("EquipmentMaintenances")]
        public async Task<ActionResult<List<EquipmentMaintenance>>> GetEquipmentMaintenances()
        {
            List<EquipmentMaintenance> equipmentMaintenances = await _context.EquipmentMaintenances.ToListAsync();
            return Ok(equipmentMaintenances);
        }

        [HttpPost("create_equipment_maintenance")]
        public async Task<ActionResult> CreateEquipmentMaintenance(EquipmentMaintenance equipmentMaintenance)
        {
            _context.EquipmentMaintenances.Add(equipmentMaintenance);
            await _context.SaveChangesAsync();
            return Created("",
                new { message = "Equipment maintenance created successfully!", id = equipmentMaintenance.Id });
        }

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
