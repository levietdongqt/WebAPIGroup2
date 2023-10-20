using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPIGroup2.Models.POJO;

[Table("FeedBack")]
public partial class FeedBack
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "ntext")]
    public string? Content { get; set; }

    public int? UserId { get; set; }

    public bool? isImportant { get; set; }

    [StringLength(256)]
    public string? Email { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FeedBackDate { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("FeedBacks")]
    public virtual User? User { get; set; }
}
