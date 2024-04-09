using SecurityLab.Models;
using System;
using System.Collections.Generic;

namespace SecurityLab.Models;

public partial class CartLine
{
    public int CartLineId { get; set; }

    public int LegoproductId { get; set; }

    public int Quantity { get; set; }

    public int? OrderId { get; set; }

    public virtual Product Legoproduct { get; set; } = null!;

    public virtual Order? Order { get; set; }
}
