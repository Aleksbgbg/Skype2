namespace Shared.Models
{
    using Newtonsoft.Json;

    public class User
    {
        [JsonConstructor]
        public User(ulong id, string name)
        {
            Id = id;
            Name = name;
        }

        [JsonProperty("Id")]
        public ulong Id { get; }

        [JsonProperty("Id")]
        public string Name { get; }
    }
}