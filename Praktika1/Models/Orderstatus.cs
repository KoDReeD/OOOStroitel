using System;
using System.Collections.Generic;

namespace Praktika1.Models;

public partial class Orderstatus
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
