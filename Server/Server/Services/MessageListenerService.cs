using Microsoft.AspNetCore.SignalR;
using Server.HubConfig;
using Server.Models;
using System.Text.Json;

namespace Server.Services
{
    public class MessageListenerService : IMessageListener, IDisposable
    {
        private Timer? _timer;
        private AutoResetEvent? _autoResetEvent;
        private readonly IHubContext<InventoryHub> _hub;
        private readonly IRabbitMQHandler _rabbitMQHandler;
        public MessageListenerService(IRabbitMQHandler handler, IHubContext<InventoryHub> hub)
        {
            _rabbitMQHandler = handler;
            _hub = hub;
            _autoResetEvent = new AutoResetEvent(false);
            _timer = new Timer((object? stateInfo) =>
            {
                var rcv = _rabbitMQHandler.GetItemFromInventory();
                if (!string.IsNullOrEmpty(rcv))
                {
                    _hub.Clients.All.SendAsync("inventorydata", JsonSerializer.Deserialize<Product>(rcv));
                }
            }, _autoResetEvent, 1000, 2000);
        }

        public void Dispose()
        {
            if (_timer != null)
            { 
                _timer.Dispose(); 
            }
        }
    }
}
