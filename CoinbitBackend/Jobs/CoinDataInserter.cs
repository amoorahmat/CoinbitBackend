using CBCoinsFetcher;
using CoinbitBackend.Entities;
using CoinbitBackend.Extension;
using CoinbitBackend.Services;
using Coravel.Invocable;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoinbitBackend.Jobs
{
    public class CoinDataInserter : IInvocable
    {
        private CoinsFetcher _coinsFetcher;
        private DBRepository _dBRepository;

        public CoinDataInserter(CoinsFetcher coinsFetcher, DBRepository dBRepository)
        {
            _coinsFetcher = coinsFetcher;
            _dBRepository = dBRepository;
        }

        public async Task Invoke()
        {
            var datalst = _coinsFetcher.GetCoinData();
            var seriesDate = DateTime.Now;

            var insertData = new List<CoinData>();
            foreach (var item in datalst)
            {
                insertData.Add(new CoinData()
                {
                    CoinId = item.Id,
                    Name = item.Name,
                    Symbol = item.Symbol,
                    Rank = item.Rank,
                    Price = item.Price,
                    Volume24hUsd = item.Volume24hUsd,
                    MarketCapUsd = item.MarketCapUsd,
                    PercentChange1h = item.PercentChange1h,
                    PercentChange24h = item.PercentChange24h,
                    PercentChange7d = item.PercentChange7d,
                    LastUpdated = item.LastUpdated,
                    MarketCapConvert = item.MarketCapConvert,
                    ConvertCurrency = item.ConvertCurrency,
                    SeriesDate = seriesDate
                });
            }

            await _dBRepository.AddRangeAsync(insertData);
            await _dBRepository.SaveChangesAsync();
        }
    }
}