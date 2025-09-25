using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TradingPlatform.Models.ApiModels;
using TradingPlatform.Services;

namespace TradingPlatform.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IndicatorsController : ControllerBase
    {
        private readonly IIndicatorService _indicatorService;
        private readonly IOandaService _oandaService;
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderService> _logger;


        public IndicatorsController(IIndicatorService indicatorService, IOandaService oandaService, IOrderService orderService, ILogger<OrderService> logger) 
        {
            _indicatorService = indicatorService;
            _oandaService = oandaService;
            _orderService = orderService; 
            _logger = logger;
        }

        [HttpPost("calculate")]
        public async Task<IActionResult> Calculate([FromBody] IndicatorRequest request)
        {
            List<Candle> candles;

            if (request.Candles != null && request.Candles.Any())
            {
                candles = request.Candles;
            }
            else
            {
                candles = await _oandaService.GetCandlesAsync(
                    request.Instrument ?? "EUR_USD",
                    request.Granularity ?? "H1",
                    request.Count ?? 150,
                    request.BearerToken ?? "be08a1aeba013ec91802c210aaff8488-12ee356da5a3c7a5db924d2a5b5e3396"
                );
            }

            var config = request.Config ?? new IndicatorConfig();
            var results = _indicatorService.CalculateIndicators(candles, config);

            return Ok(results);
        }

        [HttpPost("executeorder")]
        public async Task<IActionResult> ExecuteOrade([FromBody] IndicatorRequest request)
        {
            List<Candle> candles;

            if (request.Candles != null && request.Candles.Any())
            {
                candles = request.Candles;
            }
            else
            {
                candles = await _oandaService.GetCandlesAsync(
                    request.Instrument ?? "EUR_USD",
                    request.Granularity ?? "M5",
                    request.Count ?? 150,
                    request.BearerToken ?? "be08a1aeba013ec91802c210aaff8488-12ee356da5a3c7a5db924d2a5b5e3396"
                );
            }

            var config = request.Config ?? new IndicatorConfig();
            //  var results = _indicatorService.CalculateIndicators(candles, config);
            var result2 = await _orderService.ExecuteOrder(candles, config);

            return Ok(result2);
        }
    }

    public class IndicatorRequest
    {
        public List<Candle>? Candles { get; set; }
        public IndicatorConfig? Config { get; set; }
        public string? Instrument { get; set; }
        public string? Granularity { get; set; }
        public int? Count { get; set; }
        public string? BearerToken { get; set; }
    }
}