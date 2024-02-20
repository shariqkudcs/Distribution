using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Server.HubConfig;
using Server.Models;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/inventory")]
    public class InventoryController : ControllerBase
    {
        private readonly IHubContext<InventoryHub> _hub;
        private readonly ILogger<InventoryController> _logger;
        private readonly IRabbitMQHandler _mQHandler;
        private readonly IMessageListener _messageListener;

        public InventoryController(IHubContext<InventoryHub> hub, IRabbitMQHandler mQHandler, IMessageListener listener, ILogger<InventoryController> logger)
        {
            _hub = hub;
            _mQHandler = mQHandler;
            _messageListener = listener;
            _logger = logger;

        }

        /// <summary>
        /// Command Handler
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody] Product product)
        {
            try
            {
                _logger.LogInformation($"Received POST request to add product to inventory. Request details: {JsonConvert.SerializeObject(product)}");

                _logger.LogInformation($"Adding product to inventory: {JsonConvert.SerializeObject(product)}");

                _mQHandler.AddProductToInventory(JsonConvert.SerializeObject(product));

                _logger.LogInformation($"Product queued to add into inventory successfully.");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while processing POST request: {ex}");

                throw;
            }
        }


        /// <summary>
        /// Query Handler
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<IActionResult> Get()
        {
            try
            {
                _logger.LogInformation("Received GET request to retrieve products.");

                var products = new Product[] { new Product { Description = "Product One", Name = "One" } };

                _logger.LogInformation($"Returning products: {JsonConvert.SerializeObject(products)}");

                return Task.FromResult<IActionResult>(Ok(products));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while processing GET request: {ex}");

                throw;
            }
        }
    }
}
