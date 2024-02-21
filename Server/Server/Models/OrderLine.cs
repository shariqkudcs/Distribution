using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Server.Models;

[Table("OrderLine")]
public partial class OrderLine
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("OL_O_ID")]
    public int OlOId { get; set; }

    [Column("OL_D_ID")]
    public int OlDId { get; set; }

    [Column("OL_W_ID")]
    public int OlWId { get; set; }

    [Column("OL_ID")]
    public int OlId { get; set; }

    [Column("OL_I_ID")]
    public int? OlIId { get; set; }

    [Column("OL_SUPPLY_W_ID")]
    public int? OlSupplyWId { get; set; }

    [Column("DELIVERY_D", TypeName = "datetime")]
    public DateTime? DeliveryD { get; set; }

    [Column("QUANTITY")]
    public int? Quantity { get; set; }

    [Column("AMOUNT")]
    public double? Amount { get; set; }

    [Column("DIST_INFO")]
    [StringLength(24)]
    public string? DistInfo { get; set; }

    [ForeignKey("OlOId")]
    [InverseProperty("OrderLines")]
    public virtual Order OlO { get; set; } = null!;

    [ForeignKey("OlIId, OlWId")]
    [InverseProperty("OrderLines")]
    public virtual Stock? Stock { get; set; }
}
