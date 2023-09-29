using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPIGroup2.Models.POJO;

[Table("Review")]
public partial class Review
{
    [Key]
    public int Id { get; set; }

    public int? TemplateId { get; set; }

    public int? UserId { get; set; }

    [Column(TypeName = "ntext")]
    public string? Content { get; set; }

    public double? Rating { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ReviewDate { get; set; }

    [ForeignKey("TemplateId")]
    [InverseProperty("Reviews")]
    public virtual Template? Template { get; set; }
}
