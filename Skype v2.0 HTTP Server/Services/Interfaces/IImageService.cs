namespace HttpServer.Services.Interfaces
{
    using System.Threading.Tasks;

    using HttpServer.Models;

    using Microsoft.AspNetCore.Http;

    public interface IImageService
    {
        ImageFileInfo GetImage(long id);

        Task UploadNewImageForUser(long userId, IFormFile image);
    }
}