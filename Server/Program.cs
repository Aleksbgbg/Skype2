namespace Skype2.Server
{
    using System;

    using Skype2.Server.Endpoints;

    internal static class Program
    {
        private static void Main()
        {
            using (RestEndpoint restEndpoint = new RestEndpoint())
            {
                Console.ReadKey();
            }
        }
    }
}