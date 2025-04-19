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
    public class TariffController : ControllerBase
    {
        public readonly CollegeTaskV2Context _context;

        public TariffController(CollegeTaskV2Context context)
        {
            _context = context;
        }

        /// <summary>
        /// Получение списка всех тарифов.
        /// </summary>
        /// <returns>Список тарифов</returns>
        /// <remarks>Доступно для ролей Owner, Admin, Marketer</remarks>
        [Authorize(Roles = "Owner,Admin,Marketer")]
        [HttpGet("get_tariffs")]
        public async Task<ActionResult<IEnumerable<Tariff>>> GetTariffs()
        {
            List<Tariff> tariffs = await _context.Tariffs.ToListAsync();
            return Ok(new { tariffs });
        }

        /// <summary>
        /// Создание нового тарифа.
        /// </summary>
        /// <param name="tariffModel">Модель тарифа</param>
        /// <returns>Созданный тариф</returns>
        /// <remarks>Доступно для ролей Owner, Admin, Marketer</remarks>
        [Authorize(Roles = "Owner,Admin,Marketer")]
        [HttpPost("create_tariff")]
        public async Task<ActionResult<Tariff>> CreateTariff(CreateTariffModel tariffModel)
        {
            Tariff tariff = new Tariff
            {
                Name = tariffModel.Name,
                PricePerMinute = tariffModel.PricePerMinute
            };
            _context.Tariffs.Add(tariff);
            await _context.SaveChangesAsync();
            return Ok(new { tariff });
        }

        /// <summary>
        /// Обновление существующего тарифа.
        /// </summary>
        /// <param name="id">Идентификатор тарифа</param>
        /// <param name="updateTariff">Модель обновления тарифа</param>
        /// <returns>Сообщение об успешном обновлении</returns>
        /// <remarks>Доступно для ролей Owner, Admin, Marketer</remarks>
        [Authorize(Roles = "Owner,Admin,Marketer")]
        [HttpPut("update_tariff/{id}")]
        public async Task<ActionResult<Tariff>> UpdateTariff(int id, UpdateTariffModel updateTariff)
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

        /// <summary>
        /// Удаление тарифа.
        /// </summary>
        /// <param name="id">Идентификатор тарифа</param>
        /// <returns>Сообщение об успешном удалении</returns>
        /// <remarks>Доступно для ролей Owner, Admin, Marketer</remarks>
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
