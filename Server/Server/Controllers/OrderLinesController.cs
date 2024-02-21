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
    public class OrderLinesController : ControllerBase
    {
        private readonly DistributionContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderLinesController> _logger;

        public OrderLinesController(DistributionContext context, IMapper mapper, ILogger<OrderLinesController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderLineResource>> GetOrderLine(int id)
        {
            try
            {
                _logger.LogInformation($"Received GET request to retrieve order line with ID {id}.");

                var orderLineEf = await _context.OrderLines.FindAsync(id);

                if (orderLineEf == null)
                {
                    _logger.LogInformation($"Order line with ID {id} not found.");
                    return NotFound();
                }
                var orderLine = _mapper.Map<OrderLine, OrderLineResource>(orderLineEf);
                _logger.LogInformation($"Returning order line with ID {id}.");

                return orderLine;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while processing GET request for order line with ID {id}: {ex}");

                throw;
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderLine(int id, OrderLineResource orderLine)
        {
            _logger.LogInformation($"Received PUT request to update order line with ID {id}. Request details: {JsonConvert.SerializeObject(orderLine)}");

            if (id != orderLine.Id)
            {
                _logger.LogWarning($"Bad request. ID in the URL ({id}) does not match the ID in the request body ({orderLine.Id}).");
                return BadRequest();
            }

            try
            {
                _context.Entry(_mapper.Map<OrderLineResource, OrderLine>(orderLine)).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Order line with ID {id} updated successfully.");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderLineExists(id))
                    {
                        _logger.LogInformation($"Order line with ID {id} not found.");
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError($"Concurrency exception occurred while updating order line with ID {id}.");
                        throw;
                    }
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while processing PUT request for order line with ID {id}: {ex}");

                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult<OrderLineResource>> PostOrderLine(OrderLineResource orderLine)
        {
            _logger.LogInformation($"Received POST request to add order line. Request details: {JsonConvert.SerializeObject(orderLine)}");

            var saveEntity = _mapper.Map<OrderLineResource, OrderLine>(orderLine);
            _context.OrderLines.Add(saveEntity);

            try
            {
                _logger.LogInformation("Creating a new order line.");

                await _context.SaveChangesAsync();
                orderLine.Id = saveEntity.Id;

                _logger.LogInformation($"Order line with ID {orderLine.Id} added successfully.");

            }
            catch (DbUpdateException ex)
            {
                if (OrderLineExists(orderLine.Id))
                {
                    _logger.LogError($"Conflict: Order Line with ID {orderLine.Id} already exists.");
                    return Conflict();
                }
                else
                {
                    _logger.LogError($"Unexpected error occurred during order line creation : {ex}");
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error occurred during order line creation : {ex}");
                throw;
            }

            return CreatedAtAction("GetOrderLine", new { id = saveEntity.Id }, orderLine);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderLine(int id)
        {
            try
            {
                _logger.LogInformation($"Received DELETE request to delete order line with ID {id}.");

                var orderLine = await _context.OrderLines.FindAsync(id);

                if (orderLine == null)
                {
                    _logger.LogInformation($"Order line with ID {id} not found.");
                    return NotFound();
                }

                _context.OrderLines.Remove(orderLine);

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Order line with ID {id} deleted successfully.");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while processing DELETE request for order line with ID {id}: {ex}");

                throw;
            }
        }

        private bool OrderLineExists(int id)
        {
            return _context.OrderLines.Any(e => e.Id == id);
        }
    }
}
