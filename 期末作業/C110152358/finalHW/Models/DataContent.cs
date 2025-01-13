using System.ComponentModel.DataAnnotations;

namespace finalHW.Models
{
    public class DataContent
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Gender { get; set; } = string.Empty;
        [Required]
        public string CompanyName { get; set; } = string.Empty;


    }
}