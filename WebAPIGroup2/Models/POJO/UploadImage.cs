using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPIGroup2.Models.POJO;

[Table("UploadImage")]
public partial class UploadImage
{
    [Key]
    public int Id { get; set; }

    public int? ProductDetailId { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? ImageUrl { get; set; }

    [StringLength(500)]
    public string? FolderName { get; set; }

    [Required]
    public bool? Status { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateDate { get; set; }

    [ForeignKey("ProductDetailId")]
    [InverseProperty("UploadImages")]
    public virtual ProductDetail? ProductDetail { get; set; }
}
