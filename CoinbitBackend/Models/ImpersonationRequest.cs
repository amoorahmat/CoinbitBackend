using System.Text.Json.Serialization;

namespace CoinbitBackend.Models
{
    public class ImpersonationRequest
    {
        [JsonPropertyName("username")]
        public string UserName { get; set; }
    }
}
