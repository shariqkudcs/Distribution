namespace Server.Resource
{
    /// <summary>
    /// Represents information about districts within a warehouse.
    /// Contains details like district name, address, tax rate, and year-to-date financial information.
    /// </summary>
    public class DistrictResource
    {
        public int Id { get; set; }
        public int DWId { get; set; }
        public string? Name { get; set; }
        public string? Street1 { get; set; }
        public string? Street2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public double? Tax { get; set; }
        public double? Ytd { get; set; }
        public int? NextOId { get; set; }
    }
}
