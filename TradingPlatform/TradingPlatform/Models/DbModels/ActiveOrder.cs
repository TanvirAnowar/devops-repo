using System;

namespace TradingPlatform.Models.DbModels
{
    
    public class ActiveOrder
    {
        public string Id { get; set; } = string.Empty;

        public string Unites { get; set; } = string.Empty;

        public string EntryPrice { get; set; } = string.Empty;

        public string StopLossPrice { get; set; } = string.Empty;

        public string TakeProfitPrice { get; set; } = string.Empty;

        public string OrderJsonObject { get; set; } = string.Empty;

        public bool IsOrderActive { get; set; }

        public DateTime OrderDateTime { get; set; }
    }

}
