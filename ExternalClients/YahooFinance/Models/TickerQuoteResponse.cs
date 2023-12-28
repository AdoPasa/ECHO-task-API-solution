using Refit;
using System.Text.Json.Serialization;

namespace YahooFinanceAgregator.ExternalClients.YahooFinance.Models
{
    public class TickerQuoteResponseWrapper
    {
        [JsonPropertyName("quoteResponse")]
        public TickerQuoteResponse? QuoteResponse { get; set; }

    }

    public class TickerQuoteResponse
    {
        [JsonPropertyName("result")]
        public List<TickerQuoteResultResponse> Result { get; set; } = new List<TickerQuoteResultResponse> ();

        public string? Error { get; set; }
    }
    
    public class TickerQuoteResultResponse
    {
        [JsonPropertyName("longName")]
        public string LongName { get; set; } = string.Empty;

        [JsonPropertyName("shortName")]
        public string ShortName { get; set; } = string.Empty;

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;

        [JsonPropertyName("exchangeTimezoneName")]
        public string ExchangeTimezoneName { get; set; } = string.Empty;

        [JsonPropertyName("marketCap")]
        public long MarketCap { get; set; }

    }
}
