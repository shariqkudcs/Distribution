namespace Server.Resource
{
    /// <summary>
    /// Stores information about orders.
    /// Contains details such as order ID, customer ID, warehouse ID, carrier ID, order count, and entry date.
    /// </summary>
    public class OrderResource
    {
        /// <summary>
        /// Data Type: Integer
        /// Description: Unique identifier for the order.
        /// Purpose: Primary key for the OrderT table.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Data Type: Integer
        /// Description: Identifier of the district where the order is placed.
        /// Purpose: Foreign key referencing the DistrictResource.
        /// </summary>
        public int ODId { get; set; }

        /// <summary>
        /// Data Type: Integer
        /// Description: Identifier of the warehouse where the order is placed.
        /// Purpose: Foreign key referencing the WarehouseResource.
        /// </summary>
        public int OWId { get; set; }

        /// <summary>
        /// Data Type: Integer
        /// Description: Identifier of the customer placing the order.
        /// Purpose: Foreign key referencing the CustomerResource.
        /// </summary>
        public int? OCId { get; set; }
        public DateTime? OEntryD { get; set; }
        public int? OCarrierId { get; set; }
        public int? OOlCnt { get; set; }
        public int? OAllLocal { get; set; }
    }
}
