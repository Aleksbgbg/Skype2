namespace HttpServer.Controllers
{
    using HttpServer.Models;
    using HttpServer.Services.Interfaces;

    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("[Controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImagesController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpGet("get/{id}")]
        public ActionResult<IActionResult> GetImage(long id)
        {
            ImageFileInfo imageFileInfo = _imageService.GetImage(id);

            return PhysicalFile(imageFileInfo.Path, imageFileInfo.ContentType);
        }
    }
}