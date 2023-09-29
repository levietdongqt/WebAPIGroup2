using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPIGroup2.Models.POJO;

[Table("Category")]
public partial class Category
{
    [Key]
    public int Id { get; set; }

    public string? Name { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<CategoryTemplate> CategoryTemplates { get; set; } = new List<CategoryTemplate>();
}
