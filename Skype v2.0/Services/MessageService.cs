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
        private readonly IUserService _userService;

        private readonly SimpleTcpClient _tcpClient = new SimpleTcpClient().Connect(Constants.ServerIp.ToString(), Constants.TcpPort);

        public MessageService(IUserService userService)
        {
            _userService = userService;

            _tcpClient.DataReceived += (sender, e) => MessageReceived?.Invoke(this, new MessageReceivedEventArgs(JsonConvert.DeserializeObject<Message>(e.MessageString)));
        }

        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public void SendMessage(string content)
        {
            _tcpClient.Write(JsonConvert.SerializeObject(new Message
            {
                Content = content,
                CreatedAt = DateTime.Now,
                Sender = _userService.LoggedInUser,
                SenderId = _userService.LoggedInUser.Id
            }));
        }
    }
}