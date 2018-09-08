namespace HttpServer.Services
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

    using HttpServer.Authorization;
    using HttpServer.Database;
    using HttpServer.Services.Interfaces;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;

    using Shared.Config;
    using Shared.Models;

    public class AuthService : IAuthService
    {
        private readonly Skype2Context _databaseContext;

        private readonly IAuthorizationCache _authorizationCache;

        private readonly IConfiguration _configuration;

        private readonly IHashService _hashService;

        private readonly IAuthorizationService _authorizationService;

        public AuthService(Skype2Context databaseContext, IAuthorizationCache authorizationCache, IConfiguration configuration, IHashService hashService, IAuthorizationService authorizationService)
        {
            _databaseContext = databaseContext;
            _authorizationCache = authorizationCache;
            _configuration = configuration;
            _hashService = hashService;
            _authorizationService = authorizationService;
        }

        public bool Authorize(string username, string password, out string token)
        {
            User targetUser = _databaseContext.Users.Single(user => user.Name == username);

            string hashedPassword = _hashService.HashPassword(password, Convert.FromBase64String(targetUser.Salt));

            if (hashedPassword == targetUser.Password)
            {
                Claim[] claims = { new Claim(ClaimTypes.Name, username) };

                SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecurityKey"]));
                SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                JwtSecurityToken jwtToken = new JwtSecurityToken(Constants.HttpServerAddress, Constants.HttpServerAddress, claims, expires: DateTime.Now.AddMinutes(30), signingCredentials: credentials);

                token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
                return true;
            }

            token = null;
            return false;
        }

        public bool CheckAuthorized(string token)
        {
            return _authorizationCache.Contains(ExtractToken(token));
        }

        public void DeAuthorize(string token)
        {
            _authorizationCache.Remove(ExtractToken(token));
        }

        public async Task<bool> CanAccess(ClaimsPrincipal sessionUser, long userId)
        {
            User targetUser = _databaseContext.Users.Single(user => user.Id == userId);

            AuthorizationResult result = await _authorizationService.AuthorizeAsync(sessionUser, targetUser, Policy.ActiveUser);

            return result.Succeeded;
        }

        private string ExtractToken(string tokenAuthorization)
        {
            if (!tokenAuthorization.StartsWith("Bearer", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new InvalidOperationException("Cannot extract token from non-token authorization parameter.");
            }

            return tokenAuthorization.Split(' ', 2)[1];
        }
    }
}