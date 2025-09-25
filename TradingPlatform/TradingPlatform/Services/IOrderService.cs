using TradingPlatform.Models.ApiModels;

namespace TradingPlatform.Services
{
    public interface IOrderService
    {
        Task<int> ExecuteOrder(IEnumerable<Candle> candles, IndicatorConfig config);
    }
}
