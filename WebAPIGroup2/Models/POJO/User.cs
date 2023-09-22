using System;
using System.Collections.Generic;

namespace WebAPIGroup2.Models.POJO;

public partial class User
{
    public string Id { get; set; } = null!;

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public bool? EmailConfirmed { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? Address { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
