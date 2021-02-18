namespace CoinbitBackend.Models
{
    public class CoreResponse
    {
        public bool isSuccess { get; set; }
        public string userMessage { get; set; }
        public string devMessage { get; set; }        
        public long? total_items { get; set; }
        public int? total_pages { get; set; }
        public int? current_page { get; set; }
        public object data { get; set; }
    }
}