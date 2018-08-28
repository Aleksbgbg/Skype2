namespace HttpServer.Models
{
    public class ImageFileInfo
    {
        public ImageFileInfo(string path, string contentType)
        {
            Path = path;
            ContentType = contentType;
        }

        public string Path { get; }

        public string ContentType { get; }
    }
}