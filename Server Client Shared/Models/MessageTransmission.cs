namespace Shared.Models
{
    public class MessageTransmission
    {
        public MessageTransmission(string authorization, Message message)
        {
            Authorization = authorization;
            Message = message;
        }

        public string Authorization { get; }

        public Message Message { get; }
    }
}