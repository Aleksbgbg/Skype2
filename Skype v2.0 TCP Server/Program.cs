namespace TcpServer
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using Shared.Config;

    using SimpleTCP;

    using static System.Console;

    using Message = Shared.Models.Message;

    internal static class Program
    {
        private static void Main()
        {
            HttpClient httpClient = new HttpClient();

            SimpleTcpServer tcpServer = new SimpleTcpServer().Start(Constants.TcpPort);

            tcpServer.DataReceived += async (sender, e) =>
            {
                tcpServer.Broadcast(e.Data);

                ByteArrayContent byteArrayContent = new ByteArrayContent(Encoding.UTF8.GetBytes(e.MessageString));
                byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                await httpClient.PostAsync($"http://{Constants.ServerIp}:{Constants.HttpPort}/messages/post", byteArrayContent);

                WriteLine("Message: {0}", JsonConvert.DeserializeObject<Message>(e.MessageString).Content);
            };

            ReadKey();
        }
    }
}