using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Server.Models;

[Table("District")]
[Index("DId", "DWId", Name = "D_ID_D_W_ID", IsUnique = true)]
public partial class District
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("D_ID")]
    public int DId { get; set; }

    [Column("D_W_ID")]
    public int DWId { get; set; }

    [Column("NAME")]
    [StringLength(100)]
    public string? Name { get; set; }

    [Column("STREET_1")]
    [StringLength(100)]
    public string? Street1 { get; set; }

    [Column("STREET_2")]
    [StringLength(100)]
    public string? Street2 { get; set; }

    [Column("CITY")]
    [StringLength(100)]
    public string? City { get; set; }

    [Column("STATE")]
    [StringLength(2)]
    public string? State { get; set; }

    [Column("ZIP")]
    [StringLength(10)]
    public string? Zip { get; set; }

    [Column("TAX")]
    public double? Tax { get; set; }

    [Column("YTD")]
    public double? Ytd { get; set; }

    [Column("NEXT_O_ID")]
    public int? NextOId { get; set; }

    [InverseProperty("District")]
    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    [ForeignKey("DWId")]
    [InverseProperty("Districts")]
    public virtual Warehouse DW { get; set; } = null!;
}
