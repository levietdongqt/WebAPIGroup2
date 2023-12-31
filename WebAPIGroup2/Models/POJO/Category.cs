﻿using System;
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

    [StringLength(500)]
    [Unicode(false)]
    public string? ImageUrl { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<Collection> Collections { get; set; } = new List<Collection>();
}
