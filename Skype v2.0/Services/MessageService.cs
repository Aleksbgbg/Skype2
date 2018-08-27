namespace Skype2.Services
{
    using System;

    using Shared.Config;

    using SimpleTCP;

    using Skype2.EventArgs;
    using Skype2.Services.Interfaces;

    internal class MessageService : IMessageService
    {
        private readonly SimpleTcpClient _tcpClient = new SimpleTcpClient().Connect(Constants.ServerIp.ToString(), Constants.TcpPort);

        public MessageService()
        {
            _tcpClient.DataReceived += (sender, e) => MessageReceived?.Invoke(this, new MessageReceivedEventArgs(e.Data, e.MessageString));
        }

        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public void SendMessage(byte[] content)
        {
            _tcpClient.Write(content);
        }

        public void SendMessage(string content)
        {
            _tcpClient.Write(content);
        }
    }
}