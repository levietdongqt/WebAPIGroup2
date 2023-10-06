using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPIGroup2.Models.POJO;

[Table("CollectionTemplate")]
public partial class CollectionTemplate
{
    [Key]
    public int Id { get; set; }

    public int? CollectionId { get; set; }

    public int? TemplateId { get; set; }

    [ForeignKey("CollectionId")]
    [InverseProperty("CollectionTemplates")]
    public virtual Collection? Collection { get; set; }

    [ForeignKey("TemplateId")]
    [InverseProperty("CollectionTemplates")]
    public virtual Template? Template { get; set; }
}
