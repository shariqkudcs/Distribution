namespace Server.Services
{
    public interface IRabbitMQHandler
    {
        string? GetItemFromInventory();
        void AddProductToInventory(string product);
    }
}
