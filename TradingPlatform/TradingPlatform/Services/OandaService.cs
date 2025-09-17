using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TradingPlatform.Models;
using TradingPlatform.Utils;

namespace TradingPlatform.Services
{
    public class OandaService : IOandaService
    {
        private readonly HttpClient _httpClient;

        public OandaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Candle>> GetCandlesAsync(
            string instrument = "EUR_USD",
            string granularity = "H1",
            int count = 150,
            string bearerToken = ""
        )
        {
            if (string.IsNullOrEmpty(bearerToken))
            {
                bearerToken = AppConfig.Get("ApiSettings:OandaAPIKey");
            }

            var url = $"{AppConfig.Get("ApiSettings:BaseUrl")}instruments/{instrument}/candles?count={count}&granularity={granularity}";

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);

            var candles = new List<Candle>();
            foreach (var item in doc.RootElement.GetProperty("candles").EnumerateArray())
            {
                if (!item.GetProperty("complete").GetBoolean()) continue;

                candles.Add(new Candle
                {
                    Time = item.GetProperty("time").GetDateTime(),
                    Open = decimal.Parse(item.GetProperty("mid").GetProperty("o").GetString()!),
                    High = decimal.Parse(item.GetProperty("mid").GetProperty("h").GetString()!),
                    Low = decimal.Parse(item.GetProperty("mid").GetProperty("l").GetString()!),
                    Close = decimal.Parse(item.GetProperty("mid").GetProperty("c").GetString()!),
                    Volume = item.GetProperty("volume").GetInt64()
                });
            }

            return candles;
        }

        public async Task<decimal> CalculateLotSizeAsync(
            string pair,
            decimal riskPercent,
            IndicatorResult lastCandle,
            decimal stopLossPips,
            Bias marketBias
        )
        {
            var bearerToken = AppConfig.Get("ApiSettings:OandaAPIKey");

            var url = $"{AppConfig.Get("ApiSettings:BaseUrl")}/summary";
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            var account = doc.RootElement.GetProperty("account");
            var balance = decimal.Parse(account.GetProperty("balance").GetString()!);

            // Example calculation for lot size (for USD accounts, major pairs/metals)
            // RiskAmount = balance * (riskPercent / 100)
            // LotSize = RiskAmount / (stopLossPips * PipValue)
            // PipValue for standard lot (100,000 units) for USD pairs is usually $10 per pip
            // For metals, adjust accordingly (e.g., XAUUSD pip value is $1 for 1 lot)

            decimal riskAmount = balance * (riskPercent / 100m);
            decimal pipValue = pair.StartsWith("XAU") ? 1m : 10m; // crude example

            //     var stopLossPips = IndicatorCalculator.CalculateStopLossForCandle(lastCandle,marketBias);

            decimal lotSize = riskAmount / (stopLossPips * pipValue);

            return Math.Round(lotSize, 2);
        }

        public async Task<OrderResponse> PlaceOrderAsync(
            OrderRequest orderRequest,
            string bearerToken = ""
        )
        {
            if (string.IsNullOrEmpty(bearerToken))
            {
                bearerToken = AppConfig.Get("ApiSettings:OandaAPIKey");
            }

            var url = $"{AppConfig.Get("ApiSettings:BaseUrl")}/orders";

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            var content = new StringContent(
                JsonSerializer.Serialize(orderRequest, options),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<OrderResponse>(json, options)
                   ?? throw new InvalidOperationException("Failed to deserialize order response");
        }
    }

    // Define model classes for the order request and response
   

    
}