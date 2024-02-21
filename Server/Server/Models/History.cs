using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Server.Models;

[Keyless]
[Table("History")]
public partial class History
{
    [Column("H_C_ID")]
    public int? HCId { get; set; }

    [Column("H_C_D_ID")]
    public int? HCDId { get; set; }

    [Column("H_C_W_ID")]
    public int? HCWId { get; set; }

    [Column("H_D_ID")]
    public int? HDId { get; set; }

    [Column("H_W_ID")]
    public int? HWId { get; set; }

    [Column("DATE", TypeName = "datetime")]
    public DateTime? Date { get; set; }

    [Column("AMOUNT")]
    public double? Amount { get; set; }

    [Column("DATA")]
    [StringLength(24)]
    public string? Data { get; set; }

    [ForeignKey("HCId")]
    public virtual Customer? HC { get; set; }
}
