
namespace CoinbitBackend.Models
{
    public class CoreResponse
    {
        public bool isSuccess { get; set; }
        public string userMessage { get; set; }
        public string devMessage { get; set; }
        public object data { get; set; }

    }
}
