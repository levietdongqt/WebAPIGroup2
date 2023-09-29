using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPIGroup2.Models.POJO;

[Table("TemplateImage")]
public partial class TemplateImage
{
    [Key]
    public int Id { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? ImageUrl { get; set; }

    public int? TemplateId { get; set; }

    [ForeignKey("TemplateId")]
    [InverseProperty("TemplateImages")]
    public virtual Template? Template { get; set; }
}
