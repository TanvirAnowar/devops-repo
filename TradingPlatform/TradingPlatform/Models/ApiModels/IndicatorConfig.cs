namespace TradingPlatform.Models.ApiModels
{
    public class IchimokuConfig
    {
        public int Tenkan { get; set; } = 9;
        public int Kijun { get; set; } = 26;
        public int SenkouSpanB { get; set; } = 52;
        public int Displacement { get; set; } = 26;
    }

    public class RsiConfig
    {
        public int Period { get; set; } = 14;
    }

    public class AdxConfig
    {
        public int Period { get; set; } = 14;
    }

    public class IndicatorConfig
    {
        public IchimokuConfig Ichimoku { get; set; } = new IchimokuConfig();
        public RsiConfig Rsi { get; set; } = new RsiConfig();

        public AdxConfig Adx { get; set; } = new AdxConfig();
    }
}