using System.ComponentModel.DataAnnotations;

public class Assignment
{
    [Key]
    public int Id { get; set; }
    public int EnrollmentId { get; set; } // 外鍵: 參照報名資料
    public string FilePath { get; set; } // 儲存文件的路徑
    public DateTime UploadedAt { get; set; }

    public virtual Enrollment Enrollment { get; set; }
}
