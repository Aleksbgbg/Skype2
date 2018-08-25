namespace Skype2.Server.Endpoints.Http
{
    using System;

    internal class HttpEndpoint : IDisposable
    {
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

            }
        }
    }
}