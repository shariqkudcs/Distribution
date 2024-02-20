using Microsoft.AspNetCore.SignalR;
using Server.HubConfig;
using Server.Models;
using System.Text.Json;

namespace Server.Services
{
    public class MessageListenerService : IMessageListener, IDisposable
    {
        private Timer? _timer;
        private readonly IHubContext<InventoryHub> _hub;
        private readonly IRabbitMQHandler _rabbitMQHandler;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly ILogger<RabbitMQHandler> _logger;

        public MessageListenerService(IRabbitMQHandler handler, IHubContext<InventoryHub> hub, ILogger<RabbitMQHandler> logger)
        {
            _rabbitMQHandler = handler ?? throw new ArgumentNullException(nameof(handler));
            _hub = hub ?? throw new ArgumentNullException(nameof(hub));

            _cancellationTokenSource = new CancellationTokenSource();

            _timer = new Timer(async (object? stateInfo) =>
            {
                try
                {
                    var rcv = _rabbitMQHandler.GetItemFromInventory();
                    if (!string.IsNullOrEmpty(rcv))
                    {
                        await _hub.Clients.All.SendAsync("inventorydata", JsonSerializer.Deserialize<Product>(rcv));
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError($"Error in timer callback: {ex}");
                }
            }, null, 1000, 2000);
            _logger = logger;
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _timer?.Dispose();
        }
    }
}
