using System;
using System.Collections.Generic;

namespace TouristSpotWeb.Models;

public partial class Users
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string? PasswordHash { get; set; }

    public string? Email { get; set; }
}
