using final_project.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Enrollment
{
    [Key]
    public int Id { get; set; }

    // 所屬學生
    public int StudentId { get; set; }
    public User? Student { get; set; }

    // 所屬課程
    public int CourseId { get; set; }
    public Course? Course { get; set; }

    public double Progress { get; set; } // 學習進度

    // 在模型中明確標註索引的組成（僅作註解，實際索引配置在 DbContext 中）
    // [Index(nameof(StudentId), nameof(CourseId), IsUnique = true)]
}
