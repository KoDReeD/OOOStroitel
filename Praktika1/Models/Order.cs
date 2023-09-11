using System;
using System.Collections.Generic;

namespace Praktika1.Models;

public partial class Order
{
    public int Id { get; set; }

    public DateOnly Orderdate { get; set; }

    public DateOnly Orderdeliverydate { get; set; }

    public int Orderpickuppointid { get; set; }

    public int? Clientid { get; set; }

    public int Receiptcode { get; set; }

    public int Orderstatus { get; set; }

    public virtual User? Client { get; set; }

    public virtual Pickuppoint Orderpickuppoint { get; set; } = null!;

    public virtual ICollection<Orderproduct> Orderproducts { get; set; } = new List<Orderproduct>();

    public virtual Orderstatus OrderstatusNavigation { get; set; } = null!;
}
