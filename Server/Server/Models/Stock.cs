using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Server.Models;

[Table("Stock")]
[Index("SIId", "SWId", Name = "S_I_ID_S_W_ID", IsUnique = true)]
public partial class Stock
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("S_I_ID")]
    public int SIId { get; set; }

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

    [InverseProperty("Stock")]
    public virtual ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();

    [ForeignKey("SIId")]
    [InverseProperty("Stocks")]
    public virtual Item SI { get; set; } = null!;

    [ForeignKey("SWId")]
    [InverseProperty("Stocks")]
    public virtual Warehouse SW { get; set; } = null!;
}
