using System;
using System.Collections.Generic;

namespace FatFood_T2.Models;

public partial class Catagory
{
    public int CategoryId { get; set; }

    public int RestaurantId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Food> Foods { get; set; } = new List<Food>();

    public virtual Restaurant Restaurant { get; set; } = null!;
}
