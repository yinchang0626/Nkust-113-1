using System;
using System.Collections.Generic;

namespace FinalProject.Models;

public partial class hotels
{
    public byte id { get; set; }

    public string _class { get; set; } = null!;

    public string? star { get; set; }

    public string name { get; set; } = null!;

    public string? district { get; set; }

    public string address { get; set; } = null!;

    public string tel { get; set; } = null!;

    public short? amount { get; set; }

    public string? email { get; set; }

    public string? website { get; set; }

    public double lng { get; set; }

    public double lat { get; set; }
}
