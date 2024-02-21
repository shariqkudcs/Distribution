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

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, OrderResource order)
        {
            _logger.LogInformation($"Received PUT request to update order with ID {id}. Request details: {JsonConvert.SerializeObject(order)}");

            if (id != order.Id)
            {
                _logger.LogWarning($"Bad request. ID in the URL ({id}) does not match the ID in the request body ({order.Id}).");
                return BadRequest();
            }

            try
            {
                _context.Entry(_mapper.Map<OrderResource, Order>(order)).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Order with ID {id} updated successfully.");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!OrderExists(id))
                    {
                        _logger.LogInformation($"Order line ID {id} not found.");
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError($"Concurrency exception occurred while updating order with ID {id} : {ex}");
                        throw;
                    }
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while processing PUT request for order with ID {id}: {ex}");

                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult<OrderResource>> PostOrder(OrderResource order)
        {
            _logger.LogInformation($"Received POST request to add order. Request details: {JsonConvert.SerializeObject(order)}");

            var saveEntity = _mapper.Map<OrderResource, Order>(order);
            _context.Orders.Add(saveEntity);

            try
            {
                _logger.LogInformation("Creating a new order.");
                await _context.SaveChangesAsync();
                order.Id = saveEntity.Id;
                _logger.LogInformation($"Order with ID {order.Id} created successfully.");

            }
            catch (DbUpdateException ex)
            {
                if (OrderExists(order.Id))
                {
                    _logger.LogError($"Conflict: Order with ID {order.Id} already exists.");
                    return Conflict();
                }
                else
                {
                    _logger.LogError($"Unexpected error occurred during order creation : {ex}");
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error occurred during order creation : {ex}");
                throw;
            }

            return CreatedAtAction("GetOrder", new { id = saveEntity.Id }, order);
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
