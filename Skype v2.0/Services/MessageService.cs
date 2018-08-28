namespace Skype2.Services
{
    using System;

    using Newtonsoft.Json;

    using Shared.Config;

    using SimpleTCP;

    using Skype2.EventArgs;
    using Skype2.Services.Interfaces;

    using Message = Shared.Models.Message;

    internal class MessageService : IMessageService
    {
        private readonly ISessionService _sessionService;

        private readonly SimpleTcpClient _tcpClient = new SimpleTcpClient().Connect(Constants.ServerIp.ToString(), Constants.TcpPort);

        public MessageService(ISessionService sessionService)
        {
            _sessionService = sessionService;

            _tcpClient.DataReceived += (sender, e) => MessageReceived?.Invoke(this, new MessageReceivedEventArgs(JsonConvert.DeserializeObject<Message>(e.MessageString)));
        }

        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public void SendMessage(string content)
        {
            _tcpClient.Write(JsonConvert.SerializeObject(new Message
            {
                Content = content,
                CreatedAt = DateTime.Now,
                SenderId = _sessionService.LoggedInUser.Id
            }));
        }
    }
}