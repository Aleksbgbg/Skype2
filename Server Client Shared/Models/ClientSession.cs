namespace Shared.Models
{
    using System;

    using Newtonsoft.Json;

    public class ClientSession
    {
        [JsonProperty("Token")]
        public string Token { get; set; }

        [JsonProperty("ExpiresAt")]
        public DateTime ExpiresAt { get; set; }

        [JsonProperty("Session")]
        public Session Session { get; set; }
    }
}