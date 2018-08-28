namespace HttpServer.Services
{
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using HttpServer.Database;
    using HttpServer.Models;
    using HttpServer.Services.Interfaces;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.StaticFiles;
    using Microsoft.EntityFrameworkCore;

    using Shared.Models;

    public class ImageService : IImageService
    {
        private const string UnknownContentType = "application/octet-stream";

        private readonly IHostingEnvironment _environment;

        private readonly Skype2Context _databaseContext;

        private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider = new FileExtensionContentTypeProvider();

        public ImageService(IHostingEnvironment environment, Skype2Context databaseContext)
        {
            _environment = environment;
            _databaseContext = databaseContext;
        }

        public ImageFileInfo GetImage(long id)
        {
            string imagePath = GetImagePath(_databaseContext.UserImages.Single(image => image.Id == id));

            bool foundContentType = _fileExtensionContentTypeProvider.TryGetContentType(imagePath, out string contentType);

            return new ImageFileInfo(imagePath, foundContentType ? contentType : UnknownContentType);
        }

        public async Task UploadNewImageForUser(long userId, IFormFile image)
        {
            UserImage newUserImage = new UserImage
            {
                Extension = Path.GetExtension(image.FileName)
            };

            // Register new image
            await _databaseContext.UserImages.AddAsync(newUserImage);
            await _databaseContext.SaveChangesAsync();

            using (FileStream fileStream = File.Create(GetImagePath(newUserImage)))
            {
                await image.CopyToAsync(fileStream);
            }

            User targetUser = _databaseContext.Users.Include(user => user.Image).Single(user => user.Id == userId);

            UserImage oldUserImage = targetUser.Image;

            // Swap user image first, to ensure that foreign key constraint is not broken by removing the old image
            targetUser.ImageId = newUserImage.Id;
            await _databaseContext.SaveChangesAsync();

            if (_databaseContext.UserImages.First().Id != oldUserImage.Id)
            {
                File.Delete(GetImagePath(oldUserImage));
                _databaseContext.UserImages.Remove(oldUserImage);
                await _databaseContext.SaveChangesAsync();
            }
        }

        private string GetImagePath(UserImage userImage)
        {
            return Path.Combine(_environment.WebRootPath, "User Images", userImage.Filename);
        }
    }
}