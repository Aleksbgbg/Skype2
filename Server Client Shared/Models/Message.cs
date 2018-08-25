namespace Shared.Models
{
    using System;

    using Newtonsoft.Json;

    public class Message
    {
        [JsonConstructor]
        public Message(long id, User sender, DateTime createdAt, string content) : this(sender, createdAt, content)
        {
            Id = id;
        }

        public Message(User sender, DateTime createdAt, string content)
        {
            Sender = sender;
            CreatedAt = createdAt;
            Content = content;
        }

        [JsonProperty("Id")]
        public long? Id { get; }

        [JsonProperty("User")]
        public User Sender { get; }

        [JsonProperty("CreatedAt")]
        public DateTime CreatedAt { get; }

        [JsonProperty("Content")]
        public string Content { get; }
    }
}