using System;
using System.Collections.Generic;

namespace SecurityLab.Models;

public partial class Purchase
{
    public int OrderId { get; set; }

    public ICollection<CartLine> Lines { get; set; }
      = new List<CartLine>();
    public string? Name { get; set; }

    public string? Line1 { get; set; }

    public string? Line2 { get; set; }

    public string? Line3 { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public int? Zip { get; set; }

    public string? Country { get; set; }
}
