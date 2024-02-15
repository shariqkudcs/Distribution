using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Server.HubConfig;
using Server.Models;
using Server.Services;
using System.Text.Json;

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
            _mQHandler.AddProductToInventory(JsonSerializer.Serialize(product));
            return Ok();
        }


        /// <summary>
        /// Query Handler
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            //read data from entity framework
            return Ok(new Product[] { new Product { Description = "Product One", Name = "One" } });
        }
    }
}
