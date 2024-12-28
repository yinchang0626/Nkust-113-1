using System.ComponentModel.DataAnnotations;
namespace WebApplication1.Models
{
    public class AppUser
    {
        [Key]
        public string Id {  get; set; }
        public Address? AddressP { get; set; }  //可能為null
        public int? Pace { get; set; } //步
        public int? Mileage { get; set;} //里程

        public ICollection<Club> clubs { get; set; } //集合屬性

        public ICollection<Races> races { get; set; } //集合屬性
    }
}
