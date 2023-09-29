using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPIGroup2.Models.POJO;

[Table("ContentEmail")]
public partial class ContentEmail
{
    [Key]
    public int Id { get; set; }

    public int? DeliveryInfoId { get; set; }

    [StringLength(256)]
    [Unicode(false)]
    public string? SubjectEmail { get; set; }

    [Column(TypeName = "ntext")]
    public string? BodyEmail { get; set; }

    [StringLength(256)]
    [Unicode(false)]
    public string? Type { get; set; }

    [ForeignKey("DeliveryInfoId")]
    [InverseProperty("ContentEmails")]
    public virtual DeliveryInfo? DeliveryInfo { get; set; }
}
