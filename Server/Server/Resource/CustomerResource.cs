namespace Server.Resource
{
    /// <summary>
    /// Represents information about customers.
    /// Columns include customer details such as name, address, phone, credit information, and transaction history.
    /// </summary>
    public class CustomerResource
    {
        public int Id { get; set; }
        public int CDId { get; set; }//TODO: Unchecked in schema relationship
        public int CWId { get; set; }//TODO: Unchecked in schema relationship
        public string? First { get; set; }
        public string? Middle { get; set; }
        public string? Last { get; set; }
        public string? Street1 { get; set; }
        public string? Street2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public string? Phone { get; set; }
        public DateTime? Since { get; set; }
        public string? Credit { get; set; }
        public double? CreditLim { get; set; }
        public double? Discount { get; set; }
        public double? Balance { get; set; }
        public double? YtdPayment { get; set; }
        public int? PaymentCnt { get; set; }
        public int? DeliveryCnt { get; set; }
        public string? Data { get; set; }
    }
}
