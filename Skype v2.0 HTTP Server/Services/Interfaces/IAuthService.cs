namespace HttpServer.Services.Interfaces
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    public interface IAuthService
    {
        bool Authorize(string username, string password, out string token);

        bool CheckAuthorized(string token);

        void DeAuthorize(string token);

        Task<bool> CanAccess(ClaimsPrincipal sessionUser, long userId);
    }
}