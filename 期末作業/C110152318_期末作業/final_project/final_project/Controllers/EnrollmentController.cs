using final_project.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace final_project.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 顯示學生的課程進度
        public async Task<IActionResult> StudentProgress(int studentId)
        {
            // 從資料庫中取得指定學生的註冊資料，並包含課程資訊
            var enrollments = await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .ToListAsync();

            if (!enrollments.Any())
            {
                return NotFound($"找不到 ID 為 {studentId} 的學生或該學生尚未註冊任何課程。");
            }

            ViewData["StudentName"] = _context.Users.FirstOrDefault(u => u.Id == studentId)?.Name ?? "未知學生";
            return View(enrollments);
        }
    }
}
