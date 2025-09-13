namespace TradingPlatform.Models
{
    public class IndicatorResult
    {
        public DateTime Time { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public long Volume { get; set; }

        public decimal? TenkanSen { get; set; }
        public decimal? KijunSen { get; set; }
        public decimal? SenkouSpanA { get; set; }
        public decimal? SenkouSpanB { get; set; }
        public decimal? ChikouSpan { get; set; }

        public decimal? Rsi { get; set; }
    }
}