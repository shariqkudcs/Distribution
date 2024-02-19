using RabbitMQ.Client;
using System.Text;

namespace Server.Services
{
    public class RabbitMQHandler : IRabbitMQHandler, IDisposable
    {
        private readonly IConnection _rabbitMqConnection;
        private readonly IModel _rabbitMqChannel;
        private readonly ILogger<RabbitMQHandler> _logger;

        public RabbitMQHandler(ILogger<RabbitMQHandler> logger)
        {
            ConnectionFactory connectionFactory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            _rabbitMqConnection = connectionFactory.CreateConnection();
            _rabbitMqChannel = _rabbitMqConnection.CreateModel();
            _logger = logger;
        }

        public async Task<string?> GetItemFromInventoryAsync()
        {
            try
            {
                var ea = _rabbitMqChannel.BasicGet(queue: "myqueue", autoAck: true);
                return ea != null ? Encoding.UTF8.GetString(ea.Body.ToArray()) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetItemFromInventory: {ex}");
                return null;
            }
        }

        public async Task AddProductToInventoryAsync(string product)
        {
            try
            {
                var properties = _rabbitMqChannel.CreateBasicProperties();
                properties.Persistent = true;
                _rabbitMqChannel.BasicPublish("exchange", "", properties, Encoding.UTF8.GetBytes(product));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in AddProductToInventory: {ex}");
            }
        }

        public void Dispose()
        {
            _rabbitMqChannel?.Close();
            _rabbitMqConnection?.Close();
        }
    }
}
