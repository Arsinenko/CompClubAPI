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
        private readonly CollegeTaskContext _context;

        public PaymentsController(CollegeTaskContext context)
        {
            _context = context;
        }

        // GET: api/Payments
        [Authorize(Roles = "Employee")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPayments()
        {
            return await _context.Payments.ToListAsync();
        }

        // GET: api/Payments/5
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
                id = payment.Id, cardNumber = AesEncryption.Decrypt(payment.EncryptedCardNumber),
                cvv = AesEncryption.Decrypt(payment.EncryptedCvv), date = payment.LinkDate
            });
        }


        // POST: api/Payments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Client")]
        [HttpPost("create")]
        public async Task<ActionResult> PostPayment(CreatePaymentModel paymentModel)
        {
            try
            {
                int accountId = Convert.ToInt32(User.FindFirst("client_id")?.Value);
                var paymentExists = await _context.Payments.Where(p => p.AccountId == accountId).FirstOrDefaultAsync();
                
                if (paymentExists == null)
                {
                    Payment payment = new Payment
                    {
                        
                        AccountId = accountId,
                        EncryptedCardNumber = AesEncryption.Encrypt(paymentModel.CardNumber),
                        EncryptedCvv = AesEncryption.Encrypt(paymentModel.Cvv),
                        LinkDate = DateOnly.FromDateTime(DateTime.Now)
                        
                    };
                    _context.Payments.Add(payment);
                    await _context.SaveChangesAsync();
                    return Created("", new { id = payment.Id});
                }
                
                paymentExists.EncryptedCardNumber = AesEncryption.Encrypt(paymentModel.CardNumber);
                paymentExists.EncryptedCvv = AesEncryption.Encrypt(paymentModel.Cvv);
                paymentExists.LinkDate = DateOnly.FromDateTime(DateTime.Now);
                
                _context.Payments.Update(paymentExists);
                await _context.SaveChangesAsync();
                
                return Ok( new { id = paymentExists.Id });
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        // DELETE: api/Payments/5
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