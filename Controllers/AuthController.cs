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
            Client? client = _context.Clients.FirstOrDefault(c => c.Login == login.login);
            if (client == null)
            {
                return NotFound("Client not found!");
            }

            byte[] passwordHash = HashHelper.GenerateHash(login.password);
            if (!passwordHash.SequenceEqual(client.Password))
            {
                return BadRequest("Invalid password");
            }
            string token = GenerateJWTToken(client);
            return Ok(new { token });
        }
        [NonAction]
        public string GenerateJWTToken(Client client)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("82E88253ABB382A54AB9DB5FC55EA")); //TODO
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
