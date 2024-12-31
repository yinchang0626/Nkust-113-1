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

        public IActionResult StudentProgress(int studentId)
        {
            var enrollments = _context.Enrollments
                .Where(e => e.StudentId == studentId)
                .Include(e => e.Course)
                .ToList();

            if (enrollments.Count == 0)
            {
                return View("NoEnrollments");
            }

            ViewData["StudentName"] = _context.Users.FirstOrDefault(u => u.Id == studentId)?.Name;
            return View(enrollments);
        }
    }
}
