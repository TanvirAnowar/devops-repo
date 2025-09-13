using Microsoft.AspNetCore.Mvc;
using TradingPlatform.Models;
using TradingPlatform.Services;

namespace TradingPlatform.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IndicatorsController : ControllerBase
    {
        private readonly IIndicatorService _indicatorService;
        private readonly IOandaService _oandaService;

        public IndicatorsController(IIndicatorService indicatorService, IOandaService oandaService)
        {
            _indicatorService = indicatorService;
            _oandaService = oandaService;
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

        [HttpPost("taketrake")]
        public async Task<IActionResult> TakeTrake([FromBody] IndicatorRequest request)
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