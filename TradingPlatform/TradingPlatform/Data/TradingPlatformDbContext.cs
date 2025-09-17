using Microsoft.EntityFrameworkCore;

public class TradingPlatformDbContext : DbContext
{
    public TradingPlatformDbContext(DbContextOptions<TradingPlatformDbContext> options)
        : base(options) { }

    public DbSet<TradeStatus> TradeStatuses { get; set; }
}