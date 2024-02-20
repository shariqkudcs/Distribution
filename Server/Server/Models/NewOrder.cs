using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Server.Models;

[Keyless]
[Table("NewOrder")]
public partial class NewOrder
{
    [Column("ID")]
    public int Id { get; set; }

    [Column("NO_D_ID")]
    public int? NoDId { get; set; }

    [Column("NO_W_ID")]
    public int? NoWId { get; set; }
}
