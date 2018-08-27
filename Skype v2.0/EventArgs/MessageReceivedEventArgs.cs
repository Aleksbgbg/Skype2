namespace Skype2.EventArgs
{
    using System;

    using Shared.Models;

    internal class MessageReceivedEventArgs : EventArgs
    {
        public MessageReceivedEventArgs(Message message)
        {
            Message = message;
        }

        public Message Message { get; }
    }
}