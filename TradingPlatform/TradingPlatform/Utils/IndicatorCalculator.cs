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

        public static List<decimal?> CalculateAdxLastCandles(List<Candle> candles, int period = 14, int lastN = 10)
        {
            var results = new List<decimal?>();
            int total = candles.Count;

            if (total < period * 2)
                return Enumerable.Repeat<decimal?>(null, lastN).ToList();

            // Running smoothed values
            decimal tr14 = 0, plusDm14 = 0, minusDm14 = 0;

            // Initialize sums for the first period
            for (int i = 1; i <= period; i++)
            {
                var cur = candles[i];
                var prev = candles[i - 1];

                var tr = Math.Max(
                    (double)(cur.High - cur.Low),
                    Math.Max(
                        Math.Abs((double)(cur.High - prev.Close)),
                        Math.Abs((double)(cur.Low - prev.Close))
                    )
                );

                var upMove = cur.High - prev.High;
                var downMove = prev.Low - cur.Low;

                if (upMove > downMove && upMove > 0) plusDm14 += upMove;
                if (downMove > upMove && downMove > 0) minusDm14 += downMove;

                tr14 += (decimal)tr;
            }

            // Process the rest until the end
            decimal? adx = null;
            var dxValues = new List<decimal>();

            for (int i = period + 1; i < total; i++)
            {
                var cur = candles[i];
                var prev = candles[i - 1];

                // TR
                var tr = Math.Max(
                    (double)(cur.High - cur.Low),
                    Math.Max(
                        Math.Abs((double)(cur.High - prev.Close)),
                        Math.Abs((double)(cur.Low - prev.Close))
                    )
                );

                // +DM / -DM
                var upMove = cur.High - prev.High;
                var downMove = prev.Low - cur.Low;

                decimal plusDm = 0, minusDm = 0;
                if (upMove > downMove && upMove > 0) plusDm = upMove;
                if (downMove > upMove && downMove > 0) minusDm = downMove;

                // Wilder smoothing
                tr14 = tr14 - (tr14 / period) + (decimal)tr;
                plusDm14 = plusDm14 - (plusDm14 / period) + plusDm;
                minusDm14 = minusDm14 - (minusDm14 / period) + minusDm;

                // DI
                var plusDi = 100 * (plusDm14 / tr14);
                var minusDi = 100 * (minusDm14 / tr14);

                if (plusDi + minusDi == 0) continue;

                var dx = 100 * Math.Abs(plusDi - minusDi) / (plusDi + minusDi);
                dxValues.Add(dx);

                // First ADX = average of first "period" DX values
                if (dxValues.Count == period)
                    adx = dxValues.Average();
                else if (dxValues.Count > period && adx != null)
                    adx = ((adx.Value * (period - 1)) + dx) / period;

                // Store only last N values
                if (i >= total - lastN && adx != null)
                    results.Add(adx);
            }

            return results;
        }

        public static List<decimal?> CalculateAtrLastCandles(List<Candle> candles, int period = 14, int lastN = 10)
        {
            var results = new List<decimal?>();
            int total = candles.Count;

            if (total < period + lastN)
                return Enumerable.Repeat<decimal?>(null, lastN).ToList();

            // Step 1: compute initial ATR (simple average of first 'period' TR values)
            decimal atr = 0;
            for (int i = 1; i <= period; i++)
            {
                var cur = candles[i];
                var prev = candles[i - 1];
                var tr = Math.Max(
                    (double)(cur.High - cur.Low),
                    Math.Max(
                        Math.Abs((double)(cur.High - prev.Close)),
                        Math.Abs((double)(cur.Low - prev.Close))
                    )
                );
                atr += (decimal)tr;
            }
            atr /= period;

            // Step 2: Wilder’s smoothing for the rest
            for (int i = period + 1; i < total; i++)
            {
                var cur = candles[i];
                var prev = candles[i - 1];
                var tr = Math.Max(
                    (double)(cur.High - cur.Low),
                    Math.Max(
                        Math.Abs((double)(cur.High - prev.Close)),
                        Math.Abs((double)(cur.Low - prev.Close))
                    )
                );

                atr = ((atr * (period - 1)) + (decimal)tr) / period;

                // Save only last N
                if (i >= total - lastN)
                    results.Add(atr);
            }

            // Ensure exactly lastN elements
            while (results.Count < lastN)
                results.Insert(0, null);

            return results;
        }

    }
}