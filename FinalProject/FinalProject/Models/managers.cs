using System;
using System.Collections.Generic;

namespace FinalProject.Models;

public partial class managers
{
    public int id { get; set; }

    public string name { get; set; } = null!;

    public string mail { get; set; } = null!;

    public string password { get; set; } = null!;
}
