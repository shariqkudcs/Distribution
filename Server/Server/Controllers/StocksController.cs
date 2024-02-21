using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Server.Data;
using Server.Models;
using Server.Resource;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly DistributionContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<StocksController> _logger;

        public StocksController(DistributionContext context, IMapper mapper, ILogger<StocksController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Stock>>> GetStocks()
        {
            try
            {
                _logger.LogInformation("Received GET request to retrieve stocks.");

                var stocks = await _context.Stocks.ToListAsync();

                _logger.LogInformation($"Returning {stocks.Count} stocks.");

                return Ok(stocks);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while processing GET request for stocks: {ex}");

                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StockResource>> GetStock(int id)
        {
            try
            {
                _logger.LogInformation($"Received GET request to retrieve stock with ID {id}.");

                var stockEf = await _context.Stocks.FindAsync(id);

                if (stockEf == null)
                {
                    _logger.LogInformation($"Stock with ID {id} not found.");
                    return NotFound();
                }

                var stock = _mapper.Map<Stock, StockResource>(stockEf);

                _logger.LogInformation($"Returning stock with ID {id}.");

                return stock;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while processing GET request for stock with ID {id}: {ex}");

                throw;
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutStock(int id, StockResource stock)
        {
            _logger.LogInformation($"Received PUT request to update stock with ID {id}. Request details: {JsonConvert.SerializeObject(stock)}");

            if (id != stock.Id)
            {
                _logger.LogWarning($"Bad request. ID in the URL ({id}) does not match the ID in the request body ({stock.Id}).");
                return BadRequest();
            }

            try
            {

                _context.Entry(_mapper.Map<StockResource, Stock>(stock)).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Stock with ID {id} updated successfully.");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockExists(id))
                    {
                        _logger.LogInformation($"Stock with ID {id} not found.");
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError($"Concurrency exception occurred while updating stock with ID {id}.");
                        throw;
                    }
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while processing PUT request for stock with ID {id}: {ex}");

                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult<StockResource>> PostStock(StockResource stock)
        {
            _logger.LogInformation($"Received POST request to add stock. Request details: {JsonConvert.SerializeObject(stock)}");

            var saveEntity = _mapper.Map<StockResource, Stock>(stock);
            _context.Stocks.Add(saveEntity);

            try
            {
                _logger.LogInformation("Adding a new Stock.");

                await _context.SaveChangesAsync();
                stock.Id = saveEntity.Id;
                _logger.LogInformation($"Stock with ID {stock.Id} added successfully.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Error while updating the database: {ex.Message}");

                if (StockExists(stock.Id))
                {
                    _logger.LogError($"Conflict: Stock with ID {stock.Id} already exists.");
                    return Conflict();
                }
                else
                {
                    _logger.LogError($"Unexpected error occurred during warehouse creation : {ex}");
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error occurred during stock creation : {ex}");
                throw;
            }

            return CreatedAtAction("GetStock", new { id = saveEntity.Id }, stock);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStock(int id)
        {
            try
            {
                _logger.LogInformation($"Received DELETE request to delete stock with ID {id}.");

                var stock = await _context.Stocks.FindAsync(id);

                if (stock == null)
                {
                    _logger.LogInformation($"Stock with ID {id} not found.");
                    return NotFound();
                }

                _context.Stocks.Remove(stock);

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Stock with ID {id} deleted successfully.");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while processing DELETE request for stock with ID {id}: {ex}");

                throw;
            }
        }

        private bool StockExists(int id)
        {
            return _context.Stocks.Any(e => e.Id == id);
        }
    }
}
