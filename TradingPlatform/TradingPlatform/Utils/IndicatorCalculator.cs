using TradingPlatform.Models;

namespace TradingPlatform.Utils
{
    public static class IndicatorCalculator
    {
        public static decimal? CalculateTenkan(List<Candle> candles, int index, int period)
        {
            if (index < period - 1) return null;
            var slice = candles.Skip(index - period + 1).Take(period).ToList();
            if (!slice.Any()) return null;
            return (slice.Max(c => c.High) + slice.Min(c => c.Low)) / 2m;
        }

        public static decimal? CalculateKijun(List<Candle> candles, int index, int period)
        {
            if (index < period - 1) return null;
            var slice = candles.Skip(index - period + 1).Take(period).ToList();
            if (!slice.Any()) return null;
            return (slice.Max(c => c.High) + slice.Min(c => c.Low)) / 2m;
        }

        public static decimal? CalculateSenkouSpanB(List<Candle> candles, int index, int period)
        {
            if (index < period - 1) return null;
            var slice = candles.Skip(index - period + 1).Take(period).ToList();
            if (!slice.Any()) return null;
            return (slice.Max(c => c.High) + slice.Min(c => c.Low)) / 2m;
        }

        public static decimal? CalculateSenkouSpanA(decimal? tenkan, decimal? kijun)
        {
            if (tenkan == null || kijun == null) return null;
            return (tenkan + kijun) / 2m;
        }

        public static decimal? CalculateChikou(List<Candle> candles, int index, int displacement)
        {
            int target = index - displacement;
            if (target < 0 || target >= candles.Count) return null;
            return candles[target].Close;
        }

        public static decimal? CalculateRsi(List<Candle> candles, int index, int period)
        {
            if (index < period) return null;

            decimal gain = 0, loss = 0;
            for (int i = index - period + 1; i <= index; i++)
            {
                if (i <= 0) continue;
                var change = candles[i].Close - candles[i - 1].Close;
                if (change > 0) gain += change;
                else loss -= change;
            }

            if (gain == 0 && loss == 0) return null;
            if (loss == 0) return 100;

            var rs = (gain / period) / (loss / period);
            return 100 - (100 / (1 + rs));
        }
    }
}