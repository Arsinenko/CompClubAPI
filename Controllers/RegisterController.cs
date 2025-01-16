using CompClubAPI.Models;
using CompClubAPI.Schemas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace CompClubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly CollegeTaskContext _context;

        public RegisterController(CollegeTaskContext context)
        {
            _context = context;
        }

        [HttpPost("create_client")]
        public async Task<ActionResult<Client>> PostClient(CreateClient clientModel)
        {
            string login = clientModel.login;
            byte[] password = HashHelper.GenerateHash(clientModel.password);
            Client client = new Client
            {
                Login = clientModel.login,
                Password = password,
                FirstName = clientModel.first_name,
                MiddleName = clientModel.middle_name,
                LastName = clientModel.last_name
            };
            _context.Add(client);
            await _context.SaveChangesAsync();
            return Created();
        }

    }
}
