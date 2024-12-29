using System.ComponentModel.DataAnnotations;

namespace final_project.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        public String Street { get; set; }
        public String City { get; set; }
        public String state { get; set; }
    }
}
