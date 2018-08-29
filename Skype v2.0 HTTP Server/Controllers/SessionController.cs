namespace HttpServer.Controllers
{
    using System;

    using HttpServer.Services.Interfaces;

    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("[Controller]")]
    public class SessionController : ControllerBase
    {
        private readonly IAuthService _authService;

        public SessionController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("login")]
        public IActionResult Login([FromHeader] string authorization)
        {
            if (!authorization.StartsWith("Basic", StringComparison.InvariantCultureIgnoreCase))
            {
                return BadRequest();
            }

            string[] authParameters = authorization.Split(' ', 3);

            if (_authService.Authorize(authParameters[1], authParameters[2], out string token))
            {
                return Ok(token);
            }

            return Unauthorized();
        }

        [HttpGet("logout")]
        public IActionResult Logout([FromHeader] string authorization)
        {
            _authService.DeAuthorize(authorization);

            return Ok();
        }
    }
}