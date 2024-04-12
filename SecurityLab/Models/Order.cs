using System;
using System.Collections.Generic;

namespace SecurityLab.Models;

public partial class Order
{
    public int TransactionId { get; set; }
    public ICollection<CartLine> Lines { get; set; }
    = new List<CartLine>();

    public int CustomerId { get; set; }

    public DateOnly Date { get; set; }

    public string? DayOfWeek { get; set; }

    public byte Time { get; set; }

    public string EntryMode { get; set; } = null!;

    public short? Amount { get; set; }

    public string TypeOfTransaction { get; set; } = null!;

    public string CountryOfTransaction { get; set; } = null!;

    public string? ShippingAddress { get; set; }

    public string Bank { get; set; } = null!;

    public string TypeOfCard { get; set; } = null!;

    public bool Fraud { get; set; }

    public virtual Customer Customer { get; set; } = null!;
}
