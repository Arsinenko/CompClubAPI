using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompClubAPI.Context;
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
        private readonly CollegeTaskV2Context _context;

        public ClientController(CollegeTaskV2Context context)
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
        [Authorize(Roles = "Admin,Owner")]
        [HttpPut("update_client/{id}")]
        public async Task<ActionResult> PutClient(int id, UpdateClientModel updateModel)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return BadRequest(new { message = "Client not found." });
            }
            client.FirstName = updateModel.FirstName;
            client.LastName = updateModel.LastName;
            client.MiddleName = string.IsNullOrEmpty(updateModel.MiddleName) ? client.MiddleName : updateModel.MiddleName;
            
            await _context.SaveChangesAsync();
            return Ok(new {message = "Client updated"});
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
                Email = clientModel.Email,
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
