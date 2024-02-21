namespace Server.Resource
{
    public class StockResource
    {
        public int Id { get; set; }
        public int SIId { get; set; }
        public int SWId { get; set; }
        public int? SQuantity { get; set; }
        public string? SDist01 { get; set; }
        public int? SYtd { get; set; }
        public int? SOrderCnt { get; set; }
        public int? SRemoteCnt { get; set; }
        public string? SData { get; set; }
    }
}
