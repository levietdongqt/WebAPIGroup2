using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPIGroup2.Models.POJO;

[Table("PrintSize")]
public partial class PrintSize
{
    [Key]
    public int Id { get; set; }

    public double? Length { get; set; }

    public double? Width { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateDate { get; set; }

    [InverseProperty("PrintSize")]
    public virtual ICollection<TemplateSize> TemplateSizes { get; set; } = new List<TemplateSize>();
}
