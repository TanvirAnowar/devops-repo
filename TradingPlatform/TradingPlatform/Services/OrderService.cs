using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TradingPlatform.Models;
using TradingPlatform.Services;
using TradingPlatform.Utils;

namespace TradingPlatform.Services
{
    public class OrderService : IOrderService
    {
        private readonly IIndicatorService _indicatorService;
        private readonly IOandaService _oandaService;
        private readonly ILogger<OrderService> _logger;



        public OrderService(IIndicatorService indicatorService, IOandaService oandaService, ILogger<OrderService> logger)
        {
            _indicatorService = indicatorService;
            _oandaService = oandaService;
            _logger = logger;
        }

        // Change the return type of ExecuteOrder from Task<int> to int and remove 'async' and 'await' usage
        public async Task<int> ExecuteOrder(IEnumerable<Candle> candles, IndicatorConfig config)
        {
            //1. time check, if the time changed to new hour minuite 
            //2. active order check, if there is no active order
            //3. get last 2 indicator result
            var results = _indicatorService.CalculateIndicators(candles, config);
            //4. should close order check
            //5. should open order check
            //6. Get bias from higher timeframe
            // NOTE: Synchronous version, so cannot await GetBiasStatusAsync
            // If async is required, interface and all usages must be updated to async

            var logString = string.Format($"---- Log Start {DateTime.UtcNow} ---------\n");
            
            var marketBias = await GetBiasStatusAsync(config); 

            logString += string.Format($"Market Bias: {marketBias}\n");

            if (results.Any())
            {
                var analyzeTradeCandle = results.First();

                var lastClosingPrice = analyzeTradeCandle.Close;

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };

                string json = JsonSerializer.Serialize(analyzeTradeCandle, options);

                logString += $"\nTrade Setup Info: {json}\n";

                Console.WriteLine(json);

                if (marketBias == Bias.Bullish)
                {
                    if (lastClosingPrice > analyzeTradeCandle.TenkanSen && analyzeTradeCandle.Adx >= 20 && analyzeTradeCandle.Rsi >= 55)
                    {

                        Console.WriteLine("Place Buy Order");
                        logString += string.Format($"Place Buy Order\n");
                    }
                }
                else if (marketBias == Bias.Bearish)
                {
                    if (lastClosingPrice < analyzeTradeCandle.TenkanSen && analyzeTradeCandle.Adx >= 20 && analyzeTradeCandle.Rsi <= 45)
                    {
                        Console.WriteLine("Place Sell Order");
                        logString += string.Format($"Place Sell Order\n");
                    }
                }
                else
                {
                    Console.WriteLine("No Clear Market Bias");
                    logString += string.Format($"No Clear Market Bias\n");
                }

                logString += string.Format($"------ Log End ----------\n");

                _logger.LogInformation($" {logString}", DateTime.UtcNow);
                Console.WriteLine(json);
            }




            return 1;
        }

        private enum Bias
        {
            Bullish,
            Bearish,
            Neutral
        }


        // Keep the async version for future use if interface is updated
        private async Task<Bias> GetBiasStatusAsync(IndicatorConfig config, string higherTimeframe = "H1")
        {
            var candles = await _oandaService.GetCandlesAsync(
                "EUR_USD",
                higherTimeframe,
                150,
                AppConfig.Get("ApiSettings:OandaAPIKey")
                );

            var results = _indicatorService.CalculateIndicators(candles, config);

            if (results.First().Close > results.First().TenkanSen && results.First().Adx > 20)
                return Bias.Bullish;

            if (results.First().Close < results.First().TenkanSen && results.First().Adx > 20)
                return Bias.Bearish;

            return Bias.Neutral;
        }

    }
}
