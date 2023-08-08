using System.Text.Json.Serialization;

namespace API_Consumer.Data
{
    public class ErrorResponse
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("statuscode")]
        public int StatusCode { get; set; }

        [JsonPropertyName("errors")]
        public Dictionary<string, List<string>> Errors { get; set; } // = new Dictionary<string, List<string>>();
    }
}
