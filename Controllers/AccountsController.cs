using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CompClubAPI.Models;
using CompClubAPI.Schemas;
using Microsoft.AspNetCore.Authorization;

namespace CompClubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly CollegeTaskContext _context;

        public AccountsController(CollegeTaskContext context)
        {
            _context = context;
        }

        // GET: api/Accounts
        [Authorize(Roles = "Employee")]
        [HttpGet("get_accounts")]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {
            return await _context.Accounts.ToListAsync();
        }

        // GET: api/Accounts/5
        [Authorize]
        [HttpGet("get_info")]
        public async Task<ActionResult<Account>> GetAccount()
        {
            int clientId = Convert.ToInt32(User.FindFirst("client_id")?.Value);

            var account = await _context.Accounts.Where(a => a.IdClient == clientId).FirstOrDefaultAsync();

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }

        // PUT: api/Accounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Client")]
        [HttpPut("add_balance/")]
        public async Task<IActionResult> PutAccount(CreateAccountModel accountModel)
        {
            int clientId = Convert.ToInt32(User.FindFirst("client_id")?.Value);
            
            Account? account = await _context.Accounts.Where(a => a.IdClient == clientId).FirstOrDefaultAsync();
            if (account == null)
            {
                return BadRequest(new { error = "Account not found!" });
            }

            account.Balance += accountModel.Balance;
            _context.Update(account);
            await _context.SaveChangesAsync();
            UserActionLog log = new UserActionLog
            {
                ClientId = clientId,
                Action = "add_balance",
                Price = accountModel.Balance
            };
            _context.UserActionLogs.Add(log);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Account updated successfully!" });

        }
        
        [Authorize(Roles = "Employee")]
        [HttpPut("update_account/{id}")]
        public async Task<IActionResult> PutAccount(int id, Account account)
        {
            if (id != account.Id)
            {
                return BadRequest();
            }

            _context.Entry(account).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { message = "Account updated successfully!" });
        }

        // POST: api/Accounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost("create_account")]
        public async Task<ActionResult<Account>> PostAccount(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAccount", new { id = account.Id }, account);
        }

        // DELETE: api/Accounts/5
        [Authorize]
        [HttpDelete("delete_account/{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.Id == id);
        }
    }
}
