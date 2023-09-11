using System;
using System.Collections.Generic;

namespace Praktika1.Models;

public partial class Orderproduct
{
    public int Orderid { get; set; }

    public string Productarticlenumber { get; set; } = null!;

    public int Amount { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product ProductarticlenumberNavigation { get; set; } = null!;
}
