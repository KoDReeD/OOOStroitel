using System;
using System.Collections.Generic;

namespace Praktika1.Models;

public partial class Product
{
    public string Articlenumber { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Unitname { get; set; } = null!;

    public decimal Price { get; set; }

    public short? Maxdiscount { get; set; }

    public int Manufacturerid { get; set; }

    public int Providerid { get; set; }

    public int Categoryid { get; set; }

    public short? Currentdiscount { get; set; }

    public int Quantityinstock { get; set; }

    public string Description { get; set; } = null!;

    public string? Photo { get; set; }

    public virtual Productcategory Category { get; set; } = null!;

    public virtual Manufacturer Manufacturer { get; set; } = null!;

    public virtual ICollection<Orderproduct> Orderproducts { get; set; } = new List<Orderproduct>();

    public virtual Provider Provider { get; set; } = null!;
}
