namespace TradingPlatform.Models
{
    public class OrderRequest
    {
        public Order order { get; set; } = new Order();

        public class Order
        {
            public string Type { get; set; } = "MARKET";
            public string Instrument { get; set; } = string.Empty;
            public int Units { get; set; } = 0;
            public string TimeInForce { get; set; } = "FOK"; // Fill Or Kill
            public string PositionFill { get; set; } = "DEFAULT";
            public StopLossOnFill? StopLossOnFill { get; set; }
            public TakeProfitOnFill? TakeProfitOnFill { get; set; }
        }

        public class StopLossOnFill
        {
            public string TimeInForce { get; set; } = "GTC";
            public string Price { get; set; } = string.Empty;
        }

        public class TakeProfitOnFill
        {
            public string Price { get; set; } = string.Empty;
        }
    }
}
