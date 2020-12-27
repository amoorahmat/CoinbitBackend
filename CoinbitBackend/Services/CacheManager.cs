using CoinbitBackend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoinbitBackend.Services
{
    public class CacheManager
    {
        private List<CoinDataViewModel> CoinDatas;

        public CacheManager()
        {
            CoinDatas = new List<CoinDataViewModel>();
        }

        public void AddCoinLog(IEnumerable<CoinData> coinDatas)
        {
            try
            {
                this.CoinDatas = null;
                this.CoinDatas = new List<CoinDataViewModel>();
                this.CoinDatas.AddRange(coinDatas.ConvertToCoinDataView());
            }
            catch (Exception ex)
            {
                //todo log
            }
        }


        public List<CoinDataViewModel> GetCoinLog()
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

        public CoinDataViewModel GetCoinBySymbol(string symbol)
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
