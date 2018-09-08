namespace HttpServer.Services
{
    using System.Collections.Generic;

    using HttpServer.Services.Interfaces;

    using Shared.Models;

    public class AuthorizationCache : IAuthorizationCache
    {
        private readonly Dictionary<string, Session> _userSessions = new Dictionary<string, Session>();

        public void Add(string username, Session session)
        {
            _userSessions[username] = session;
        }

        public void Remove(string username)
        {
            _userSessions.Remove(username);
        }

        public bool Contains(string username)
        {
            return _userSessions.ContainsKey(username);
        }

        public Session GetSession(string username)
        {
            return _userSessions[username];
        }
    }
}