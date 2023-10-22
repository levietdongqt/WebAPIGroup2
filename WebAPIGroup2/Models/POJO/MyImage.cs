using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPIGroup2.Models.POJO;

public partial class MyImage
{
    [Key]
    public int Id { get; set; }

    public int? TemplateId { get; set; }

    public int? PurchaseOrderId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateDate { get; set; }

    public bool? Status { get; set; }

    [InverseProperty("MyImages")]
    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    [InverseProperty("MyImage")]
    public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();

    [ForeignKey("PurchaseOrderId")]
    [InverseProperty("MyImages")]
    public virtual PurchaseOrder? PurchaseOrder { get; set; }

    [ForeignKey("TemplateId")]
    [InverseProperty("MyImages")]
    public virtual Template? Template { get; set; }
}
