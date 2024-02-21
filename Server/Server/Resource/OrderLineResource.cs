using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Resource
{
    public class OrderLineResource
    {
        public int Id { get; set; }
        public int OlOId { get; set; }
        public int OlDId { get; set; }
        public int OlWId { get; set; }
        public int OlId { get; set; }
        public int? OlIId { get; set; }
        public int? OlSupplyWId { get; set; }
        public DateTime? DeliveryD { get; set; }
        public int? Quantity { get; set; }
        public double? Amount { get; set; }
        public string? DistInfo { get; set; }
    }
}
