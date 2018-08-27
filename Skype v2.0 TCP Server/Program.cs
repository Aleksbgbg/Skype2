namespace TcpServer
{
    using Shared.Config;

    using SimpleTCP;

    using static System.Console;

    internal static class Program
    {
        private static void Main()
        {
            SimpleTcpServer tcpServer = new SimpleTcpServer().Start(Constants.TcpPort);

            tcpServer.DataReceived += (sender, e) =>
            {
                tcpServer.Broadcast(e.Data);

                WriteLine("Message: {0}", e.MessageString);
            };

            ReadKey();
        }
    }
}