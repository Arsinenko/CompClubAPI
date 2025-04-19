using CompClubAPI.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CompClubAPI.Models;
using CompClubAPI.Schemas;
using Microsoft.AspNetCore.Authorization;

namespace CompClubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly CollegeTaskV2Context _context;
        private AesEncryption _encryption;

        public PaymentsController(CollegeTaskV2Context context)
        {
            _context = context;
        }

        // GET: api/Payments
        // [Authorize(Roles = "Employee")]
        // [HttpGet]
        // public async Task<ActionResult<IEnumerable<Payment>>> GetPayments()
        // {
        //     return await _context.Payments.ToListAsync();
        // }

        /// <summary>
        /// Получение информации о платежных данных текущего пользователя.
        /// </summary>
        /// <returns>Информация о платежных данных</returns>
        /// <remarks>Доступно только для роли Client</remarks>
        [Authorize(Roles = "Client")]
        [HttpGet("get_info")]
        public async Task<ActionResult<Payment>> GetPayment()
        {
            int accountId = Convert.ToInt32(User.FindFirst("account_id")?.Value);
            var payment = await _context.Payments.Where(p => p.AccountId == accountId).FirstOrDefaultAsync();
        
            if (payment == null)
            {
                return NotFound(new {error = "Payment not found!"});
            }
        
            return Ok(new
            {
                id = payment.Id,
                cardNumber = _encryption.Decrypt(payment.EncryptedCardNumber),
                cvv = _encryption.Decrypt(payment.EncryptedCvv),
                createdAt = payment.CreatedAt
            });
        }

        /// <summary>
        /// Создание или обновление платежных данных.
        /// </summary>
        /// <param name="paymentModel">Модель платежных данных</param>
        /// <returns>Идентификатор созданных/обновленных платежных данных</returns>
        /// <remarks>Доступно только для роли Client</remarks>
        [Authorize(Roles = "Client")]
        [HttpPost("create")]
        public async Task<ActionResult> PostPayment(CreatePaymentModel paymentModel)
        {
            try
            {
                int accountId = Convert.ToInt32(User.FindFirst("account_id")?.Value);
                var paymentExists = await _context.Payments.Where(p => p.AccountId == accountId)
                    .FirstOrDefaultAsync();
                
                if (paymentExists == null)
                {
                    Payment payment = new Payment
                    {
                        
                        AccountId = accountId,
                        EncryptedCardNumber = _encryption.Encrypt(paymentModel.CardNumber),
                        EncryptedCvv = _encryption.Encrypt(paymentModel.Cvv)
                        
                    };
                    _context.Payments.Add(payment);
                    await _context.SaveChangesAsync();
                    return Created("", new { id = payment.Id});
                }
                
                paymentExists.EncryptedCardNumber = _encryption.Encrypt(paymentModel.CardNumber);
                paymentExists.EncryptedCvv = _encryption.Encrypt(paymentModel.Cvv);
                
                _context.Payments.Update(paymentExists);
                await _context.SaveChangesAsync();
                
                return Ok( new { id = paymentExists.Id });
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        /// <summary>
        /// Удаление платежных данных текущего пользователя.
        /// </summary>
        /// <returns>Результат операции</returns>
        /// <remarks>Требуется авторизация</remarks>
        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeletePayment()
        {
            int accountId = Convert.ToInt32(User.FindFirst("account_id")?.Value);
            Payment? payment = await _context.Payments.Where(p => p.AccountId == accountId).FirstOrDefaultAsync();
            if (payment == null)
            {
                return BadRequest(new { error = "Payment not found!" });
            }
            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PaymentExists(int id)
        {
            return _context.Payments.Any(e => e.Id == id);
        }
    }
}