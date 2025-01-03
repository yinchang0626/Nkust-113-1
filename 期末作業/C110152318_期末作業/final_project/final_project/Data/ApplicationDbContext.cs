using final_project.Models;
using Microsoft.EntityFrameworkCore;

namespace final_project.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // 添加 DbSet 屬性來表示資料庫中的表
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 設定 User 與 Enrollment 的一對多關係
            modelBuilder.Entity<User>()
                .HasMany(u => u.Enrollments) // User (學生) 參與多個註冊
                .WithOne(e => e.Student) // 每個註冊對應一個學生
                .HasForeignKey(e => e.StudentId) // 外鍵是 Enrollment 的 StudentId
                .OnDelete(DeleteBehavior.Restrict); // 防止級聯刪除

            // 設定 Course 與 Enrollment 的一對多關係
            modelBuilder.Entity<Course>()
                .HasMany(c => c.Enrollments) // Course 可以有多個註冊
                .WithOne(e => e.Course) // 每個註冊屬於一門課
                .HasForeignKey(e => e.CourseId) // 外鍵是 Enrollment 的 CourseId
                .OnDelete(DeleteBehavior.Cascade); // 刪除課程時刪除相關註冊
                                                   // 配置 Enrollment 的複合索引
            modelBuilder.Entity<Assignment>()
                .HasOne(a => a.Enrollment)  // Assignment 會有一個 Enrollment
                .WithMany(e => e.Assignments)  // 每個 Enrollment 會有多個 Assignment
                .HasForeignKey(a => a.EnrollmentId);  // 指定外鍵
        }

    }
}
