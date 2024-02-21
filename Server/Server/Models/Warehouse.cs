using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Server.Models;

[Table("Warehouse")]
public partial class Warehouse
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("NAME")]
    [StringLength(100)]
    public string? Name { get; set; }

    [Column("STREET_1")]
    [StringLength(20)]
    public string? Street1 { get; set; }

    [Column("STREET_2")]
    [StringLength(20)]
    public string? Street2 { get; set; }

    [Column("CITY")]
    [StringLength(20)]
    public string? City { get; set; }

    [Column("STATE")]
    [StringLength(2)]
    public string? State { get; set; }

    [Column("ZIP")]
    [StringLength(9)]
    public string? Zip { get; set; }

    [Column("W_TAX")]
    public double? WTax { get; set; }

    [Column("YTD")]
    public double? Ytd { get; set; }

    [InverseProperty("DW")]
    public virtual ICollection<District> Districts { get; set; } = new List<District>();

    [InverseProperty("SW")]
    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
