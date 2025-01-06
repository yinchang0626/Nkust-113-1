using System.ComponentModel.DataAnnotations;

namespace final_project.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public int Credits { get; set; }
        // 課程的學生註冊資訊
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
