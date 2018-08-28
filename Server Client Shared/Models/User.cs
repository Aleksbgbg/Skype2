namespace Shared.Models
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public class User : Snowflake
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("ImageId")]
        public long ImageId { get; set; }

        [JsonProperty("CreatedAt")]
        public DateTime CreatedAt { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        [JsonIgnore]
        public string Salt { get; set; }

        [JsonIgnore]
        public UserImage Image { get; set; }

        [JsonIgnore]
        public ICollection<Message> Messages { get; } = new HashSet<Message>();
    }
}