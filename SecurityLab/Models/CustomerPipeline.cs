using System;
using System.Collections.Generic;

namespace SecurityLab.Models;

public partial class CustomerPipeline
{
    public int ClusterId { get; set; }

    public int Purchase1 { get; set; }

    public int Purchase2 { get; set; }

    public int Purchase3 { get; set; }

    public int Rating1 { get; set; }

    public int Rating2 { get; set; }

    public int Rating3 { get; set; }
}
