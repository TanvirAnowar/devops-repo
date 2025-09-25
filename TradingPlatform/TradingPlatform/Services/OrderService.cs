using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TradingPlatform.Models;
using TradingPlatform.Models.ApiModels;
using TradingPlatform.Services;
using TradingPlatform.Utils;

namespace TradingPlatform.Services
{
    public class OrderService : IOrderService
    {
        private readonly IIndicatorService _indicatorService;
        private readonly IOandaService _oandaService;
        private readonly ILogger<OrderService> _logger;
        private readonly ITradeStatusService _tradeStatusService;



        public OrderService(IIndicatorService indicatorService, IOandaService oandaService, ILogger<OrderService> logger, ITradeStatusService tradeStatusService)
        {
            _indicatorService = indicatorService;
            _oandaService = oandaService;
            _logger = logger;
            _tradeStatusService = tradeStatusService;
        }

        // Change the return type of ExecuteOrder from Task<int> to int and remove 'async' and 'await' usage
        public async Task<int> ExecuteOrder(IEnumerable<Candle> candles, IndicatorConfig config)
        {
            var brokerActiveTradeOrders = await _oandaService.GetActiveTradeStatusAsync("EUR_USD");
            var isActiveTrade = brokerActiveTradeOrders.Orders.Any();

            IEnumerable<IndicatorResult> results = null;
           
            if (isActiveTrade)
            {
                Console.WriteLine("There is an active trade. Exiting ExecuteOrder.");
                
                var lastTradeStatusId = brokerActiveTradeOrders.LastTransactionID;

                results = _indicatorService.CalculateIndicators(candles, config);

                var existingOrderInfo = await _oandaService.GetTradeByIdAsync(lastTradeStatusId);

                var existingOrder = existingOrderInfo.Orders.FirstOrDefault();

                var orderType = existingOrder.Type;


            }
            else
            {
                Console.WriteLine("No active trade found. Proceeding with order execution.");
         //       return false;
            }
            //  var activeTrade = await _tradeStatusService.GetAllAsync();

            //   return 0;

            //1. time check, if the time changed to new hour minuite 
            //2. active order check, if there is no active order
            //3. get last 2 indicator result
            
            if (results == null)
            {
                results = _indicatorService.CalculateIndicators(candles, config);
            }
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
                var analyzeTradeCandle = results.Last();

                var lastClosingPrice = analyzeTradeCandle.Close;

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };

                string json = JsonSerializer.Serialize(analyzeTradeCandle, options);

                logString += $"\nTrade Setup Info: {json}\n";             
            

              if (marketBias == Bias.Bullish)
                {
                  if (lastClosingPrice > analyzeTradeCandle.Open && lastClosingPrice > analyzeTradeCandle.KijunSen && analyzeTradeCandle.Adx >= 25 && analyzeTradeCandle.Rsi >= 55)
                    {


                        (decimal stopLossLevel, decimal lotSize) = await StopLossAndLotSizeCalculation(marketBias, analyzeTradeCandle);
                        
                        logString += string.Format($"Calculated SL Size: {stopLossLevel}\nCalculated Lot Size: {lotSize}\n");


                        var response = await PlaceOrder(lastClosingPrice, stopLossLevel, lotSize, marketBias);

                        // Console.WriteLine("Place Buy Order");
                        logString += string.Format($"Place Buy Order\n");
                    }
                    else
                    {
                        // Console.WriteLine("Buy Trade setup not formed");
                        logString += string.Format($"Buy Trade setup not formed\n");

                    }
                }
                else if (marketBias == Bias.Bearish)
                {
                    if (lastClosingPrice < analyzeTradeCandle.Open && lastClosingPrice < analyzeTradeCandle.KijunSen && analyzeTradeCandle.Adx >= 25 && analyzeTradeCandle.Rsi <= 45)
                    {
                        (decimal stopLossLevel, decimal lotSize) = await StopLossAndLotSizeCalculation(marketBias, analyzeTradeCandle);

                        logString += string.Format($"Calculated SL Size: {stopLossLevel}\nCalculated Lot Size: {lotSize}\n");

                        var response = await PlaceOrder(lastClosingPrice, stopLossLevel, lotSize, marketBias );

                        //    Console.WriteLine("Place Sell Order");
                        logString += string.Format($"Place Sell Order\n");
                    }
                    else
                    {
                        //   Console.WriteLine("Sell Trade setup not formed");
                        logString += string.Format($"Sell Trade setup not formed\n");

                    }
                }
                else
                {
                    //    Console.WriteLine("No Clear Market Bias");
                    logString += string.Format($"No Clear Market Bias\n");
                }

                logString += string.Format($"------ Log End ----------\n");

                _logger.LogInformation($" {logString}", DateTime.UtcNow);
                Console.WriteLine(logString);
            }



            var x = await _tradeStatusService.GetAllAsync();
            return 1;
        }

        private async Task<OrderResponse> PlaceOrder(decimal lastClosingPrice, decimal stopLossLevel, decimal lotSize, Bias bias)
        {
            lotSize = bias == Bias.Bearish ? (lotSize * -1) : lotSize;
          
            var response = await _oandaService.PlaceOrderAsync(new OrderRequest
            {
                order = new OrderRequest.Order
                {
                    Instrument = "EUR_USD",
                    Units = (int)(lotSize * 100000),
                    Type = "MARKET",
                    StopLossOnFill = new OrderRequest.StopLossOnFill
                    {
                        Price = Math.Round(stopLossLevel, 5).ToString() 
                    },
                    TakeProfitOnFill = new OrderRequest.TakeProfitOnFill
                    {
                        Price = bias == Bias.Bullish ?
                             Math.Round((lastClosingPrice + Math.Abs(lastClosingPrice - stopLossLevel) * 6),5).ToString() :
                             Math.Round((lastClosingPrice - Math.Abs(lastClosingPrice - stopLossLevel) * 6), 5).ToString()
                    },
                    TimeInForce = "FOK",
                    PositionFill = "DEFAULT"
                },
                // TrailingStopLossOnFill is not part of OrderRequest.Order, so omit or handle separately if needed
            });

            return response;
        }

        private async Task<(decimal stopLossLevel, decimal lotSize)> StopLossAndLotSizeCalculation(Bias marketBias, IndicatorResult analyzeTradeCandle)
        {
            var stopLossLevel = IndicatorCalculator.CalculateStopLossForCandle(analyzeTradeCandle, marketBias);
            var lotSize = await _oandaService.CalculateLotSizeAsync(
                       "EUR/USD",
                       decimal.Parse(AppConfig.Get("TradeSettings:Risk")), // You need to provide a decimal riskPercent value here, replace 1 with your actual risk percent
                       analyzeTradeCandle,
                       stopLossLevel,
                       marketBias
                       );
            return (stopLossLevel, lotSize);
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

            if (results.First().Close > results.First().KijunSen && results.Last().Adx > 25)
                return Bias.Bullish;

            if (results.First().Close < results.First().KijunSen && results.Last().Adx > 25)
                return Bias.Bearish;

            return Bias.Neutral;
        }

    }
}
