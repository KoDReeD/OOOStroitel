using System;
using System.Collections.Generic;

namespace Praktika1.Models;

public partial class Pickuppoint
{
    public int Id { get; set; }

    public string Address { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
