using System.Net.Http.Headers;
using System.Text.Json;
using TradingPlatform.Models;

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
            string bearerToken = "be08a1aeba013ec91802c210aaff8488-12ee356da5a3c7a5db924d2a5b5e3396")
        {
            var url = $"https://api-fxpractice.oanda.com/v3/accounts/101-004-24777125-001/instruments/{instrument}/candles?count={count}&granularity={granularity}";

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
    }
}