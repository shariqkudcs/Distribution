namespace Server.Response
{
    /// <summary>
    /// Stores information about warehouses.
    /// Includes warehouse name, address, tax rate, and year-to-date financial information.
    /// </summary>
    public class WarehouseResource
    {
        /// <summary>
        /// Data Type: Integer
        /// Description: Unique identifier for the warehouse.
        /// Purpose: Primary key for the Warehouse table.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Data Type: String of Length 100
        /// Description: Name of the warehouse.
        /// Purpose: Stores the name of the warehouse for identification.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Data Type: String of Lenght 20
        /// Description: First line of the street address of the warehouse.
        /// Purpose: Captures the primary street address.
        /// </summary>
        public string? Street1 { get; set; }

        /// <summary>
        /// Data Type: String of Length 20
        /// Description: Second line of the street address of the warehouse.
        /// Purpose: Additional information for the street address.
        /// </summary>
        public string? Street2 { get; set; }

        /// <summary>
        /// Data Type: String of Length 20
        /// Description: City where the warehouse is located.
        /// Purpose: Captures the city information.
        /// </summary>
        public string? City { get; set; }

        /// <summary>
        /// Data Type: String of Length 2
        /// Description: State code where the warehouse is located.
        /// Purpose: Represents the state of the warehouse.
        /// </summary>
        public string? State { get; set; }

        /// <summary>
        /// Data Type: String of Length 9
        /// Description: ZIP code of the warehouse location.
        /// Purpose: Stores the ZIP code for addressing.
        /// </summary>
        public string? Zip { get; set; }

        /// <summary>
        /// Data Type: Double Nullable
        /// Description: Tax rate applicable to the warehouse.
        /// Purpose: Captures the tax rate for financial calculations.
        /// </summary>
        public double? WTax { get; set; }

        /// <summary>
        /// Data Type: Double Nullable
        /// Description: Year-to-date financial information for the warehouse.
        /// Purpose: Tracks the financial performance of the warehouse.
        /// </summary>
        public double? Ytd { get; set; }

    }
}
