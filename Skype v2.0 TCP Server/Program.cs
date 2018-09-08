namespace TcpServer
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;

    using Newtonsoft.Json;

    using Shared.Config;
    using Shared.Models;

    using SimpleTCP;

    using static System.Console;

    internal static class Program
    {
        private static void Main()
        {
            HttpClient httpClient = new HttpClient();

            SimpleTcpServer tcpServer = new SimpleTcpServer().Start(Constants.TcpPort);

            tcpServer.DataReceived += async (sender, e) =>
            {
                MessageTransmission messageTransmission = JsonConvert.DeserializeObject<MessageTransmission>(e.MessageString);

                string messageString = JsonConvert.SerializeObject(messageTransmission.Message);

                tcpServer.Broadcast(messageString);

                ByteArrayContent byteArrayContent = new ByteArrayContent(Encoding.UTF8.GetBytes(messageString));
                byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, $"{Constants.HttpServerAddress}/messages/post")
                {
                        Content = byteArrayContent,
                        Headers =
                        {
                                { "Authorization", messageTransmission.Authorization }
                        }
                };

                await httpClient.SendAsync(message);

                WriteLine("Message: {0}", messageTransmission.Message.Content);
            };

            ReadKey();
        }
    }
}