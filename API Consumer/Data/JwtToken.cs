using Newtonsoft.Json;

namespace API_Consumer.Data
{
    public class JwtToken
    {
        [JsonProperty("accessToken")]
        public string? AccessToken { get; set; }

        [JsonProperty("expires")]
        public DateTime ExpiresAt { get; set; }
    }
}
