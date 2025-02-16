using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CompClubAPI.Models;
using CompClubAPI.Schemas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace CompClubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly CollegeTaskContext _context;
        private readonly IConfiguration _configuration;


        public AccountController(CollegeTaskContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Account
        [HttpGet("get_accounts")]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {
            return await _context.Accounts.ToListAsync();
        }

        // GET: api/Account/5
        [Authorize(Roles = "Admin")]
        [HttpGet("get_info/{id}")]
        public async Task<ActionResult<Account>> GetAccount(int id)
        {
            var account = await _context.Accounts.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }
        
        [Authorize(Roles = "Client")]
        [HttpGet("get_info")]
        public async Task<ActionResult<Account>> GetAccountInfo()
        {
            int accountId = Convert.ToInt32(User.FindFirst("account_id")?.Value);
            var AccountClient = await _context.Accounts.Where(a => a.Id == accountId).Include(a => a.IdClientNavigation).FirstOrDefaultAsync();

            if (AccountClient == null)
            {
                return NotFound();
            }

            return Ok(AccountClient);
        }
        
        
        // PUT: api/Account/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Admin")] // for employee
        [HttpPut("update/{id}")]
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

            return NoContent();
        }


        [Authorize(Roles = "Client")]
        [HttpPut("update")]
        public async Task<IActionResult> PutAccount(UpdateAccountModel accountModel)
        {
            int accountId = Convert.ToInt32(User.FindFirst("account_id")?.Value);
            Account? account = await _context.Accounts.Where(a => a.Id == accountId).FirstOrDefaultAsync();
            if (account == null)
            {
                return BadRequest(new { error = "Account not found!" });
            }

            account.Login = accountModel.Login;
            account.Password = HashHelper.GenerateHash(accountModel.Password); 
            account.Balance = accountModel.Balance;
            account.UpdatedAt = DateTime.Now;
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
            return Ok(new {message = "Account updated successfully!"});
        }

        
        [HttpPost("add_balance")]
        public async Task<ActionResult> AddBalance(AddBalanceModel balanceModel)
        {
            int accountId = Convert.ToInt32(User.FindFirst("account_id")?.Value);
            Account? account = await _context.Accounts.Where(a => a.Id == accountId).FirstOrDefaultAsync();
            if (account == null)
            {
                return BadRequest(new { error = "Account not found!" });
            }

            BalanceHistory history = new BalanceHistory
            {
                AccountId = accountId,
                Action = "add_balance",
                Price = balanceModel.Money,
                PreviousBalance = account.Balance
            };
            _context.BalanceHistories.Add(history);
            account.Balance += balanceModel.Money;
            account.UpdatedAt = DateTime.Now;
            _context.Accounts.Update(account);
            
            await _context.SaveChangesAsync();
            return Ok(new {message = "Balance updated successfully!"});
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPost("add_balance_by_id/{id}")]
        public async Task<ActionResult> AddBalanceById(int id, [FromQuery] int idClub, decimal money)
        {
            Account? account = await _context.Accounts.Where(a => a.Id == id).FirstOrDefaultAsync();
            if (account == null)
            {
                return BadRequest(new { error = "Account not found!" });
            }
            BalanceHistory history = new BalanceHistory
            {
                AccountId = id,
                Action = "add_balance",
                Price = money,
                PreviousBalance = account.Balance
            };
            CostRevenue revenue = new CostRevenue
            {
                IdClub = idClub,
                Amount = money,
                Revenue = true,
                CreatedAt = DateTime.Now
            };
            _context.CostRevenues.Add(revenue);
            _context.BalanceHistories.Add(history);
            account.Balance += money;
            account.UpdatedAt = DateTime.Now;
            _context.Accounts.Update(account);
            
            await _context.SaveChangesAsync();
            return Ok(new {message = "Balance updated successfully!"});
        }

        [Authorize(Roles = "Client")]
        [HttpGet("balance_history")]
        public async Task<ActionResult> GetBalanceHistory()
        {
            int accountId = Convert.ToInt32(User.FindFirst("account_id")?.Value);
            List<BalanceHistory> history = await _context.BalanceHistories.Where(h => h.AccountId == accountId).ToListAsync();
            return Ok(history);
        }
        [Authorize(Roles = "Client")]
        [HttpPost("change_password")]
        public async Task<ActionResult> ChangePassword(ChangePasswordModel changePasswordModel)
        {
            // for authorized users only
            int accountId = Convert.ToInt32(User.FindFirst("account_id")?.Value);
            Account? account = await _context.Accounts.Where(a => a.Id == accountId).FirstOrDefaultAsync();
            if (account == null)
            {
                return BadRequest(new { error = "Account not found!" });
            }
            account.Password = HashHelper.GenerateHash(changePasswordModel.Password);
            account.UpdatedAt = DateTime.Now;
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
            return Ok(new {message = "Password updated successfully!"});
        }

        [HttpPost("change_password_by_email/{email}")]
        public async Task<ActionResult> ChangePasswordByEmail(string email)
        {
            
            string to = email;
            string from = _configuration["secretData:mail"]!;
            string subject = "Your temp password";
            
            
            string randomPassword = string.Empty;
            Random random = new Random();
            for (int i = 0; i < 8; i++)
            {
                randomPassword += (char)('a' + random.Next(26));
            }
            
            string body = "Your temporary password is: " + randomPassword + "\n Please change it as soon as possible.";
            MailMessage message = new MailMessage(from, to, subject, body);
            
            SmtpClient client = new SmtpClient("smtp.gmail.com");
            client.Port = 587;
            
            client.Credentials = new NetworkCredential(_configuration["secretData:mail"], _configuration["secretData:mailPassword"]); // TODO make it more secure
            client.EnableSsl = true;
            
            client.Send(message);
            return Ok(new {message = "Password sent to email"});
        }

        [HttpPost("authentication")]
        public async Task<ActionResult> AuthClient(AuthModel authModel)
        {
            byte[] passwordHash = HashHelper.GenerateHash(authModel.password);

            Account? account = await _context.Accounts.FirstOrDefaultAsync(a => a.Login == authModel.login && a.Password.SequenceEqual(passwordHash));
            if (account == null)
            {
                return NotFound(new {error = "Client not found!"});
            }

            account.LastLogin = DateTime.Now;
            _context.Update(account);
            await _context.SaveChangesAsync();
            string token = GenerateJwtToken(account);
            return Ok(new { token });
        }
        [NonAction]
        private string GenerateJwtToken(Account account)
        {
            var secretKey = new SymmetricSecurityKey("df8f3c6058ce4d93b799b4d8dc0b5ff66e1eccf69aa29505c6c84a6339a914a4"u8.ToArray()); //TODO make it more secure
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: "CompClubAPI",
                audience: "CompClubAPI",
                claims: new[]
                {
                    new Claim("account_id", account.Id.ToString()),
                    new Claim("account_login", account.Login),
                    new Claim(ClaimTypes.Role, "Client")
                },

                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.Id == id);
        }
    }
}
