using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using CompClubAPI.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CompClubAPI.Models;
using CompClubAPI.ResponseSchema;
using CompClubAPI.Schemas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace CompClubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly CollegeTaskV2Context _context;
        private readonly IConfiguration _configuration;


        public AccountController(CollegeTaskV2Context context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Account
        [Authorize(Roles = "Admin, Owner")]
        [HttpGet("get_accounts")]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {
            List<Account> accounts = await _context.Accounts.Include(a => a.IdClientNavigation).ToListAsync();
            return Ok(new { accounts });
        }

        // GET: api/Account/5
        [Authorize(Roles = "Admin")]
        [HttpGet("get_info/{id}")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<Account>> GetAccount(int id)
        {
            var account = await _context.Accounts.FindAsync(id);

            if (account == null)
            {
                return BadRequest(new ErrorResponse{Error = "Account not found."});
            }

            return Ok(new {account});
        }
        
        [Authorize(Roles = "Client")]
        [HttpGet("get_info")]
        [ProducesResponseType(typeof(Account), StatusCodes.Status200OK)] // Успешный ответ[]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)] // Ошибка 400
        public async Task<ActionResult<Account>> GetAccountInfo()
        {
            int accountId = Convert.ToInt32(User.FindFirst("account_id")?.Value);
            var accountClient = await _context.Accounts.Where(a => a.Id == accountId).Include(a => a.IdClientNavigation).FirstOrDefaultAsync();

            if (accountClient == null)
            {
                return BadRequest(new ErrorResponse{Error = "Account not found."});
            }

            return Ok(accountClient);
        }
        
        
        // PUT: api/Account/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Admin, Owner")] // for employee
        [HttpPut("update/{id}")]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)] // Успешный ответ[]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)] // Ошибка 400
        public async Task<IActionResult> PutAccount(int id, UpdateAccountByIdModel accountModel)
        {
            Account? account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return BadRequest(new ErrorResponse{ Error = "Account not found." });
            }

            account.Login = string.IsNullOrWhiteSpace(accountModel.Login) ? account.Login : accountModel.Login;
            account.Email = string.IsNullOrWhiteSpace(accountModel.Email) ? account.Email : accountModel.Email;
            account.Password = string.IsNullOrWhiteSpace(accountModel.Password) ? account.Password : HashHelper.GenerateHash(accountModel.Password);
            
            _context.Entry(account).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Account updated" });
        }

        [Authorize(Roles = "Client")]
        [HttpPut("update")]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]// Успешный ответ
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]// Ошибка 400
        public async Task<IActionResult> PutAccount(UpdateAccountModel accountModel)
        {
            int accountId = Convert.ToInt32(User.FindFirst("account_id")?.Value);
            Account? account = await _context.Accounts.Where(a => a.Id == accountId).FirstOrDefaultAsync();
            if (account == null)
            {
                return BadRequest(new { error = "Account not found!" });
            }

            if (!string.IsNullOrEmpty(accountModel.Login))
            {
                account.Login = accountModel.Login;
            }

            if (!string.IsNullOrEmpty(accountModel.Password))
            {
                account.Password = HashHelper.GenerateHash(accountModel.Password); 
            }
            account.UpdatedAt = DateTime.Now;
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
            return Ok(new {message = "Account updated successfully!"});
        }

        [Authorize(Roles = "Admin,Owner")]
        [HttpPost("add_balance")]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]// Успешный ответ
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]// Ошибка 400
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
        
        [Authorize(Roles = "Admin,Owner")]
        [HttpPost("add_balance_by_id/{id}")]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]// Успешный ответ
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]// Ошибка 400
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
        [ProducesResponseType(typeof(BalanceHistoryResponse), StatusCodes.Status200OK)]// Успешный ответ
        public async Task<ActionResult<BalanceHistoryResponse>> GetBalanceHistory()
        {
            int accountId = Convert.ToInt32(User.FindFirst("account_id")?.Value);
            List<BalanceHistory> history = await _context.BalanceHistories.Where(h => h.AccountId == accountId).ToListAsync();
            BalanceHistoryResponse response = new BalanceHistoryResponse
            {
                History = history
            };
            return Ok(response);
        }
        [Authorize(Roles = "Client")]
        [HttpPost("change_password")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)] // Успешный ответ
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)] // Ошибка 400
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
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)] // Успешный ответ
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

        [Authorize(Roles = "Admin,Owner")]
        [HttpPut("deactivate_account/{id}")]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)] // Успешный ответ
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)] // Ошибка 400
        public async Task<ActionResult> DeactivateAccount(int id)
        {
            Account account = await _context.Accounts.Where(a => a.Id == id).FirstOrDefaultAsync();
            if (account == null)
            {
                return BadRequest(new { error = "Account not found!" });
            }

            account.IsAlive = false;
            account.UpdatedAt = DateTime.Now;
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
            return Ok(new {message = "Account deactivated successfully!"});
        }

        [HttpPost("authentication")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)] // Успешный ответ
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)] // Ошибка 400
        public async Task<ActionResult> AuthClient(AuthModel authModel)
        {
            byte[] passwordHash = HashHelper.GenerateHash(authModel.password);

            Account? account = await _context.Accounts.FirstOrDefaultAsync(a => a.Login == authModel.login && a.Password.SequenceEqual(passwordHash));
            if (account == null)
            {
                return BadRequest(new {error = "Client not found!"});
            }

            if (account.IsAlive == false)
            {
                return Unauthorized(new {error = "You are not logged in. Your account deactivated."});
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
