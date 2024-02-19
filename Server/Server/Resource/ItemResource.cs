namespace Server.Resource
{
    /// <summary>
    /// Contains information about items that can be ordered.
    /// Includes item name, price, and a reference to an image.
    /// </summary>
    public class ItemResource
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public double? Price { get; set; }
        public string? Data { get; set; }
    }
}
