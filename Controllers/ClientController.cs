using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompClubAPI.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CompClubAPI.Models;
using CompClubAPI.ResponseSchema;
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
        /// <summary>
        /// Возвращает информацию о клиентах. Доступно только для администраторов и владельцев.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Owner")]
        [HttpGet("get_clients")]
        [ProducesResponseType(typeof(ClientsResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            List<Client> clients = await _context.Clients.ToListAsync();
            return Ok(new {clients});
        }

        // GET: api/Client/5
        /// <summary>
        /// Возвращает информацию о клиенте по его ID. Доступно только для администраторов и владельцев.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Owner")]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ClientResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Client>> GetClient(int id)
        {
            Client? client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return BadRequest(new {error = "Client not found!"});
            }
            return Ok(new {client});
        }

        /// <summary>
        /// Обновляет данные клиента по его ID. Доступно только для администраторов и владельцев.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateModel"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Owner")]
        [HttpPut("update_client/{id}")]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
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

        /// <summary>
        /// Создает нового клиента.
        /// </summary>
        /// <param name="clientModel"></param>
        /// <returns></returns>
        [HttpPost("create_client")]
        [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Client>> PostClient(CreateClient clientModel)
        {
            if (clientModel.Login.Length < 3 || clientModel.Password.Length < 8)
            {
                return BadRequest(new {error = "Login or password is too short! Password must be at least 8 characters long! Login must be at least 3 characters long!"}); //{})
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
