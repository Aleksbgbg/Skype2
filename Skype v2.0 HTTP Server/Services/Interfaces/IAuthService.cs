namespace HttpServer.Services.Interfaces
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Shared.Models;

    public interface IAuthService
    {
        bool Authorize(string username, string password, out ClientSession clientSession);

        void Invalidate(string username);

        Task<bool> CanAccess(ClaimsPrincipal sessionUser, long userId);

        ClientSession RefreshToken(ClientSession oldSession);
    }
}