namespace Shared.Models
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public class UserImage : Snowflake
    {
        [JsonProperty("Extension")]
        public string Extension { get; set; }

        [JsonIgnore]
        public string Filename => string.Concat(Id, Extension);

        [JsonIgnore]
        public ICollection<User> AttachedUsers { get; } = new HashSet<User>();
    }
}