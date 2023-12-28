namespace YahooFinanceAgregator.Stores
{
    public class YahooSessionStore
    {
        private string _sessionCookie = string.Empty;
        private string _sessionCrumb = string.Empty;

        public YahooSessionStore(IConfiguration configuration)
        {
            _sessionCookie = configuration.GetValue<string>("YahooFinanceSession:Cookie");
            _sessionCrumb = configuration.GetValue<string>("YahooFinanceSession:Crumb");
        }

        public string GetSessionCookie() { 
            return _sessionCookie;
        }

        public string GetSessionCrumb()
        {
            return _sessionCrumb;
        }
    }
}
