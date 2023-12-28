using Refit;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace YahooFinanceAgregator.ExternalClients.YahooFinance.Models
{
    public class TickerQuoteSummaryWrapperResponse
    {
        [JsonPropertyName("quoteSummary")]
        public TickerQuoteSummaryResponse? QuoteSummary { get; set; }

    }

    public class TickerQuoteSummaryResponse
    {
        [JsonPropertyName("result")]
        public List<TickerQuoteSummaryResultResponse> Result { get; set; } = new List<TickerQuoteSummaryResultResponse> ();

        public string? Error { get; set; }
    }
    
    public class TickerQuoteSummaryResultResponse
    {
        [JsonPropertyName("assetProfile")]
        public TickerQuoteSummaryAssetProfileResponse? AssetProfile { get; set; }
    }

    public class TickerQuoteSummaryAssetProfileResponse
    {
        [JsonPropertyName("address1")]
        public string Address { get; set; } = string.Empty;

        [JsonPropertyName("city")]
        public string City { get; set; } = string.Empty;

        [JsonPropertyName("state")]
        public string State { get; set; } = string.Empty;

        [JsonPropertyName("zip")]
        public string Zip { get; set; } = string.Empty;

        [JsonPropertyName("country")]
        public string Country { get; set; } = string.Empty;

        [JsonPropertyName("fullTimeEmployees")]
        public int FullTimeEmployees { get; set; }

        [JsonPropertyName("longBusinessSummary")]
        public string BusinnesDescription { get; set; } = string.Empty;

        public int FoundedYear
        {
            get {
                string pattern = @"\bfounded[\sa-z]+(\d{4})\b";
                Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                Match match = regex.Match(BusinnesDescription);

                if (match.Success)
                {  
                    return int.Parse(match.Groups[1].Value);
                }

                return 0;
            }
        }
    }
}
