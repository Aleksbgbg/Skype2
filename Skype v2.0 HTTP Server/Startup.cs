namespace HttpServer
{
    using System.IO;

    using HttpServer.Database;
    using HttpServer.Extensions;
    using HttpServer.Services;
    using HttpServer.Services.Interfaces;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddJwtAuthentication(_configuration);

            services.AddDbContext<Skype2Context>(options => options.UseNpgsql(File.ReadAllText("Connection String.txt")));

            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddSingleton<IAuthorizationCache, AuthorizationCache>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}