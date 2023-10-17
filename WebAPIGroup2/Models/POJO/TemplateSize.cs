using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPIGroup2.Models.POJO;

[Table("TemplateSize")]
public partial class TemplateSize
{
    [Key]
    public int Id { get; set; }

    public int? TemplateId { get; set; }

    public int? PrintSizeId { get; set; }

    [ForeignKey("PrintSizeId")]
    [InverseProperty("TemplateSizes")]
    public virtual PrintSize? PrintSize { get; set; }

    [InverseProperty("TemplateSize")]
    public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();

    [ForeignKey("TemplateId")]
    [InverseProperty("TemplateSizes")]
    public virtual Template? Template { get; set; }
}
