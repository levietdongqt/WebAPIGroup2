using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPIGroup2.Models.POJO;

[Table("User")]
[Index("Email", Name = "UNI_Email", IsUnique = true)]
public partial class User
{
    [Key]
    public int Id { get; set; }

    [StringLength(256)]
    public string? Email { get; set; }

    [StringLength(256)]
    public string? Password { get; set; }

    [StringLength(256)]
    public string? FullName { get; set; }

    public bool? EmailConfirmed { get; set; }

    [Column(TypeName = "date")]
    public DateTime? DateOfBirth { get; set; }

    [StringLength(256)]
    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? Avatar { get; set; }

    public bool? Gender { get; set; }   

    [StringLength(50)]
    [Unicode(false)]
    public string? Role { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? Status { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateDate { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<DeliveryInfo> DeliveryInfos { get; set; } = new List<DeliveryInfo>();

    [InverseProperty("User")]
    public virtual ICollection<FeedBack> FeedBacks { get; set; } = new List<FeedBack>();

    [InverseProperty("User")]
    public virtual ICollection<MonthlySpending> MonthlySpendings { get; set; } = new List<MonthlySpending>();

    [InverseProperty("User")]
    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();

    [InverseProperty("User")]
    public virtual ICollection<RefeshToken> RefeshTokens { get; set; } = new List<RefeshToken>();

    [InverseProperty("User")]
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
