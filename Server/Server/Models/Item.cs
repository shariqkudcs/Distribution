using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Server.Models;

[Table("Item")]
public partial class Item
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("NAME")]
    [StringLength(24)]
    public string? Name { get; set; }

    [Column("PRICE")]
    public double? Price { get; set; }

    [Column("DATA")]
    public string? Data { get; set; }

    [InverseProperty("SI")]
    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
