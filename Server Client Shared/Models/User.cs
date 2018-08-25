namespace Shared.Models
{
    using Newtonsoft.Json;

    public class User
    {
        [JsonConstructor]
        public User(long id, string name)
        {
            Id = id;
            Name = name;
        }

        [JsonProperty("Id")]
        public long Id { get; }

        [JsonProperty("Id")]
        public string Name { get; }
    }
}