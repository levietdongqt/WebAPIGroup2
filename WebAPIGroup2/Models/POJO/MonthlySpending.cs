using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPIGroup2.Models.POJO;

[Table("MonthlySpending")]
public partial class MonthlySpending
{
    [Key]
    public int Id { get; set; }

    public int? UserId { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? Total { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? TimeDetail { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("MonthlySpendings")]
    public virtual User? User { get; set; }
}
