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
            decimal stopLossLevel,
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

            decimal riskAmount = balance * (riskPercent / 100m);

            // pip size: JPY pairs use 0.01, others use 0.0001
            decimal pipSize = pair.EndsWith("JPY", StringComparison.OrdinalIgnoreCase) ? 0.01m : 0.0001m;

            // crude pip value example for USD-quoted pairs / XAU
            decimal pipValue = pair.StartsWith("XAU", StringComparison.OrdinalIgnoreCase) ? 1m : 10m;

            // convert price difference to pips
            var stopLossPips = Math.Abs(stopLossLevel - lastCandle.Close) / pipSize;

            if (stopLossPips <= 0) throw new InvalidOperationException("Stop loss must differ from entry price.");

            decimal lotSize = riskAmount / (stopLossPips * pipValue);

            // round if you want
            lotSize = Math.Round(lotSize, 2);

            return lotSize;
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

            var url = $"{AppConfig.Get("ApiSettings:BaseUrl")}orders";

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            /*
            var content = new StringContent(
                JsonSerializer.Serialize(orderRequest, options),
                Encoding.UTF8,
                "application/json"
            );
            */
            //   var response = await _httpClient.PostAsync(url, content);
            
                  /*      var formattedRequest = new
                        {
                            order = new
                            {
                                type = "MARKET",
                                instrument = "EUR_USD",
                                units = -8445000,
                                timeInForce = "FOK",
                                positionFill = "DEFAULT",
                                stopLossOnFill = new { price = "1.18408" },  //stopLossOnFill = { price = "1.18408" }
                                takeProfitOnFill = new { price = "1.17947" }  //{ price = "1.17947" }
                            }
                        };
              */
            var formattedRequest = new
            {
                order = new
                {
                    type = orderRequest.order.Type,
                    instrument = orderRequest.order.Instrument,
                    units = orderRequest.order.Units, // API expects string
                    timeInForce = orderRequest.order.TimeInForce,
                    positionFill = orderRequest.order.PositionFill,
                    stopLossOnFill = orderRequest.order.StopLossOnFill != null
                            ? new { price = orderRequest.order.StopLossOnFill.Price }
                            : null,
                    takeProfitOnFill = orderRequest.order.TakeProfitOnFill != null
                            ? new { price = orderRequest.order.TakeProfitOnFill.Price }
                            : null
                }
            };

            var response = await _httpClient.PostAsJsonAsync(url, formattedRequest);
            response.EnsureSuccessStatusCode();

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<OrderResponse>(json, options)
                   ?? throw new InvalidOperationException("Failed to deserialize order response");
        }
    }

    // Define model classes for the order request and response
   

    
}