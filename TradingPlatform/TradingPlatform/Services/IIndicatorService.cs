using TradingPlatform.Models.ApiModels;

namespace TradingPlatform.Services
{
    public interface IIndicatorService
    {
        IEnumerable<IndicatorResult> CalculateIndicators(IEnumerable<Candle> candles, IndicatorConfig config);
    }
}