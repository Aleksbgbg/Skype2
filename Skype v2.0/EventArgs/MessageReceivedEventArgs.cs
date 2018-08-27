namespace Skype2.EventArgs
{
    using System;

    internal class MessageReceivedEventArgs : EventArgs
    {
        public MessageReceivedEventArgs(byte[] data, string messageString)
        {
            Data = data;
            MessageString = messageString;
        }

        public byte[] Data { get; }

        public string MessageString { get; }
    }
}