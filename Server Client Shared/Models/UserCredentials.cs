namespace Shared.Models
{
    using Newtonsoft.Json;

    public class UserCredentials
    {
        [JsonConstructor]
        public UserCredentials(string username, string password)
        {
            Username = username;
            Password = password;
        }

        [JsonProperty("Username")]
        public string Username { get; }

        [JsonProperty("Password")]
        public string Password { get; }
    }
}