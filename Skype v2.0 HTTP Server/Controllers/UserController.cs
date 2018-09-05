namespace HttpServer.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using HttpServer.Database;
    using HttpServer.Models;
    using HttpServer.Services.Interfaces;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using Shared.Models;

    [Authorize]
    [ApiController]
    [Route("[Controller]")]
    public class UserController : ControllerBase
    {
        private readonly Skype2Context _databaseContext;

        private readonly IAuthService _authService;

        private readonly IImageService _imageService;

        public UserController(Skype2Context databaseContext, IAuthService authService, IImageService imageService)
        {
            _databaseContext = databaseContext;
            _authService = authService;
            _imageService = imageService;
        }

        [AllowAnonymous]
        [HttpGet("{userId}/get/image")]
        public ActionResult<IActionResult> GetImage(long userId)
        {
            ImageFileInfo imageFileInfo = _imageService.GetImage(_databaseContext.Users.Single(user => user.Id == userId).ImageId);

            return PhysicalFile(imageFileInfo.Path, imageFileInfo.ContentType);
        }

        [HttpGet("get/by/name/{name}")]
        public ActionResult<User> GetUserByName(string name)
        {
            return _databaseContext.Users.Single(user => user.Name == name);
        }

        [HttpGet("{userId}")]
        public ActionResult<User> GetUser(long userId)
        {
            User targetUser = _databaseContext.Users.SingleOrDefault(user => user.Id == userId);

            if (targetUser == null)
            {
                return BadRequest("No such user exists.");
            }

            return targetUser;
        }

        [HttpPost("{userId}/post/image")]
        public async Task<ActionResult> PostImage(long userId, IFormFile image)
        {
            if (!await _authService.CanAccess(User, userId))
            {
                return Unauthorized();
            }

            await _imageService.UploadNewImageForUser(userId, image);

            return Ok();
        }
    }
}