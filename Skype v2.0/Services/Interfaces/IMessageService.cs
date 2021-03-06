﻿namespace Skype2.Services.Interfaces
{
    using System;

    using Skype2.EventArgs;

    internal interface IMessageService
    {
        event EventHandler<MessageReceivedEventArgs> MessageReceived;

        void SendMessage(string content);
    }
}