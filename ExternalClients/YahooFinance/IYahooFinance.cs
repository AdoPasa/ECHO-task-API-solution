using Refit;
using YahooFinanceAgregator.ExternalClients.YahooFinance.Models;

namespace YahooFinanceAgregator.ExternalClients
{
    public interface IYahooFinance
    {
        /*
            * Note: I planned to extend the API client to fetch the crum and session cookie value.
                    Based on the online resources I should use the following method in order https://query1.finance.yahoo.com/v1/test/getcrumb in order to fetch the crumb value
                    Because this request always returned "429 Too Many Requests" I created a workaround and created an YahooSessionStore where I loaded the data from the appsettings file
        */

        [Get("/v7/finance/quote?symbols={symbol}&crumb={crumb}")]
        Task<TickerQuoteResponseWrapper> GetQuote([Header("cookie")] string sessionCookie, string crumb, [Query] string symbol);

        [Get("/v10/finance/quoteSummary/{symbol}?formatted=true&modules=assetProfile&crumb={crumb}")]
        Task<TickerQuoteSummaryWrapperResponse> GetQuoteSummary([Header("cookie")] string sessionCookie, string crumb, [Query] string symbol);


        [Get("/v7/finance/download/{symbol}?interval=1d&events=history&includeAdjustedClose=true&period1={startDateTimestamp}&period2={endDateTimestamp}")]
        Task<string> GetHistory([Query] string symbol, long startDateTimestamp, long endDateTimestamp);
    }
}
