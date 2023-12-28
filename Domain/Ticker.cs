namespace YahooFinanceAgregator.Domain
{
    public class Ticker
    {
        public int Id { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string MarketTimeZone { get; set; } = string.Empty;

        public int NumberOfEmployees { get; set; }
        public int FoundedYear { get; set; }
        public long MarketCap { get; set; }

        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Zip { get; set; } = string.Empty;


        public List<TickerHistory> History = new List<TickerHistory>(); 
    }
}
