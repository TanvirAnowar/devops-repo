namespace TradingPlatform.Models
{
    public class OrderResponse
    {
        public OrderCreateTransaction? orderCreateTransaction { get; set; }
        public OrderFillTransaction? orderFillTransaction { get; set; }
        public TradeOpened? tradeOpened { get; set; }
        public List<string>? RelatedTransactionIDs { get; set; }
        public string? LastTransactionID { get; set; }

        // Nested classes
        public class OrderCreateTransaction
        {
            public string? Id { get; set; }
            public string? Time { get; set; }
            public long? UserID { get; set; }
            public string? AccountID { get; set; }
            public string? BatchID { get; set; }
            public string? Type { get; set; }
            public string? Instrument { get; set; }
            public string? Units { get; set; }
            public string? TimeInForce { get; set; }
            public string? PositionFill { get; set; }
            public ClientExtensions? ClientExtensions { get; set; }
            public PriceObject? TakeProfitOnFill { get; set; }
            public PriceObject? StopLossOnFill { get; set; }
            public ClientExtensions? TradeClientExtensions { get; set; }
        }

        public class OrderFillTransaction
        {
            public string? Id { get; set; }
            public string? Time { get; set; }
            public long? UserID { get; set; }
            public string? AccountID { get; set; }
            public string? BatchID { get; set; }
            public string? Type { get; set; }
            public string? OrderID { get; set; }
            public TradeOpenedDetails? TradeOpened { get; set; }
            public string? Pl { get; set; }
            public string? Financing { get; set; }
            public string? Commission { get; set; }
        }

        public class TradeOpenedDetails
        {
            public string? TradeID { get; set; }
            public string? Units { get; set; }
            public string? Price { get; set; }
            public string? RealizedPL { get; set; }
            public string? Financing { get; set; }
        }

        public class TradeOpened
        {
            public string? TradeID { get; set; }
            public string? Units { get; set; }
            public string? Price { get; set; }
            public string? Instrument { get; set; }
            public string? InitialMarginRequired { get; set; }
        }

        public class PriceObject
        {
            public string Price { get; set; } = string.Empty;
        }

        public class ClientExtensions
        {
            public string? Id { get; set; }
            public string? Tag { get; set; }
            public string? Comment { get; set; }
        }

        public class StopLossOrder
        {
            public string? ID { get; set; }
            public decimal? Price { get; set; }
        }

        public class TakeProfitOrder
        {
            public string? ID { get; set; }
            public decimal? Price { get; set; }
        }
    }
}
