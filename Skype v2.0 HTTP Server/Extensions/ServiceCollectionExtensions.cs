namespace HttpServer.Extensions
{
    using System.Text;

    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;

    using Shared.Config;

    internal static class ServiceCollectionExtensions
    {
        internal static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            string selfAddress = $"{Constants.ServerIp}:{Constants.TcpPort}";

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = selfAddress,
                        ValidAudience = selfAddress,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["SecurityKey"]))
                    });
        }
    }
}