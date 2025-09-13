using TradingPlatform.Models;
using TradingPlatform.Utils;

namespace TradingPlatform.Services
{
    public class IndicatorService : IIndicatorService
    {
        public IEnumerable<IndicatorResult> CalculateIndicators(IEnumerable<Candle> candles, IndicatorConfig config)
        {
            var candleList = candles.ToList();
            var results = new List<IndicatorResult>();

            for (int i = 0; i < candleList.Count; i++)
            {
                var candle = candleList[i];
                var result = new IndicatorResult
                {
                    Time = candle.Time,
                    Open = candle.Open,
                    High = candle.High,
                    Low = candle.Low,
                    Close = candle.Close,
                    Volume = candle.Volume
                };

                result.TenkanSen = IndicatorCalculator.CalculateTenkan(candleList, i, config.Ichimoku.Tenkan);
                result.KijunSen = IndicatorCalculator.CalculateKijun(candleList, i, config.Ichimoku.Kijun);
                result.SenkouSpanB = IndicatorCalculator.CalculateSenkouSpanB(candleList, i, config.Ichimoku.SenkouSpanB);
                result.SenkouSpanA = IndicatorCalculator.CalculateSenkouSpanA(result.TenkanSen, result.KijunSen);
                result.ChikouSpan = IndicatorCalculator.CalculateChikou(candleList, i, config.Ichimoku.Displacement);

                result.Rsi = IndicatorCalculator.CalculateRsi(candleList, i, config.Rsi.Period);

                results.Add(result);
            }

            return results.GetRange(results.Count -2, 2);
        }
    }
}