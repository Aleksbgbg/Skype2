namespace Skype2.Server
{
    using System;

    using Skype2.Server.Endpoints.Http;

    internal static class Program
    {
        private static void Main()
        {
            using (HttpEndpoint httpEndpoint = new HttpEndpoint())
            {
                Console.ReadKey();
            }
        }
    }
}