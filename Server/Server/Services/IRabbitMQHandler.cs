namespace Server.Services
{
    public interface IRabbitMQHandler
    {
        Task<string?> GetItemFromInventoryAsync();
        Task AddProductToInventoryAsync(string product);
    }
}
