using System;
using System.Collections.Generic;

namespace Praktika1.Models;

public partial class Provider
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
