namespace HttpServer
{
    using System.IO;

    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;

    internal static class Program
    {
        private static void Main()
        {
            WebHost.CreateDefaultBuilder()
                   .UseKestrel()
                   .UseUrls("http://0.0.0.0:3002")
                   .UseContentRoot(Directory.GetCurrentDirectory())
                   .UseIISIntegration()
                   .UseStartup<Startup>()
                   .Build()
                   .Run();
        }
    }
}