using TradingPlatform.Models;

namespace TradingPlatform.Services
{
    public interface IOandaService
    {
        Task<List<Candle>> GetCandlesAsync(
            string instrument = "EUR_USD", 
            string granularity = "H1", 
            int count = 150, 
            string bearerToken = "");

        Task<decimal> CalculateLotSizeAsync(
            string pair,
            decimal riskPercent,
            IndicatorResult lastCandle,
            decimal stopLossPips,
            Bias marketBias
        );
        
        Task<OrderResponse> PlaceOrderAsync(
            OrderRequest orderRequest,
            string bearerToken = ""
        );
    }
}