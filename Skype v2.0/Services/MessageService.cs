namespace Skype2.Services
{
    using System;

    using Newtonsoft.Json;

    using Shared.Config;
    using Shared.Models;

    using SimpleTCP;

    using Skype2.EventArgs;
    using Skype2.Services.Interfaces;

    using Message = Shared.Models.Message;

    internal class MessageService : IMessageService
    {
        private readonly IRestService _restService;

        private readonly SimpleTcpClient _tcpClient = new SimpleTcpClient().Connect(Constants.ServerIp.ToString(), Constants.TcpPort);

        public MessageService(IRestService restService)
        {
            _restService = restService;

            _tcpClient.DataReceived += (sender, e) => MessageReceived?.Invoke(this, new MessageReceivedEventArgs(JsonConvert.DeserializeObject<Message>(e.MessageString)));
        }

        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public void SendMessage(string content)
        {
            _tcpClient.Write(JsonConvert.SerializeObject(new MessageTransmission($"Token {_restService.AuthToken}",
                                                                                 new Message
                                                                                 {
                                                                                     Content = content,
                                                                                     CreatedAt = DateTime.Now,
                                                                                     SenderId = _restService.LoggedInUser.Id
                                                                                 })));
        }
    }
}