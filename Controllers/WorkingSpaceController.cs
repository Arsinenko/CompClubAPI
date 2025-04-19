using System.Security.Claims;
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
    public class WorkingSpaceController : ControllerBase
    {
        private readonly CollegeTaskV2Context _context;

        public WorkingSpaceController(CollegeTaskV2Context context)
        {
            _context = context;
        }

        /// <summary>
        /// Создание нового рабочего места.
        /// </summary>
        /// <param name="model">Модель рабочего места</param>
        /// <returns>Сообщение об успешном создании и идентификатор</returns>
        /// <remarks>Доступно для ролей Owner, Admin, System_administrator</remarks>
        [Authorize(Roles = "Owner,Admin,System_administrator")]
        [HttpPost("create_working_space")]
        public async Task<ActionResult> CreateWorkingSpace(CreateWorkingSpace model)
        {
            WorkingSpace workingSpace = new WorkingSpace
            {
                IdClub = model.IdClub,
                Name = model.Name,
                Status = model.Status,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                TariffId = model.IdTariff
            };
            _context.WorkingSpaces.Add(workingSpace);
            await _context.SaveChangesAsync();
            return Created("", new { message = "Working space created successfully!", id = workingSpace.Id });
        }

        /// <summary>
        /// Получение списка всех рабочих мест.
        /// </summary>
        /// <returns>Список рабочих мест</returns>
        /// <remarks>Доступно для ролей Owner, Admin, System_administrator</remarks>
        [Authorize(Roles = "Owner,Admin,System_administrator")]
        [HttpGet("working_spaces")]
        public async Task<ActionResult> GetWorkingSpaces()
        {
            List<WorkingSpace> workingSpaces = await _context.WorkingSpaces.ToListAsync();
            return Ok(new {workingSpaces});
        }

        /// <summary>
        /// Получение рабочих мест по клубу.
        /// </summary>
        /// <param name="idClub">Идентификатор клуба</param>
        /// <returns>Список рабочих мест клуба</returns>
        /// <remarks>Доступно для ролей Owner, Admin, System_administrator, Client</remarks>
        [Authorize(Roles = "Owner,Admin,System_administrator,Client")]
        [HttpGet("working_spaces_by_club/{idClub}")]
        public async Task<ActionResult> GetWorkingSpacesByClub(int idClub)
        {
            var role = User.FindFirstValue(ClaimTypes.Role);
            IQueryable<WorkingSpace> query = _context.WorkingSpaces
                .Where(ws => ws.IdClub == idClub);
            if (role != "Client")
            {
                query = query.Include(ws => ws.Bookings);
            }
            List<WorkingSpace> workingSpaces = await query.ToListAsync();
            return Ok(new {workingSpaces});
        }

        /// <summary>
        /// Получение информации о рабочем месте.
        /// </summary>
        /// <param name="id">Идентификатор рабочего места</param>
        /// <returns>Информация о рабочем месте</returns>
        [HttpGet("get_info/{id}")]
        public async Task<ActionResult> GetWorkingSpace(int id)
        {
            WorkingSpace? workingSpace = await _context.WorkingSpaces.FindAsync(id);
            return Ok(workingSpace);
        }

        /// <summary>
        /// Обновление информации о рабочем месте.
        /// </summary>
        /// <param name="id">Идентификатор рабочего места</param>
        /// <param name="workingSpace">Модель рабочего места</param>
        /// <returns>Сообщение об успешном обновлении</returns>
        /// <remarks>Доступно для ролей Owner, Admin, System_administrator</remarks>
        [Authorize(Roles = "Owner,Admin,System_administrator")]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateWorkingSpace(int id, CreateWorkingSpace workingSpace)
        {
            WorkingSpace? workingSpaceExists = await _context.WorkingSpaces.FindAsync(id);

            if (workingSpaceExists == null)
            {
                return BadRequest(new { error = "Working space not found!" });
            }

            workingSpaceExists.IdClub = workingSpace.IdClub;
            workingSpaceExists.Name = workingSpace.Name;
            workingSpaceExists.Status = workingSpace.Status;
            workingSpaceExists.UpdatedAt = DateTime.Now;

            _context.WorkingSpaces.Update(workingSpaceExists);
            await _context.SaveChangesAsync();
            return Ok(new { message = $"Working space with id {workingSpaceExists.Id} updated successfully!" });
        }

        /// <summary>
        /// Удаление рабочего места.
        /// </summary>
        /// <param name="id">Идентификатор рабочего места</param>
        /// <returns>Сообщение об успешном удалении</returns>
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteWorkingSpace(int id)
        {
            WorkingSpace? workingSpace = await _context.WorkingSpaces.FindAsync(id);
            if (workingSpace == null)
            {
                return BadRequest(new { error = "Working space not found!" });
            }
            _context.WorkingSpaces.Remove(workingSpace);
            await _context.SaveChangesAsync();
            return Ok(new { message = $"Working space with id {workingSpace.Id} deleted successfully!" });
        }

        /// <summary>
        /// Добавление оборудования к рабочему месту.
        /// </summary>
        /// <param name="workingSpaceEquipment">Модель оборудования рабочего места</param>
        /// <returns>Сообщение об успешном добавлении</returns>
        [HttpPost("add_equipment")]
        public async Task<ActionResult> AddEquipmentToWorkingSpace(WorkingSpaceEquipment workingSpaceEquipment)
        {
            _context.Add(workingSpaceEquipment);
            await _context.SaveChangesAsync();
            return Created("", new { message = "Equipment added to working space successfully!" });
        }

        /// <summary>
        /// Удаление оборудования с рабочего места.
        /// </summary>
        /// <param name="id">Идентификатор оборудования рабочего места</param>
        /// <returns>Сообщение об успешном удалении</returns>
        [HttpDelete("delete_equipment/{id}")]
        public async Task<ActionResult> DeleteEquipmentFromWorkingSpace(int id)
        {
            WorkingSpaceEquipment? workingSpaceEquipment = await _context.WorkingSpaceEquipments.FindAsync(id);
            if (workingSpaceEquipment == null)
            {
                return BadRequest(new { error = "Working space equipment not found!" });
            }
            _context.WorkingSpaceEquipments.Remove(workingSpaceEquipment);
            await _context.SaveChangesAsync();
            return Ok(new { message = $"Working space equipment with id {workingSpaceEquipment.Id} deleted successfully!" });
        }
    }
}
