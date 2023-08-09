using Newtonsoft.Json;

namespace Client.Data
{
    public class JwtToken
    {
        [JsonProperty("accessToken")]
        public string? AccessToken { get; set; }

        [JsonProperty("expires")]
        public DateTime ExpiresAt { get; set; }
    }
}
