using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecurityLab.Models;

public partial class Order
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TransactionId { get; set; }

    public int CustomerId { get; set; }

    public ICollection<CartLine> Lines { get; set; }
    = new List<CartLine>();
    public DateOnly Date { get; set; }

    public string DayOfWeek { get; set; } = null!;

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
