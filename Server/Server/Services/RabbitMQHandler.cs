using RabbitMQ.Client;
using System.Text;

namespace Server.Services
{
    public class RabbitMQHandler : IRabbitMQHandler, IDisposable
    {
        private readonly IConnection _rabbitMqConnection;
        private readonly IModel _rabbitMqChannel;

        public RabbitMQHandler()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            _rabbitMqConnection = connectionFactory.CreateConnection();
            _rabbitMqChannel = _rabbitMqConnection.CreateModel();
        }

        public string? GetItemFromInventory()
        {
            var ea = _rabbitMqChannel.BasicGet(queue: "myqueue", autoAck: true);
            if (ea == null)
            {
                return null;
            }
            return Encoding.UTF8.GetString(ea.Body.ToArray());
        }

        public void AddProductToInventory(string product)
        {
            var properties = _rabbitMqChannel.CreateBasicProperties();
            properties.Persistent = true;
            _rabbitMqChannel.BasicPublish("exchange", "", properties, Encoding.UTF8.GetBytes(product));
        }


        public void Dispose()
        {
            _rabbitMqChannel.Close();
            _rabbitMqConnection.Close();
        }
    }
}
