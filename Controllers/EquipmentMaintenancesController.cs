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
    public class EquipmentMaintenancesController : ControllerBase
    {
        private readonly CollegeTaskContext _context;

        public EquipmentMaintenancesController(CollegeTaskContext context)
        {
            _context = context;
        }

        // GET: api/EquipmentMaintenances
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipmentMaintenance>>> GetEquipmentMaintenances()
        {
            return await _context.EquipmentMaintenances.ToListAsync();
        }

        // GET: api/EquipmentMaintenances/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EquipmentMaintenance>> GetEquipmentMaintenance(int id)
        {
            var equipmentMaintenance = await _context.EquipmentMaintenances.FindAsync(id);

            if (equipmentMaintenance == null)
            {
                return NotFound();
            }

            return equipmentMaintenance;
        }

        // PUT: api/EquipmentMaintenances/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEquipmentMaintenance(int id, EquipmentMaintenance equipmentMaintenance)
        {
            if (id != equipmentMaintenance.Id)
            {
                return BadRequest();
            }

            _context.Entry(equipmentMaintenance).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EquipmentMaintenanceExists(id))
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

        // POST: api/EquipmentMaintenances
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EquipmentMaintenance>> PostEquipmentMaintenance(EquipmentMaintenance equipmentMaintenance)
        {
            _context.EquipmentMaintenances.Add(equipmentMaintenance);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEquipmentMaintenance", new { id = equipmentMaintenance.Id }, equipmentMaintenance);
        }

        // DELETE: api/EquipmentMaintenances/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEquipmentMaintenance(int id)
        {
            var equipmentMaintenance = await _context.EquipmentMaintenances.FindAsync(id);
            if (equipmentMaintenance == null)
            {
                return NotFound();
            }

            _context.EquipmentMaintenances.Remove(equipmentMaintenance);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EquipmentMaintenanceExists(int id)
        {
            return _context.EquipmentMaintenances.Any(e => e.Id == id);
        }
    }
}
