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

namespace CompClubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly CollegeTaskContext _context;

        public ClientController(CollegeTaskContext context)
        {
            _context = context;
        }

        // GET: api/Client
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            return await _context.Clients.ToListAsync();
        }

        // GET: api/Client/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            return client;
        }

        // PUT: api/Client/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("update_client")]
        public async Task<IActionResult> PutClient(int id, CreateClient clientModel)
        {
            int accountId = Convert.ToInt32(User.FindFirst("account_id")?.Value);
            Client? client = await _context.Accounts
                .Where(a => a.Id == accountId)
                .Select(a => a.IdClientNavigation)
                .FirstOrDefaultAsync();
            if (client == null)
            {
                return NotFound( new {message = "Client not found!"});
            }
            
            client.FirstName = clientModel.FirstName;
            client.MiddleName = clientModel.MiddleName;
            client.LastName = clientModel.LastName;
            client.UpdatedAt = DateTime.Now;
            
            _context.Clients.Update(client);
            await _context.SaveChangesAsync();
            return Ok(new {message = $"Client with id {client.Id} updated successfully!"});
        }

        // POST: api/Client
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("create_client")]
        public async Task<ActionResult<Client>> PostClient(CreateClient clientModel)
        {
            if (clientModel.Login.Length < 3 || clientModel.Password.Length < 8)
            {
                return BadRequest(new {message = "Login or password is too short! Password must be at least 8 characters long! Login must be at least 3 characters long!"}); //{})
            }
            Client client = new Client
            {
                FirstName = clientModel.FirstName,
                MiddleName = clientModel.MiddleName,
                LastName = clientModel.LastName
            };
            
            _context.Clients.Add(client);
            
            await _context.SaveChangesAsync();
            Account account = new Account
            {
                IdClient = client.Id,
                Balance = 0,
                Login = clientModel.Login,
                Password = HashHelper.GenerateHash(clientModel.Password)
            };
            _context.Add(account);
            await _context.SaveChangesAsync();
            return Created("", new {message = "Client created successfully!", client_id = client.Id, accountId = account.Id });
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}
