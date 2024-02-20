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
    public class OrdersController : ControllerBase
    {
        private readonly DistributionContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<InventoryController> _logger;

        public OrdersController(DistributionContext context, IMapper mapper, ILogger<InventoryController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            try
            {
                _logger.LogInformation("Retrieving all orders.");

                var orders = await _context.Orders.ToListAsync();

                _logger.LogInformation($"Number of orders retrieved: {orders.Count}");

                _logger.LogInformation($"Orders details: {JsonConvert.SerializeObject(orders)}");

                return orders;
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error occurred while retrieving orders.{0}", ex);
                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResource>> GetOrder(int id)
        {
            try
            {
                _logger.LogInformation($"Retrieving order with ID {id}");

                var orderEf = await _context.Orders.FindAsync(id);

                if (orderEf == null)
                {
                    _logger.LogWarning($"Order with ID {id} not found.");
                    return NotFound();
                }

                var orderResource = _mapper.Map<Order, OrderResource>(orderEf);
                _logger.LogInformation($"Order details: {JsonConvert.SerializeObject(orderResource)}");

                return orderResource;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error occurred while retrieving order with ID {id}.", ex);
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult<OrderResource>> PostOrder(OrderResource order)
        {
            try
            {
                _logger.LogInformation("Creating a new order.");

                _logger.LogInformation($"Order details: {JsonConvert.SerializeObject(order)}");

                _context.Orders.Add(_mapper.Map<OrderResource, Order>(order));
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Order with ID {order.Id} created successfully.");

                return CreatedAtAction("GetOrder", new { id = order.Id }, order);
            }
            catch (DbUpdateException)
            {
                if (OrderExists(order.Id))
                {
                    _logger.LogError($"Conflict: Order with ID {order.Id} already exists.");
                    return Conflict();
                }
                else
                {
                    _logger.LogError("Unexpected error occurred during order creation.");
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error occurred during order creation.{0}", ex);
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting order with ID {id}");

                var order = await _context.Orders.FindAsync(id);

                if (order == null)
                {
                    _logger.LogWarning($"Order with ID {id} not found for deletion.");
                    return NotFound();
                }

                _logger.LogInformation($"Order details before deletion: {JsonConvert.SerializeObject(order)}");

                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Order with ID {id} deleted successfully.");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error occurred during order deletion with ID {id}.", ex);
                throw;
            }
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
