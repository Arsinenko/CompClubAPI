using CompClubAPI.Context;
using CompClubAPI.Models;
using CompClubAPI.ResponseSchema;
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
        /// <summary>
        /// Добавление оборудования. Для сотрудников. 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Owner,Admin,System_administrator")]
        [HttpPost("create_equipment")]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
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
        /// <summary>
        /// Получение информации об оборудовании по id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Owner,Admin,System_administrator")]
        [HttpGet("info/{id}")]
        [ProducesResponseType(typeof(GetEquipmentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetEquipmentInfo(int id)
        {
            var equipment = await _context.Equipment.FindAsync(id);
            if (equipment == null)
            {
                return BadRequest(new { message = "Equipment not found!" });
            }

            GetEquipmentResponse response = new GetEquipmentResponse
            {
                Equipment = equipment
            };

            return Ok(response);
        }
        /// <summary>
        /// Обновление данных об оборудовании. Для сотрудников.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="equipment"></param>
        /// <returns></returns>
        [Authorize(Roles = "Owner,Admin,System_administrator")]
        [HttpPut("update/{id}")]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
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
        /// <summary>
        /// Удаление оборудования. Только для сотрудников.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Owner,Admin,System_administrator")]
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
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
        /// <summary>
        /// Получение данных об обслуживании оборудования. Для сотрудников. 
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Owner,Admin,System_administrator")]
        [HttpGet("get_equipment_maintenances")]
        [ProducesResponseType(typeof(GetEquipmentMaintenanceResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<EquipmentMaintenance>>> GetEquipmentMaintenances()
        {
            List<EquipmentMaintenance> equipmentMaintenances = await _context.EquipmentMaintenances.ToListAsync();
            
            return Ok(new { equipmentMaintenances });
        }
        /// <summary>
        /// Добавление данных об обслуживании оборудования. Для сотрудников.
        /// </summary>
        /// <param name="equipmentMaintenance"></param>
        /// <returns></returns>
        [Authorize(Roles = "Owner,Admin,System_administrator")]
        [HttpPost("create_equipment_maintenance")]
        [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
        public async Task<ActionResult> CreateEquipmentMaintenance(CreateEquipmentMaintenanceModel equipmentMaintenance)
        {
            EquipmentMaintenance maintenance = new EquipmentMaintenance
            {
                EquipmentId = equipmentMaintenance.EquipmentId,
                MaintenanceDate = equipmentMaintenance.MaintenanceDate,
                Cost = equipmentMaintenance.Cost,
                Description = string.IsNullOrEmpty(equipmentMaintenance.Description) ? null : equipmentMaintenance.Description,
                CreatedAt = DateTime.Now
            };
            _context.EquipmentMaintenances.Add(maintenance);
            await _context.SaveChangesAsync();
            return Created("", new { message = "Equipment maintenance created successfully!" });
        }
        
        /// <summary>
        /// Обновление данных об обслуживании оборудования. Для сотрудников.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="equipmentMaintenance"></param>
        /// <returns></returns>
        [Authorize(Roles = "Owner,Admin,System_administrator")]
        [HttpPut("update_equipment_maintenance/{id}")]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
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
        /// <summary>
        /// Получение данных об оборудовании. Для сотрудников.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Owner,Admin,System_administrator")]
        [HttpGet("get_equipment")]
        [ProducesResponseType(typeof(GetEquipmentsSchema), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Equipment>>> GetEquipment()
        {
            List<Equipment> equipments = await _context.Equipment.ToListAsync();
            return Ok(new {equipments});
        }
        
        /// <summary>
        /// Получении оборудования по id клуба. Для сотрудников.
        /// </summary>
        /// <param name="idClub"></param>
        /// <returns></returns>
        [Authorize(Roles = "Owner,Admin,System_administrator")]
        [HttpGet("get_equipment_by_club/{idClub}")]
        [ProducesResponseType(typeof(GetEquipmentsSchema), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Equipment>>> GetEquipmentByClub(int idClub)
        {
            List<Equipment> equipments = await _context.Equipment.Where(e => e.IdClub == idClub).ToListAsync();
            return Ok(new {equipments});
        }
    }
}
