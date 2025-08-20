using System;
using System.Collections.Generic;

namespace FatFood_T2.Models;

public partial class User
{
    public int Id { get; set; }

    public string FName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? Phone { get; set; }

    public int Role { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Deliveradd> Deliveradds { get; set; } = new List<Deliveradd>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Restaurant> Restaurants { get; set; } = new List<Restaurant>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
