namespace HttpServer.Services
{
    using System;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    using HttpServer.Database;
    using HttpServer.Services.Interfaces;

    public class AuthService : IAuthService
    {
        private const int TokenLengthBytes = 32;

        private readonly RNGCryptoServiceProvider _cryptoServiceProvider = new RNGCryptoServiceProvider();

        private readonly Skype2Context _databaseContext;

        private readonly IAuthorizationCache _authorizationCache;

        public AuthService(Skype2Context databaseContext, IAuthorizationCache authorizationCache)
        {
            _databaseContext = databaseContext;
            _authorizationCache = authorizationCache;
        }

        public bool Authorize(string username, string password, out string token)
        {
            bool passwordIsRight = _databaseContext.Users.Single(user => user.Name == username).Password == Encoding.UTF8.GetString(Convert.FromBase64String(password));

            if (passwordIsRight)
            {
                byte[] tokenBytes = new byte[TokenLengthBytes];

                _cryptoServiceProvider.GetBytes(tokenBytes);

                token = BitConverter.ToString(tokenBytes).Replace("-", "");

                _authorizationCache.Add(token);
            }
            else
            {
                token = null;
            }

            return passwordIsRight;
        }

        public bool CheckAuthorized(string token)
        {
            return _authorizationCache.Contains(ExtractToken(token));
        }

        public void DeAuthorize(string token)
        {
            _authorizationCache.Remove(ExtractToken(token));
        }

        private string ExtractToken(string tokenAuthorization)
        {
            if (!tokenAuthorization.StartsWith("Token", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new InvalidOperationException("Cannot extract token from non-token authorization parameter.");
            }

            return tokenAuthorization.Split(' ', 2)[1];
        }
    }
}