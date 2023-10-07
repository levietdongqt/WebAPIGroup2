using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPIGroup2.Models.POJO;

[Table("Image")]
public partial class Image
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

    public int? Index { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateDate { get; set; }

    [ForeignKey("ProductDetailId")]
    [InverseProperty("Images")]
    public virtual ProductDetail? ProductDetail { get; set; }
}
