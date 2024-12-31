using final_project.Models;
using System.ComponentModel.DataAnnotations;

public class Enrollment
{
    [Key]
    public int Id { get; set; }

    // 所屬學生
    public int StudentId { get; set; }
    public User Student { get; set; }

    // 所屬課程
    public int CourseId { get; set; }
    public Course Course { get; set; }

    public double Progress { get; set; } // 學習進度
}
