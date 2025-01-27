using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CompClubAPI.Models;
using CompClubAPI.Schemas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;

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

        // // GET: api/Payments/5
        // [Authorize(Roles = "Client")]
        // [HttpGet("get_info")]
        // public async Task<ActionResult<Payment>> GetPayment()
        // {
        //     int clientId = Convert.ToInt32(User.FindFirst("client_id")?.Value);
        //     var payment = await _context.Payments.Where(p => p.ClientId == clientId).FirstOrDefaultAsync();
        //
        //     if (payment == null)
        //     {
        //         return NotFound(new {error = "Payment not found!"});
        //     }
        //
        //     return Ok(new { cardNumber = AesEncryption.Decrypt(payment.EncryptedCardNumber), cvv = AesEncryption.Decrypt(payment.EncryptedCvv), date = payment.LinkDate })>;
        // }

        // PUT: api/Payments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> PutPayment(int id, Payment payment)
        {
            if (id != payment.Id)
            {
                return BadRequest();
            }

            _context.Entry(payment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentExists(id))
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

        // POST: api/Payments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Client")]
        [HttpPost("create")]
        public async Task<ActionResult<Payment>> PostPayment(CreatePaymentModel paymentModel)
        {
            int clientId = Convert.ToInt32(User.FindFirst("client_id")?.Value);
            Payment payment = new Payment
            {
                ClientId = clientId,
                EncryptedCardNumber = AesEncryption.Encrypt(paymentModel.CardNumber),
                EncryptedCvv = AesEncryption.Encrypt(paymentModel.Cvv),
                LinkDate = paymentModel.Date,
            };
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPayment", new { id = payment.Id });
        }

        // DELETE: api/Payments/5
        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
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
