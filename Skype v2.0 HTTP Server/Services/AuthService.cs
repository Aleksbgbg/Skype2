namespace HttpServer.Services
{
    using System;
    using System.Collections.Generic;
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

        public bool Authorize(string username, string password, out ClientSession token)
        {
            User targetUser = _databaseContext.Users.Single(user => user.Name == username);

            string hashedPassword = _hashService.HashPassword(password, Convert.FromBase64String(targetUser.Salt));

            if (hashedPassword == targetUser.Password)
            {
                Claim[] claims = { new Claim(ClaimTypes.Name, username) };

                token = GenerateSession(username, claims);
                return true;
            }

            token = null;
            return false;
        }

        public ClientSession RefreshToken(ClientSession oldSession)
        {
            ClaimsPrincipal oldTokenClaimsPrincipal;

            {
                TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecurityKey"])),
                    ValidateLifetime = false,
                    ValidAudience = Constants.HttpServerAddress,
                    ValidIssuer = Constants.HttpServerAddress
                };

                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

                oldTokenClaimsPrincipal = tokenHandler.ValidateToken(oldSession.Token, tokenValidationParameters, out SecurityToken securityToken);

                if (securityToken == null)
                {
                    return null;
                }
            }

            string username = oldTokenClaimsPrincipal.Identity.Name;

            if (!_authorizationCache.Contains(username))
            {
                return null;
            }

            Session userSession = _authorizationCache.GetSession(username);

            if (DateTime.Now >= userSession.ExpiresAt || userSession.RefreshToken != oldSession.Session.RefreshToken)
            {
                return null;
            }

            return GenerateSession(username, oldTokenClaimsPrincipal.Claims);
        }

        public void Invalidate(string username)
        {
            _authorizationCache.Remove(username);
        }

        public async Task<bool> CanAccess(ClaimsPrincipal sessionUser, long userId)
        {
            User targetUser = _databaseContext.Users.Single(user => user.Id == userId);

            AuthorizationResult result = await _authorizationService.AuthorizeAsync(sessionUser, targetUser, Policy.ActiveUser);

            return result.Succeeded;
        }

        private ClientSession GenerateSession(string username, IEnumerable<Claim> claims)
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecurityKey"]));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwtToken = new JwtSecurityToken(Constants.HttpServerAddress, Constants.HttpServerAddress, claims, expires: DateTime.Now.AddMinutes(0.5), signingCredentials: credentials);

            ClientSession clientSesh = new ClientSession
            {
                ExpiresAt = DateTime.Now.AddMinutes(5.0),
                Session = new Session
                {
                    ExpiresAt = DateTime.Now.AddDays(1),
                    RefreshToken = Convert.ToBase64String(_hashService.GenerateSalt(256))
                },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtToken)
            };

            _authorizationCache.Add(username, clientSesh.Session);

            return clientSesh;
        }
    }
}