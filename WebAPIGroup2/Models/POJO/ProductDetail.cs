using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPIGroup2.Models.POJO;

[Table("ProductDetail")]
public partial class ProductDetail
{
    [Key]
    public int Id { get; set; }

    public int? TemplateId { get; set; }

    public int? MaterialPageId { get; set; }

    public int? PurchaseOrderId { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? Price { get; set; }

    public int? Quantity { get; set; }

    public bool? Status { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateDate { get; set; }

    [ForeignKey("MaterialPageId")]
    [InverseProperty("ProductDetails")]
    public virtual MaterialPage? MaterialPage { get; set; }

    [ForeignKey("PurchaseOrderId")]
    [InverseProperty("ProductDetails")]
    public virtual PurchaseOrder? PurchaseOrder { get; set; }

    [ForeignKey("TemplateId")]
    [InverseProperty("ProductDetails")]
    public virtual TemplateSize? Template { get; set; }

    [InverseProperty("ProductDetail")]
    public virtual ICollection<UploadImage> UploadImages { get; set; } = new List<UploadImage>();
}
