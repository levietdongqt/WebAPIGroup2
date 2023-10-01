using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPIGroup2.Models.POJO;

[Table("DescriptionTemplate")]
public partial class DescriptionTemplate
{
    [Key]
    public int Id { get; set; }

    [Unicode(false)]
    public string? Title { get; set; }

    [Column(TypeName = "ntext")]
    public string? Description { get; set; }

    public int? TemplateId { get; set; }

    [ForeignKey("TemplateId")]
    [InverseProperty("DescriptionTemplates")]
    public virtual Template? Template { get; set; }
}
