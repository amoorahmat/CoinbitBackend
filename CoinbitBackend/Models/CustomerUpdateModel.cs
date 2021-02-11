using System;

namespace CoinbitBackend.Models
{
    public class CustomerUpdateModel
    {
        public long customer_id { get; set; }   
        public string father_name { get; set; }
        public string tel { get; set; }
        public DateTime birth_date { get; set; }
        public string national_code { get; set; }
        public int bank_id { get; set; }
        public string card_number { get; set; }

    }
}
