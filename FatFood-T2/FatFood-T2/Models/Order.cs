using System;
using System.Collections.Generic;

namespace FatFood_T2.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    public int RestaurantId { get; set; }

    public DateTime? OrderDate { get; set; }

    public int? Status { get; set; }

    public decimal TotalAmount { get; set; }

    public virtual User Customer { get; set; } = null!;

    public virtual ICollection<Orderdetail> Orderdetails { get; set; } = new List<Orderdetail>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Restaurant Restaurant { get; set; } = null!;
}
