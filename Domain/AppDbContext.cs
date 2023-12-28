using Microsoft.EntityFrameworkCore;

namespace YahooFinanceAgregator.Domain
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            :base(options) { 
        }

        public DbSet<Ticker> Tickers { get; set; } = null!;
        public DbSet<TickerHistory> TickerHistory { get; set; } = null!;
    }
}
