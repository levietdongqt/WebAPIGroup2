using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPIGroup2.Models.POJO;

[Table("Template")]
public partial class Template
{
    [Key]
    public int Id { get; set; }

    public string? Name { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? PricePlus { get; set; }

    [Required]
    public bool? Status { get; set; }

    public int? QuantitySold { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateDate { get; set; }

    [InverseProperty("Template")]
    public virtual ICollection<CategoryTemplate> CategoryTemplates { get; set; } = new List<CategoryTemplate>();

    [InverseProperty("Template")]
    public virtual ICollection<DescriptionTemplate> DescriptionTemplates { get; set; } = new List<DescriptionTemplate>();

    [InverseProperty("Template")]
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    [InverseProperty("Template")]
    public virtual ICollection<TemplateImage> TemplateImages { get; set; } = new List<TemplateImage>();

    [InverseProperty("Template")]
    public virtual ICollection<TemplateSize> TemplateSizes { get; set; } = new List<TemplateSize>();
}
