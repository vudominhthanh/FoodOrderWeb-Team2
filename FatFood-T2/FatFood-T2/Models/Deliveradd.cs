using System;
using System.Collections.Generic;

namespace FatFood_T2.Models;

public partial class Deliveradd
{
    public int AddressId { get; set; }

    public int UserId { get; set; }

    public string? Label { get; set; }

    public string Address { get; set; } = null!;

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public bool? IsDefault { get; set; }

    public virtual User User { get; set; } = null!;
}
