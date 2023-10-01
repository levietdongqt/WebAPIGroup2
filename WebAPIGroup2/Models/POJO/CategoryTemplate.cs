using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPIGroup2.Models.POJO;

[Table("CategoryTemplate")]
public partial class CategoryTemplate
{
    [Key]
    public int Id { get; set; }

    public int? CategoryId { get; set; }

    public int? TemplateId { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("CategoryTemplates")]
    public virtual Category? Category { get; set; }

    [ForeignKey("TemplateId")]
    [InverseProperty("CategoryTemplates")]
    public virtual Template? Template { get; set; }
}
