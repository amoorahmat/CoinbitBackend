using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinbitBackend.Entities
{
    public static class Conversion
    {
        public static IEnumerable<CoinDataViewModel> ConvertToCoinDataView(this IEnumerable<CoinData> coinDatas)
        {
            foreach (var item in coinDatas)
            {
                yield return new CoinDataViewModel()
                {
                    Id = item.Id,
                    CoinId = item.CoinId,
                    Name = item.Name,
                    Symbol = item.Symbol,
                    Ranking = item.Ranking,
                    Price = item.Price,
                    Volume24hUsd = item.Volume24hUsd,
                    MarketCapUsd = item.MarketCapUsd,
                    PercentChange1h = item.PercentChange1h,
                    PercentChange24h = item.PercentChange24h,
                    PercentChange7d = item.PercentChange7d,
                    LastUpdated = item.LastUpdated,
                    MarketCapConvert = item.MarketCapConvert,
                    ConvertCurrency = item.ConvertCurrency,
                    createDate = item.createDate,
                    SeriesDate = item.SeriesDate
                };
            }
        }
    }
}
