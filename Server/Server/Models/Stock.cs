using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Server.Models
{

    [PrimaryKey("SIId", "SWId")]
    [Table("Stock")]
    public partial class Stock
    {
        [Key]
        [Column("S_I_ID")]
        public int SIId { get; set; }

        [Key]
        [Column("S_W_ID")]
        public int SWId { get; set; }

        [Column("S_QUANTITY")]
        public int? SQuantity { get; set; }

        [Column("S_DIST_01")]
        [StringLength(24)]
        public string? SDist01 { get; set; }

        [Column("S_YTD")]
        public int? SYtd { get; set; }

        [Column("S_ORDER_CNT")]
        public int? SOrderCnt { get; set; }

        [Column("S_REMOTE_CNT")]
        public int? SRemoteCnt { get; set; }

        [Column("S_DATA")]
        public string? SData { get; set; }
    }
}
