using System.ComponentModel.DataAnnotations;

namespace final_project.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        // 學生的註冊資訊
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
