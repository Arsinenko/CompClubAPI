using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CompClubAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;

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
        [Authorize(Roles = "Employee")]
        [HttpGet("get_clients")]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            return await _context.Clients.ToListAsync();
        }

        // GET: api/Client/5
        [Authorize]
        [HttpGet("get_info")]
        public async Task<ActionResult> GetClient()
        {
            int clientId = Convert.ToInt32(User.FindFirst("client_id")?.Value);
            var client = await _context.Clients.FindAsync(clientId);

            if (client == null)
            {
                return NotFound(new {error = "Client not found!"});
            }

            return Ok(new { login = client.Login, firstname = client.FirstName, middlename = client.MiddleName, lastname = client.LastName });
        }
            
        // PUT: api/Client/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClient(int id, Client client)
        {
            if (id != client.Id)
            {
                return BadRequest();
            }

            _context.Entry(client).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
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

        // POST: api/Client
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Client>> PostClient(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            Account account = new Account { IdClient = client.Id, Balance = 100 };
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClient", new { id = client.Id }, client);
        }

        // DELETE: api/Client/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}
