using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    public class Member
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }
        public DateTime Birthday { get; set; }
        public string Email { get; set; }
    }
}