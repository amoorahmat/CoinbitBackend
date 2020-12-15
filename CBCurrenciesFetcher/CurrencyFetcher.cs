using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;

namespace CBCurrenciesFetcher
{
    public class CurrencyFetcher
    {
        string _goldUrl, _currencyUrl;

        public CurrencyFetcher(string goldUrl, string currencyUrl)
        {
            _goldUrl = goldUrl;
            _currencyUrl = currencyUrl;
        }

        public async Task<IEnumerable<GoldAndCurrencyModel>> GetGoldData()
        {
            return await GetData(_goldUrl);
        }

        public async Task<IEnumerable<GoldAndCurrencyModel>> GetCurrencyData()
        {
            return await GetData(_currencyUrl);
        }

        private async Task<IEnumerable<GoldAndCurrencyModel>> GetData(string url)
        {
            try
            {
                using (new HttpClient())
                {
                    string result;
                    using (HttpClient httpClient = new HttpClient())
                    {
                        result = await httpClient.GetStringAsync(new Uri(url));
                    }


                    if (string.IsNullOrWhiteSpace(result))
                        return null;

                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(result);

                    return ConvertData(xmlDocument);
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private List<GoldAndCurrencyModel> ConvertData(XmlDocument xml)
        {
            XmlNodeList elemList = xml.GetElementsByTagName("item");

            var datalist = new List<GoldAndCurrencyModel>();
            foreach (XmlNode item in elemList)
            {
                GoldAndCurrencyModel obj = new GoldAndCurrencyModel();
                foreach (XmlNode node in item.ChildNodes)
                {
                    switch (node.Name.ToLower())
                    {
                        case "name":
                            obj.name = node.InnerText;
                            break;
                        case "price":
                            obj.price = node.InnerText;
                            break;
                        case "change":
                            obj.change = node.InnerText;
                            break;
                        case "percent":
                            obj.percent = node.InnerText;
                            break;
                    }
                }
                datalist.Add(obj);
            }
            return datalist;
        }
    }
}
