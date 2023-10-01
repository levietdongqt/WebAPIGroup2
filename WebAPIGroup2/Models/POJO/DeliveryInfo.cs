using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPIGroup2.Models.POJO;

[Table("DeliveryInfo")]
public partial class DeliveryInfo
{
    [Key]
    public int Id { get; set; }

    public int? UserId { get; set; }

    [StringLength(256)]
    public string? Email { get; set; }

    [Column(TypeName = "ntext")]
    public string? DeliveryAddress { get; set; }

    public string? Phone { get; set; }

    [InverseProperty("DeliveryInfo")]
    public virtual ICollection<ContentEmail> ContentEmails { get; set; } = new List<ContentEmail>();

    [InverseProperty("DeliveryInfo")]
    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();

    [ForeignKey("UserId")]
    [InverseProperty("DeliveryInfos")]
    public virtual User? User { get; set; }
}
