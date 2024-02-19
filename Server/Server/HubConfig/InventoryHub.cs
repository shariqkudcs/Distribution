using Microsoft.AspNetCore.SignalR;
using Server.Models;

namespace Server.HubConfig
{
    public class InventoryHub : Hub
    {
        public async Task InventoryData(List<Product> data)
        {
            await Clients.All.SendAsync("inventorydata", data);
        }
    }
}
