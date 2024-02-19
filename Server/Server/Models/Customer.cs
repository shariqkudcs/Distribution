using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Server.Models
{

    [PrimaryKey("Id", "CDId", "CWId")]
    [Table("Customer")]
    public partial class Customer
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Key]
        [Column("C_D_ID")]
        public int CDId { get; set; }

        [Key]
        [Column("C_W_ID")]
        public int CWId { get; set; }

        [Column("FIRST")]
        [StringLength(100)]
        public string? First { get; set; }

        [Column("MIDDLE")]
        [StringLength(2)]
        public string? Middle { get; set; }

        [Column("LAST")]
        [StringLength(100)]
        public string? Last { get; set; }

        [Column("STREET_1")]
        [StringLength(100)]
        public string? Street1 { get; set; }

        [Column("STREET_2")]
        [StringLength(20)]
        public string? Street2 { get; set; }

        [Column("CITY")]
        [StringLength(100)]
        public string? City { get; set; }

        [Column("STATE")]
        [StringLength(2)]
        public string? State { get; set; }

        [Column("ZIP")]
        [StringLength(9)]
        public string? Zip { get; set; }

        [Column("PHONE")]
        [StringLength(16)]
        public string? Phone { get; set; }

        [Column("SINCE", TypeName = "datetime")]
        public DateTime? Since { get; set; }

        [Column("CREDIT")]
        [StringLength(2)]
        public string? Credit { get; set; }

        [Column("CREDIT_LIM")]
        public double? CreditLim { get; set; }

        [Column("DISCOUNT")]
        public double? Discount { get; set; }

        [Column("BALANCE")]
        public double? Balance { get; set; }

        [Column("YTD_PAYMENT")]
        public double? YtdPayment { get; set; }

        [Column("PAYMENT_CNT")]
        public int? PaymentCnt { get; set; }

        [Column("DELIVERY_CNT")]
        public int? DeliveryCnt { get; set; }

        [Column("DATA")]
        public string? Data { get; set; }
    }
}