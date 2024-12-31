using final_project.Data;
using final_project.Models;
using Microsoft.EntityFrameworkCore;

namespace final_project;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new ApplicationDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
        {
            // 如果資料庫中已經有資料，則不執行任何操作
            if (context.Users.Any() || context.Courses.Any() || context.Enrollments.Any())
            {
                return;
            }

            // 新增學生
            var students = new[]
            {
                new User { Name = "Alice", Email = "alice@example.com" },
                new User { Name = "Bob", Email = "bob@example.com" },
                new User { Name = "Charlie", Email = "charlie@example.com" }
            };
            context.Users.AddRange(students);

            // 新增課程
            var courses = new[]
            {
                new Course { Name = "Mathematics", Description = "Introductory Math", Credits = 3 },
                new Course { Name = "History", Description = "World History Overview", Credits = 2 },
                new Course { Name = "Programming", Description = "Learn C# Programming", Credits = 4 }
            };
            context.Courses.AddRange(courses);

            // 新增註冊資料
            var enrollments = new[]
            {
                new Enrollment { Student = students[0], Course = courses[0], Progress = 0.75 },
                new Enrollment { Student = students[0], Course = courses[1], Progress = 0.50 },
                new Enrollment { Student = students[1], Course = courses[2], Progress = 0.30 },
                new Enrollment { Student = students[2], Course = courses[0], Progress = 0.90 }
            };
            context.Enrollments.AddRange(enrollments);

            // 儲存變更到資料庫
            context.SaveChanges();
        }
    }
}
