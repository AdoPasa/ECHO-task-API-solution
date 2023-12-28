using System;

namespace YahooFinanceAgregator.Models
{
    public class GetTickerResponse
    {
        public string Symbol { get; set; } = string.Empty;
        public GetTickerStatusResponse Status { get; set; } = GetTickerStatusResponse.MissingTicker;

        public string CompanyName { get; set; } = string.Empty;
        public long MarketCap { get; set; }
        public int FoundedYear { get; set; }
        public int NumberOfEmployees { get; set; }

        public string HeadquarterCity { get; set; } = string.Empty;
        public string HeadquarterState { get; set; } = string.Empty;

        public string Date { get; set; } = string.Empty;

        public decimal ClosedPrice { get; set; }
        public decimal OpenPrice { get; set; }

        public string MarketTimeZone { get; set; } = string.Empty;
        public string LocalTimeZone { 
            get {
                return TimeZoneInfo.Local.DisplayName;
            } 
        }

        public string MarketOpenDateTime {
            get {
                if (string.IsNullOrEmpty(this.Date))
                    return string.Empty;

                var marketDate = DateTime.Parse(this.Date).AddHours(9).AddMinutes(30);
                return marketDate.ToString("yyyy-MM-dd HH:mm");
            }
        }

        public string LocalOpenDateTime
        {
            get {
                if (string.IsNullOrEmpty(MarketTimeZone) || string.IsNullOrEmpty(this.MarketOpenDateTime))
                    return string.Empty;

                TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById(MarketTimeZone);
                var marketDate = DateTime.Parse(this.MarketOpenDateTime);
                var locaTime = TimeZoneInfo.ConvertTime(marketDate, timeInfo, TimeZoneInfo.Local);

                return locaTime.ToString("yyyy-MM-dd HH:mm");
            }
        }

        public string MarketCloseDateTime
        {
            get
            {
                if (string.IsNullOrEmpty(this.Date))
                    return string.Empty;

                var marketDate = DateTime.Parse(this.Date).AddHours(16);
                return marketDate.ToString("yyyy-MM-dd HH:mm");
            }
        }

        public string LocalCloseDateTime
        {
            get
            {
                if (string.IsNullOrEmpty(MarketTimeZone) || string.IsNullOrEmpty(this.MarketCloseDateTime))
                    return string.Empty;

                TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById(MarketTimeZone);
                var marketDate = DateTime.Parse(this.MarketCloseDateTime);
                var locaTime = TimeZoneInfo.ConvertTime(marketDate, timeInfo, TimeZoneInfo.Local);

                return locaTime.ToString("yyyy-MM-dd HH:mm");
            }
        }
    }

    public enum GetTickerStatusResponse 
    {
        MissingTicker = 0,
        TickerNotFound,
        MissingHistory,
        HistoryNotFound,
        DataExists
    }
}
