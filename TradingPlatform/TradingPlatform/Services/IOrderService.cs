using TradingPlatform.Models;

namespace TradingPlatform.Services
{
    public interface IOrderService
    {
        Task<int> ExecuteOrder(IEnumerable<Candle> candles, IndicatorConfig config);
    }
}
