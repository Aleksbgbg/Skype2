namespace TcpServer
{
    using Newtonsoft.Json;

    using Shared.Config;

    using SimpleTCP;

    using static System.Console;

    using Message = Shared.Models.Message;

    internal static class Program
    {
        private static void Main()
        {
            SimpleTcpServer tcpServer = new SimpleTcpServer().Start(Constants.TcpPort);

            tcpServer.DataReceived += (sender, e) =>
            {
                tcpServer.Broadcast(e.Data);

                WriteLine("Message: {0}", JsonConvert.DeserializeObject<Message>(e.MessageString).Content);
            };

            ReadKey();
        }
    }
}