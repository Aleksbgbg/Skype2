namespace Shared.Models
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public class User
    {
        [JsonProperty("Id")]
        public long Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<Message> Messages { get; } = new HashSet<Message>();
    }
}