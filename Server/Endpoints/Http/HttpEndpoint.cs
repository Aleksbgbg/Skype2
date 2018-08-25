namespace Skype2.Server.Endpoints.Http
{
    using System;

    using Microsoft.Owin.Hosting;

    using Shared.Config;

    internal class HttpEndpoint : IDisposable
    {
        private readonly IDisposable _webStart = WebApp.Start<Startup>($"http://{Constants.ServerIp}:{Constants.HttpPort}");

        ~HttpEndpoint()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _webStart.Dispose();
            }
        }
    }
}