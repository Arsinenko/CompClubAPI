using CompClubAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CompClubAPI.Schemas;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
namespace CompClubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController] 
    public class AuthController : ControllerBase
    {
        private readonly CollegeTaskContext _context;

        public AuthController(CollegeTaskContext context)
        {
            _context = context;
        }

        [HttpPost("auth")]
        public IActionResult Login(AuthModel login)
        {
            byte[] passwordHash = HashHelper.GenerateHash(login.password);

            Client? client = _context.Clients.FirstOrDefault(c => c.Login == login.login && c.Password.SequenceEqual(passwordHash));
            if (client == null)
            {
                return NotFound(new {error = "Client not found!"});
            }

            //byte[] passwordHash = HashHelper.GenerateHash(login.password);
            //if (!passwordHash.SequenceEqual(client.Password))
            //{
            //    return BadRequest("Invalid login or password");
            //}
            string token = GenerateJWTToken(client);
            return Ok(new { token });
        }
        [NonAction]
        public string GenerateJWTToken(Client client)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("df8f3c6058ce4d93b799b4d8dc0b5ff66e1eccf69aa29505c6c84a6339a914a4")); //TODO
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "CompClubAPI",
                audience: "CompClubAPI",
                claims: new[]
                {
                    new Claim("client_id", client.Id.ToString()),
                    new Claim("client_login", client.Login.ToString()),
                },
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
