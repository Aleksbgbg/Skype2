namespace Shared.Models
{
    using System;

    using Newtonsoft.Json;

    public class Message : Snowflake
    {
        [JsonProperty("Content")]
        public string Content { get; set; }

        [JsonProperty("CreatedAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("SenderId")]
        public long SenderId { get; set; }

        [JsonProperty("Sender")]
        public User Sender { get; set; }
    }
}