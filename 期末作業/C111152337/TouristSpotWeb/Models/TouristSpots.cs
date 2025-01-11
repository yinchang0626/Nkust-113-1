using System;
using System.Collections.Generic;

namespace TouristSpotWeb.Models;

public partial class TouristSpots
{
    public string Id { get; set; } = null!;

    public string? Name { get; set; }

    public string? Zone { get; set; }

    public string? Toldescribe { get; set; }

    public string? Description { get; set; }

    public string? Tel { get; set; }

    public string? Address { get; set; }

    public int? Zipcode { get; set; }

    public string? Region { get; set; }

    public string? Town { get; set; }

    public string? Travellinginfo { get; set; }

    public string? Opentime { get; set; }

    public string? Picture1 { get; set; }

    public string? Picdescribe1 { get; set; }

    public string? Picture2 { get; set; }

    public string? Picdescribe2 { get; set; }

    public string? Picture3 { get; set; }

    public string? Picdescribe3 { get; set; }

    public string? Map { get; set; }

    public string? Gov { get; set; }

    public double? Px { get; set; }

    public double? Py { get; set; }

    public string? Orgclass { get; set; }

    public byte? Class1 { get; set; }

    public byte? Class2 { get; set; }

    public byte? Class3 { get; set; }

    public byte? Level { get; set; }

    public string? Website { get; set; }

    public string? Parkinginfo { get; set; }

    public double? Parkinginfo_Px { get; set; }

    public double? Parkinginfo_Py { get; set; }

    public string? Ticketinfo { get; set; }

    public string? Remarks { get; set; }

    public string? Keyword { get; set; }

    public DateTime? Changetime { get; set; }
}
