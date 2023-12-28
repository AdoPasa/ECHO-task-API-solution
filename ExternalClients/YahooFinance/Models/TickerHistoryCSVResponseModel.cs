
using CsvHelper.Configuration.Attributes;

namespace YahooFinanceAgregator.ExternalClients.YahooFinance.Models
{
    public class TickerHistoryCSVResponseModel
    {
        //Date,Open,High,Low,Close,Adj Close, Volume
        //2023-12-12,193.080002,194.720001,191.720001,194.710007,194.710007,52696900
        [Index(0)]
        public string Date { get; set; } = string.Empty;
        
        [Index(1)]
        public decimal OpenPrice { get; set; }

        [Index(2)]
        public decimal HighPrice { get; set; }

        [Index(3)]
        public decimal LowPrice { get; set; }

        [Index(4)]
        public decimal ClosePrice { get; set; }

        [Index(5)]
        public decimal AdjustedClosePrice { get; set; }

        [Index(6)]
        public long Volume { get; set; }
    }
}
