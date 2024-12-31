using final_project.Data;
using final_project.Models;
using Microsoft.EntityFrameworkCore;

namespace final_project;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using (var context = new ApplicationDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
        {
            if (await IsDatabaseInitializedAsync(context))
            {
                return;
            }

            var students = new[]
            {
                new User { Name = "Alice", Email = "alice@example.com", Password = "Password123" },
                new User { Name = "Bob", Email = "bob@example.com", Password = "Password123" },
                new User { Name = "Charlie", Email = "charlie@example.com", Password = "Password123" }
            };
            await context.Users.AddRangeAsync(students);

            var courses = new[]
            {
                new Course { Name = "Mathematics", Description = "Introductory Math", Credits = 3 },
                new Course { Name = "History", Description = "World History Overview", Credits = 2 },
                new Course { Name = "Programming", Description = "Learn C# Programming", Credits = 4 }
            };
            await context.Courses.AddRangeAsync(courses);

            var enrollments = new[]
            {
                new Enrollment { Student = students[0], Course = courses[0], Progress = 0.75 },
                new Enrollment { Student = students[0], Course = courses[1], Progress = 0.50 },
                new Enrollment { Student = students[1], Course = courses[2], Progress = 0.30 },
                new Enrollment { Student = students[2], Course = courses[0], Progress = 0.90 }
            };
            await context.Enrollments.AddRangeAsync(enrollments);

            await context.SaveChangesAsync();
        }
    }

    private static async Task<bool> IsDatabaseInitializedAsync(ApplicationDbContext context)
    {
        return await context.Users.AnyAsync() || await context.Courses.AnyAsync() || await context.Enrollments.AnyAsync();
    }
}
