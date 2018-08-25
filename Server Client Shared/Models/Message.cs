namespace Shared.Models
{
    using System;

    public class Message
    {
        public Message(User sender, DateTime createdAt, string content)
        {
            Sender = sender;
            CreatedAt = createdAt;
            Content = content;
        }

        public User Sender { get; }

        public DateTime CreatedAt { get; }

        public string Content { get; }
    }
}