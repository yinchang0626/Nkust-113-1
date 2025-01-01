using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using final_project.Models;
using final_project.Data;
using Microsoft.EntityFrameworkCore;

namespace final_project.Controllers
{
    [Authorize] // 僅限登入用戶
    public class EnrollmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 顯示學生的課程進度
        public async Task<IActionResult> StudentProgress()
        {
            // 從 Claims 中提取 UserId
            var userId = User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int studentId))
            {
                return RedirectToAction("Login", "Account");
            }

            var enrollments = await _context.Enrollments
                .Where(e => e.StudentId == studentId)
                .Include(e => e.Course)
                .ToListAsync();

            if (!enrollments.Any())
            {
                return View("NoEnrollments");
            }

            var student = await _context.Users.FindAsync(studentId);
            if (student == null)
            {
                return NotFound("學生不存在。");
            }

            ViewBag.StudentName = student.Name;
            return View(enrollments);
        }

        // 顯示所有可用課程
        public async Task<IActionResult> AvailableCourses()
        {
            // 從 Claims 中提取 UserId
            var userId = User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int studentId))
            {
                return RedirectToAction("Login", "Account");
            }

            var courses = await _context.Courses.ToListAsync();
            ViewBag.StudentId = studentId; // 傳遞學生 ID 給前端
            return View(courses);
        }

        // 報名課程
        [HttpPost]
        public async Task<IActionResult> Enroll(int courseId)
        {
            // 從 Claims 中提取 UserId
            var userId = User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int studentId))
            {
                return RedirectToAction("Login", "Account");
            }

            var student = await _context.Users.FindAsync(studentId);
            var course = await _context.Courses.FindAsync(courseId);

            if (student == null || course == null)
            {
                return NotFound("學生或課程不存在。");
            }

            // 檢查是否已存在相同的 Enrollment
            bool exists = await _context.Enrollments
                .AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId);

            if (exists)
            {
                TempData["ErrorMessage"] = "您已經報名了這門課程。";
                return RedirectToAction("AvailableCourses");
            }

            // 如果不存在，則新增
            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseId = courseId,
                Progress = 0 // 預設進度為 0
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            return RedirectToAction("StudentProgress");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEnrollment(int enrollmentId)
        {
            var enrollment = await _context.Enrollments.FindAsync(enrollmentId);

            if (enrollment == null)
            {
                TempData["ErrorMessage"] = "找不到此報名資料。";
                return RedirectToAction("StudentProgress", new { studentId = User.FindFirst("UserId")?.Value });
            }

            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "課程已成功刪除。";
            return RedirectToAction("StudentProgress", new { studentId = User.FindFirst("UserId")?.Value });
        }
    }
}
