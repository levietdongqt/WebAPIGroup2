using System;
using System.Collections.Generic;

namespace WebAPIGroup2.Models.POJO;

public partial class UserRole
{
    public int Id { get; set; }

    public string? Role { get; set; }

    public string? UserId { get; set; }

    public virtual User? User { get; set; }
}
