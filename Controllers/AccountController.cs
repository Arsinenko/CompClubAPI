using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CompClubAPI.Models;
using CompClubAPI.Schemas;
using Microsoft.IdentityModel.Tokens;

namespace CompClubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly CollegeTaskContext _context;

        public AccountController(CollegeTaskContext context)
        {
            _context = context;
        }

        // GET: api/Account
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {
            return await _context.Accounts.ToListAsync();
        }

        // GET: api/Account/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(int id)
        {
            var account = await _context.Accounts.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }

        // PUT: api/Account/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
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

        // POST: api/Account
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("create_account")]
        public async Task<ActionResult<Account>> CreateAccount(CreateAccountModel accountModel)
        {
            Account account = new Account
            {
                IdClient = accountModel.ClientId,
                Balance = accountModel.Balance,
                Login = accountModel.Login,
                Password = HashHelper.GenerateHash(accountModel.Password)
            };
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAccount", new { id = account.Id }, account);
        }
        [HttpPost("authentication")]
        public IActionResult AuthClient(AuthModel authModel)
        {
            byte[] passwordHash = HashHelper.GenerateHash(authModel.password);

            Account? account = _context.Accounts.FirstOrDefault(a => a.Login == authModel.login && a.Password.SequenceEqual(passwordHash));
            if (account == null)
            {
                return NotFound(new {error = "Client not found!"});
            }
            string token = GenerateJwtToken(account);
            return Ok(new { token });
        }
        [NonAction]
        public string GenerateJwtToken(Account account)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("df8f3c6058ce4d93b799b4d8dc0b5ff66e1eccf69aa29505c6c84a6339a914a4")); //TODO
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: "CompClubAPI",
                audience: "CompClubAPI",
                claims: new[]
                {
                    new Claim("client_id", account.Id.ToString()),
                    new Claim("client_login", account.Login.ToString()),
                    new Claim(ClaimTypes.Role, "Client")
                },

                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        // // DELETE: api/Account/5
        // [HttpPost("deactivate")]
        // public async Task<IActionResult> DeactivateAccount()
        // {
        //     int clientId = int.Parse(User.Claims.First(c => c.Type == "client_id").Value);
        //     Account? account = await _context.Accounts.Where(a => a.IdClient == clientId).FirstOrDefaultAsync();
        //     if (account == null)
        //     {
        //         return NotFound(new {message = "Account not found"});
        //     }
        //     Client? client = await _context.Clients.Where(c => c.Id == clientId).FirstOrDefaultAsync();
        //     if (client == null)
        //     {
        //         return NotFound(new { message = "Client not found!" });
        //     }
        //     account.UpdatedAt = DateTime.Now;
        //     account.IsActive = false;
        //     _context.Accounts.Update(account);
        //     await _context.SaveChangesAsync();
        //     return Ok(new {message = $"Deactivated account {account.Id}, Deactivated client with id {clientId}"});
        // }

        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.Id == id);
        }
    }
}
