using Microsoft.EntityFrameworkCore;
using TradingPlatform.Models.DbModels;
using TradingPlatform.Services;

public class TradingPlatformDbContext : DbContext
{
    public TradingPlatformDbContext(DbContextOptions<TradingPlatformDbContext> options)
        : base(options) { }

    public DbSet<TradeStatusModel> TradeStatuses { get; set; }
    public DbSet<ActiveOrder> ActiveOrders { get; set; }
}

