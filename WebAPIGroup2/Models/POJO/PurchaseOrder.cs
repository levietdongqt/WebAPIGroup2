using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPIGroup2.Models.POJO;

[Table("PurchaseOrder")]
public partial class PurchaseOrder
{
    [Key]
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? DeliveryInfoId { get; set; }

    [Unicode(false)]
    public string? CreditCard { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? PriceTotal { get; set; }

    [StringLength(40)]
    [Unicode(false)]
    public string Status { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? CreateDate { get; set; }

    [ForeignKey("DeliveryInfoId")]
    [InverseProperty("PurchaseOrders")]
    public virtual DeliveryInfo? DeliveryInfo { get; set; }

    [InverseProperty("PurchaseOrder")]
    public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();

    [ForeignKey("UserId")]
    [InverseProperty("PurchaseOrders")]
    public virtual User? User { get; set; }
}
