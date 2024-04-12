using System;
using System.Collections.Generic;

namespace SecurityLab.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateOnly BirthDate { get; set; }

    public string? CountryOfResidence { get; set; }

    public string? Gender { get; set; }

    public double Age { get; set; }

    public string? UserId { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
