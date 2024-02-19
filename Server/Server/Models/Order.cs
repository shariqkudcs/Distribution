using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Server.Models
{
    [PrimaryKey("Id", "ODId", "OWId")]
    public partial class Order
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Key]
        [Column("O_D_ID")]
        public int ODId { get; set; }

        [Key]
        [Column("O_W_ID")]
        public int OWId { get; set; }

        [Column("O_C_ID")]
        public int? OCId { get; set; }

        [Column("O_ENTRY_D", TypeName = "datetime")]
        public DateTime? OEntryD { get; set; }

        [Column("O_CARRIER_ID")]
        public int? OCarrierId { get; set; }

        [Column("O_OL_CNT")]
        public int? OOlCnt { get; set; }

        [Column("O_ALL_LOCAL")]
        public int? OAllLocal { get; set; }
    }
}