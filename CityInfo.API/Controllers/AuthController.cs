using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CityInfo.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public class AuthRequestBody
        {
            public string? Username { get; set; }
            public string? Password { get; set; }
        }

        public class CityInfoUser
        {
            public int UserId { get; set; }
            public string Username { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string City { get; set; }

            public CityInfoUser(int _userid, string _username, string _firstname, string _lastname, string _city)
            {
                UserId = _userid;
                Username = _username;
                FirstName = _firstname;
                LastName = _lastname;
                City = _city;
            }
        }

        public IConfiguration Configuration { get; }
        public AuthController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        [HttpPost("login")]
        public ActionResult<string> Login(AuthRequestBody logininput)
        {
            var user = ValidateUser(logininput.Username, logininput.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            var securityKey = new SymmetricSecurityKey(Convert.FromBase64String(Configuration["Authentication:SecretForKey"]));

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenClaims = new List<Claim>();
            tokenClaims.Add(new Claim("sub", user.UserId.ToString()));
            tokenClaims.Add(new Claim("given_name", user.FirstName));
            tokenClaims.Add(new Claim("family_name", user.LastName));
            tokenClaims.Add(new Claim("city", user.City));

            var token = new JwtSecurityToken(
                Configuration["Authentication:Issuer"],
                Configuration["Authentication:Audience"],
                tokenClaims, DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(10),
                signingCredentials);

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(jwtToken);
        }

        private CityInfoUser ValidateUser(string? username, string? password)
        {
            return new CityInfoUser(1, username ?? "", "Omer", "Mohideem", "omer.mohideen@mycompany.com");
        }
    }
}