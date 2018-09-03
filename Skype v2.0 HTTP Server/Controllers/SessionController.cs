namespace HttpServer.Controllers
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

    using HttpServer.Database;
    using HttpServer.Services.Interfaces;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;

    using Shared.Config;
    using Shared.Models;

    [AllowAnonymous]
    [ApiController]
    [Route("[Controller]")]
    public class SessionController : ControllerBase
    {
        private readonly IAuthService _authService;

        private readonly Skype2Context _databaseContext;

        private readonly IConfiguration _configuration;

        public SessionController(IAuthService authService, Skype2Context databaseContext, IConfiguration configuration)
        {
            _authService = authService;
            _databaseContext = databaseContext;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult RequestToken([FromBody] UserCredentials userCredentials)
        {
            User targetUser = _databaseContext.Users.Single(user => user.Name == userCredentials.Username);

            if (targetUser.Password == userCredentials.Password)
            {
                Claim[] claims = { new Claim(ClaimTypes.Name, userCredentials.Username) };

                SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecurityKey"]));
                SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                string selfAddress = $"{Constants.ServerIp}:{Constants.TcpPort}";

                JwtSecurityToken token = new JwtSecurityToken(selfAddress, selfAddress, claims, expires: DateTime.Now.AddMinutes(30), signingCredentials: credentials);

                return Ok(new JwtSecurityTokenHandler().WriteToken(token));
            }

            return BadRequest("Invalid credentials.");
        }

        [HttpPost("register")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> Register([FromForm] string username, [FromForm] string password)
        {
            await _databaseContext.Users.AddAsync(new User
            {
                CreatedAt = DateTime.Now,
                Name = username,
                Password = Encoding.UTF8.GetString(Convert.FromBase64String(password))
            });
            await _databaseContext.SaveChangesAsync();

            if (_authService.Authorize(username, password, out string token))
            {
                return Ok(token);
            }

            return Unauthorized();
        }

        [Authorize]
        [HttpGet("logout")]
        public IActionResult Logout([FromHeader] string authorization)
        {
            _authService.DeAuthorize(authorization);

            return Ok();
        }
    }
}