using NoobsMuc.Coinmarketcap.Client;
using System.Collections.Generic;

namespace CBCoinsFetcher
{
    public class CoinsFetcher
    {
        private string _apiKey;
        private int _count;

        public CoinsFetcher(string apikey, int count)
        {
            _apiKey = apikey;
            _count = count;
        }

        public IEnumerable<Currency> GetCoinData()
        {    
            return ((ICoinmarketcapClient)new CoinmarketcapClient(_apiKey)).GetCurrencies(_count);
        }
    }
}
