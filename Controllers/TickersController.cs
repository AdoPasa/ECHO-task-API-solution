using Microsoft.AspNetCore.Mvc;
using YahooFinanceAgregator.Models;
using YahooFinanceAgregator.Services;

namespace YahooFinanceAgregator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TickersController : ControllerBase
    {
        private TickerService _tickerService;

        public TickersController(TickerService tickerService)
        {
            _tickerService = tickerService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]GetTickersRequest request)
        {
            if(request.Symbols.Count == 0)
                return BadRequest(string.Empty);

            if (request.Date.Date > DateTime.Now.Date)
                return BadRequest(string.Empty);

            return Ok(await _tickerService.GetTickerData(request.Symbols, request.Date.Date));
        }

        [HttpGet("{symbol}")]
        public async Task<IActionResult> GetTickerBySymbol(string? symbol, [FromQuery] GetTickersRequest request)
        {
            if (symbol == null || string.IsNullOrEmpty(symbol.Trim()))
                return BadRequest(string.Empty);

            if (request.Date.Date > DateTime.Now.Date)
                return BadRequest(string.Empty);

            return Ok(await _tickerService.GetTickerData(symbol, request.Date.Date));
        }
    }
}