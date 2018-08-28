namespace HttpServer.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using HttpServer.Database;
    using HttpServer.Models;
    using HttpServer.Services.Interfaces;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using Shared.Models;

    [ApiController]
    [Route("[Controller]")]
    public class UserController : ControllerBase
    {
        private readonly IImageService _imageService;

        private readonly Skype2Context _databaseContext;

        public UserController(IImageService imageService, Skype2Context databaseContext)
        {
            _imageService = imageService;
            _databaseContext = databaseContext;
        }

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

        [HttpPost("{userId}/post/image")]
        public async Task PostImage(long userId, IFormFile image)
        {
            await _imageService.UploadNewImageForUser(userId, image);
        }
    }
}