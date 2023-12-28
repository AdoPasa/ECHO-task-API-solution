using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Refit;
using System.Globalization;
using YahooFinanceAgregator.Domain;
using YahooFinanceAgregator.ExternalClients;
using YahooFinanceAgregator.ExternalClients.YahooFinance.Models;
using YahooFinanceAgregator.Models;
using YahooFinanceAgregator.Stores;

namespace YahooFinanceAgregator.Services
{
    public class TickerService
    {
        private AppDbContext _dbContext;
        private IYahooFinance _yahooFinanceClient;
        private YahooSessionStore _yahooSessionStore;

        public TickerService(AppDbContext dbContext, IYahooFinance yahooFinanceClient, YahooSessionStore yahooSessionStore)
        {
            _dbContext = dbContext;
            _yahooFinanceClient = yahooFinanceClient;
            _yahooSessionStore = yahooSessionStore;
        }

        public async Task<List<GetTickerResponse>> GetTickerData(List<string> symbols, DateTime date)
        {
            symbols = symbols.Select(s => s.ToUpper()).ToList();

            var query = from tickerData in _dbContext.Tickers.Where(t => symbols.Contains(t.Symbol))
                        from tickerHistoryData in _dbContext.TickerHistory.Where(th => tickerData.Id == th.TickerId && th.Date == date).DefaultIfEmpty()
                        select new { Ticker = tickerData, History = tickerHistoryData };

            var data = await query.AsNoTracking().ToListAsync();

            var transformedTickerData = data
                .Select(td => {
                    var historyData = td.History;

                    return new GetTickerResponse
                    {
                        Symbol = td.Ticker.Symbol,
                        Date = date.ToString("yyyy-MM-dd"),
                        MarketTimeZone = td.Ticker.MarketTimeZone,
                        CompanyName = td.Ticker.Name,
                        FoundedYear = td.Ticker.FoundedYear,
                        HeadquarterCity = td.Ticker.City,
                        HeadquarterState = td.Ticker.State,
                        MarketCap = td.Ticker.MarketCap,
                        NumberOfEmployees = td.Ticker.NumberOfEmployees,
                        Status = historyData == null ? GetTickerStatusResponse.MissingHistory : GetTickerStatusResponse.DataExists,
                        OpenPrice = historyData == null ? 0 : historyData.OpenPrice,
                        ClosedPrice = historyData == null ? 0 : historyData.ClosePrice,
                    };
                })
                .ToList();

            var missingData = symbols
                .Where(s => !transformedTickerData.Any(ttd => ttd.Symbol == s))
                .Select(s => new GetTickerResponse { 
                    Symbol = s,
                    Date = date.ToString("yyyy-MM-dd"),
                    Status = GetTickerStatusResponse.MissingTicker,
                })
                .ToList();


            transformedTickerData.AddRange(missingData);
            return transformedTickerData;
        }

        public async Task<GetTickerResponse> GetTickerData(string symbol, DateTime date) 
        {
            var tickerData = await FetchTickerData(symbol);

            if (tickerData == null)
                return new GetTickerResponse { Status = GetTickerStatusResponse.TickerNotFound, Symbol = symbol };

            var thickerHistory = await FetchHistoryData(tickerData.Id , symbol, date);

            return new GetTickerResponse
            {
                Symbol = tickerData.Symbol,
                Date = date.ToString("yyyy-MM-dd"),
                MarketTimeZone = tickerData.MarketTimeZone,
                CompanyName = tickerData.Name,
                FoundedYear = tickerData.FoundedYear,
                HeadquarterCity = tickerData.City,
                HeadquarterState = tickerData.State,
                MarketCap = tickerData.MarketCap,
                NumberOfEmployees = tickerData.NumberOfEmployees,
                Status = thickerHistory == null ? GetTickerStatusResponse.HistoryNotFound : GetTickerStatusResponse.DataExists,
                OpenPrice = thickerHistory == null ? 0 : thickerHistory.OpenPrice,
                ClosedPrice = thickerHistory == null ? 0 : thickerHistory.ClosePrice,
            };    
        }

        public async Task<Ticker?> FetchTickerData(string symbol)
        {
            var ticker = await _dbContext.Tickers
                .Where(t => t.Symbol == symbol.ToUpper())
                .FirstOrDefaultAsync();

            if (ticker == null) {
                var tickerQuote = await _yahooFinanceClient.GetQuote(_yahooSessionStore.GetSessionCookie(), _yahooSessionStore.GetSessionCrumb(), symbol);

                if (tickerQuote.QuoteResponse == null || tickerQuote.QuoteResponse.Result.Count == 0)
                    return null;

                var tickerQuoteData = tickerQuote.QuoteResponse.Result.First();
                ticker = new Ticker { 
                    Symbol = symbol.ToUpper(),
                    Name = string.IsNullOrEmpty(tickerQuoteData.LongName) ? tickerQuoteData.ShortName : tickerQuoteData.LongName,
                    MarketCap = tickerQuoteData.MarketCap,
                    MarketTimeZone = tickerQuoteData.ExchangeTimezoneName,
                };


                if(string.IsNullOrEmpty(ticker.Name))
                    return null;

                try
                {
                    var tickerQuoteSummary = await _yahooFinanceClient.GetQuoteSummary(_yahooSessionStore.GetSessionCookie(), _yahooSessionStore.GetSessionCrumb(), symbol);

                    if (tickerQuoteSummary.QuoteSummary != null)
                    {
                        var tickerQuoteSummaryData = tickerQuoteSummary.QuoteSummary.Result.FirstOrDefault();

                        if (tickerQuoteSummaryData?.AssetProfile != null)
                        {
                            ticker.Address = tickerQuoteSummaryData.AssetProfile.Address;
                            ticker.City = tickerQuoteSummaryData.AssetProfile.City;
                            ticker.State = tickerQuoteSummaryData.AssetProfile.State;
                            ticker.NumberOfEmployees = tickerQuoteSummaryData.AssetProfile.FullTimeEmployees;
                            ticker.FoundedYear = tickerQuoteSummaryData.AssetProfile.FoundedYear;
                        }
                    }
                }
                catch (ApiException ex)
                {
                    // TODO: Add log that the data is not found for the given symbol
                    if (ex.StatusCode != System.Net.HttpStatusCode.NotFound)
                    {
                        // TODO: add log that there was an error while fetching the data for the given ticker
                        throw ex;
                    }

                }

                _dbContext.Add(ticker);
                await _dbContext.SaveChangesAsync();
            }

            return ticker;
        }

        private async Task<TickerHistory?> FetchHistoryData(int tickerId, string symbol, DateTime date) 
        {
            var tickerHistoryData = await _dbContext.TickerHistory
                .Where(th => th.TickerId == tickerId && th.Date == date)
                .FirstOrDefaultAsync();

            if (tickerHistoryData != null)
                return tickerHistoryData;


            string historyCSV = string.Empty;
            try
            {
                var startDateTimestamp = ((DateTimeOffset)date.AddHours(6)).ToUnixTimeSeconds();
                var endDateTimestamp = ((DateTimeOffset)date.AddHours(30)).ToUnixTimeSeconds();
                historyCSV = await _yahooFinanceClient.GetHistory(symbol, startDateTimestamp, endDateTimestamp);
            }
            catch (ApiException ex)
            {
                // TODO: Add log that the data is not found for the given date
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                // TODO: add log that there was an error while fetching the data for the given ticker
                throw ex;
            }


            using (TextReader reader = new StringReader(historyCSV))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var parsedData = csv
                    .GetRecords<TickerHistoryCSVResponseModel>()
                    .FirstOrDefault();

                if(parsedData == null)
                    return null;

                tickerHistoryData = new TickerHistory
                {
                    TickerId = tickerId,
                    OpenPrice = parsedData.OpenPrice,
                    ClosePrice = parsedData.AdjustedClosePrice,
                    Date = date,
                };
            }


            _dbContext.Add(tickerHistoryData);
            await _dbContext.SaveChangesAsync();

            return tickerHistoryData;
        }
    }
}
