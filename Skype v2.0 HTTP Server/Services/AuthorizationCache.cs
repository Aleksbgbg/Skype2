namespace HttpServer.Services
{
    using System.Collections.Generic;

    using HttpServer.Services.Interfaces;

    public class AuthorizationCache : IAuthorizationCache
    {
        private readonly List<string> _cache = new List<string>();

        public void Add(string token)
        {
            _cache.Add(token);
        }

        public void Remove(string token)
        {
            _cache.Remove(token);
        }

        public bool Contains(string token)
        {
            return _cache.Contains(token);
        }
    }
}