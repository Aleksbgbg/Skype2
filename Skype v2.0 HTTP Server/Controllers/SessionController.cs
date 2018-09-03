namespace HttpServer.Controllers
{
    using System;
    using System.Text;
    using System.Threading.Tasks;

    using HttpServer.Database;
    using HttpServer.Services.Interfaces;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Shared.Models;

    [AllowAnonymous]
    [ApiController]
    [Route("[Controller]")]
    public class SessionController : ControllerBase
    {
        private readonly IAuthService _authService;

        private readonly Skype2Context _databaseContext;

        public SessionController(IAuthService authService, Skype2Context databaseContext)
        {
            _authService = authService;
            _databaseContext = databaseContext;
        }

        [HttpPost("login")]
        public IActionResult RequestToken([FromBody] UserCredentials userCredentials)
        {
            if (_authService.Authorize(userCredentials.Username, userCredentials.Password, out string token))
            {
                return Ok(token);
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