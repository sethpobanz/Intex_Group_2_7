using System;
using System.Collections.Generic;

namespace SecurityLab.Models;

public partial class Customer
{
    public short CustomerId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateOnly BirthDate { get; set; }

    public string CountryOfResidence { get; set; } = null!;

    public string? Gender { get; set; }

    public double Age { get; set; }
}
