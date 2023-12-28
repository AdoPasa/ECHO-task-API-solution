namespace YahooFinanceAgregator.Models
{
    public class GetTickersRequest
    {
        public List<string> Symbols { get; set; } = new List<string>();
        public DateTime Date { get; set; }
    }
}
