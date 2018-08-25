namespace Skype2.Server.Endpoints
{
    using System;

    internal class RestEndpoint : IDisposable
    {
        ~RestEndpoint()
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