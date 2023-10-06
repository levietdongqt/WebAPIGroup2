using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPIGroup2.Models.POJO;

[Table("Collection")]
public partial class Collection
{
    [Key]
    public int Id { get; set; }

    public string? Name { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? ImageUrl { get; set; }

    public int? CategoryId { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Collections")]
    public virtual Category? Category { get; set; }

    [InverseProperty("Collection")]
    public virtual ICollection<CollectionTemplate> CollectionTemplates { get; set; } = new List<CollectionTemplate>();
}
