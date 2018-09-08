namespace Shared.Models
{
    using System;

    using Newtonsoft.Json;

    public class Session
    {
        [JsonProperty("RefreshToken")]
        public string RefreshToken { get; set; }

        [JsonProperty("ExpiresAt")]
        public DateTime ExpiresAt { get; set; }
    }
}