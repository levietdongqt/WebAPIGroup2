using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPIGroup2.Models.POJO;

[Table("RefeshToken")]
public partial class RefeshToken
{
    [Key]
    public int Id { get; set; }

    public int? UserId { get; set; }

    [Unicode(false)]
    public string? JwtId { get; set; }

    [Unicode(false)]
    public string? Token { get; set; }

    [Required]
    public bool? IsUsed { get; set; }

    [Required]
    public bool? IsRevoked { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? IssueAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ExpireAt { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("RefeshTokens")]
    public virtual User? User { get; set; }
}
