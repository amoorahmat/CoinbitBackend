using CoinbitBackend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoinbitBackend.Services
{
    public class CacheManager
    {
        private List<CoinData> CoinDatas;

        public CacheManager()
        {
            CoinDatas = new List<CoinData>();
        }

        public void AddCoinLog(IEnumerable<CoinData> coinDatas)
        {
            try
            {
                this.CoinDatas = null;
                this.CoinDatas = new List<CoinData>();
                this.CoinDatas.AddRange(coinDatas);
            }
            catch (Exception ex)
            {
                //todo log
            }
        }


        public List<CoinData> GetCoinLog()
        {
            try
            {
                return CoinDatas.OrderBy(p => p.Ranking).ToList();
            }
            catch (Exception ex)
            {
                //todo log
                return null;
            }
        }

        public CoinData GetCoinBySymbol(string symbol)
        {
            try
            {
                return CoinDatas.FirstOrDefault(l => l.Symbol == symbol);
            }
            catch (Exception ex)
            {
                //todo log
                return null;
            }
        }
    }
}
