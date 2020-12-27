using MD.PersianDateTime;

namespace CoinbitBackend.Entities
{
    public class CoinDataViewModel : CoinData
    {
        public string SeriesDateFa
        {
            get
            {
                var str = new PersianDateTime(SeriesDate);
                return str.ToString();
            }
        }
    }
}
