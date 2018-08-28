namespace HttpServer.Services.Interfaces
{
    public interface IAuthService
    {
        bool Authorize(string username, string password, out string token);

        bool CheckAuthorized(string token);

        void DeAuthorize(string token);
    }
}