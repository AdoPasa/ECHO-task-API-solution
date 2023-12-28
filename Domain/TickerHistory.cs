namespace YahooFinanceAgregator.Domain
{
    public class TickerHistory
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal ClosePrice { get; set; }

        public int TickerId { get; set; }
        public Ticker? Ticker { get; set; }
    }
}
