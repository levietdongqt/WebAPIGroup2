using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPIGroup2.Models.POJO;

[Table("MaterialPage")]
public partial class MaterialPage
{
    [Key]
    public int Id { get; set; }

    public string? Name { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? InchSold { get; set; }

    public double? PricePerInch { get; set; }

    [Required]
    public bool? Status { get; set; }

    public string? Description { get; set; }

    [InverseProperty("MaterialPage")]
    public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();
}
