namespace HttpServer.Services.Interfaces
{
    using Shared.Models;

    public interface IAuthorizationCache
    {
        void Add(string username, Session session);

        void Remove(string username);

        bool Contains(string username);

        Session GetSession(string username);
    }
}