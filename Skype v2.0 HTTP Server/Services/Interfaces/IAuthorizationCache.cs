namespace HttpServer.Services.Interfaces
{
    public interface IAuthorizationCache
    {
        void Add(string token);

        void Remove(string token);

        bool Contains(string token);
    }
}