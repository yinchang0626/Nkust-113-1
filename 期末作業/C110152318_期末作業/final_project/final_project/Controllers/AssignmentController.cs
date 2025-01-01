using final_project.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using final_project.Models;

public class AssignmentController : Controller
{
    private readonly ApplicationDbContext _context;

    public AssignmentController(ApplicationDbContext context)
    {
        _context = context;
    }

    // 顯示作業上傳表單
    public IActionResult Upload(int enrollmentId)
    {
        var enrollment = _context.Enrollments
            .Include(e => e.Course)
            .FirstOrDefault(e => e.Id == enrollmentId);

        if (enrollment == null || enrollment.Course == null)
        {
            TempData["ErrorMessage"] = "找不到相關課程資料。";
            return RedirectToAction("Index", "Home");
        }

        ViewBag.CourseName = enrollment.Course.Name;
        ViewBag.EnrollmentId = enrollmentId;

        return View();
    }

    public IActionResult ViewAssignments(int enrollmentId)
    {
        // 從資料庫中獲取具體的 Enrollment 物件，並確保加載相關的作業資料
        var enrollment = _context.Enrollments
            .Include(e => e.Student)    // 確保加載 Student 物件
            .Include(e => e.Assignments) // 確保加載作業資料
            .FirstOrDefault(e => e.Id == enrollmentId);

        // 若找不到對應的 Enrollment，返回 404
        if (enrollment == null)
        {
            return NotFound();
        }

        // 如果 Student 為 null，返回錯誤訊息或預設頁面
        if (enrollment.Student == null)
        {
            TempData["ErrorMessage"] = "無法找到對應的學生資料。";
            return RedirectToAction("Index", "Home"); // 可根據需要導向首頁或其他頁面
        }

        // 如果沒有作業資料，顯示對應訊息
        if (enrollment.Assignments == null || !enrollment.Assignments.Any())
        {
            TempData["Message"] = "該學生尚未提交作業。";
        }

        // 返回視圖並傳遞 Enrollment 物件
        return View(enrollment);
    }

    // 處理作業上傳
    [HttpPost]
    public async Task<IActionResult> Upload(int enrollmentId, IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            TempData["ErrorMessage"] = "請選擇一個文件上傳。";
            return RedirectToAction("Upload", new { enrollmentId });
        }

        // 檢查檔案大小，限制為10MB
        if (file.Length > 10 * 1024 * 1024)
        {
            TempData["ErrorMessage"] = "檔案大小不能超過 10MB。";
            return RedirectToAction("Upload", new { enrollmentId });
        }

        var enrollment = await _context.Enrollments
            .Include(e => e.Course)
            .Include(e => e.Student)
            .FirstOrDefaultAsync(e => e.Id == enrollmentId);

        if (enrollment == null)
        {
            TempData["ErrorMessage"] = "無效的報名資料。";
            return RedirectToAction("Index", "Home");
        }

        // 生成唯一的檔案名稱
        string originalFileName = Path.GetFileNameWithoutExtension(file.FileName);
        string fileExtension = Path.GetExtension(file.FileName);
        string currentDate = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string uniqueFileName = $"{currentDate}_{originalFileName}{fileExtension}";

        string studentEmail = enrollment.Student?.Email ?? "未提供電子郵件";
        string courseName = enrollment.Course?.Name ?? "未提供課程名稱";

        string uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", studentEmail, courseName);
        Directory.CreateDirectory(uploadsDirectory);

        string filePath = Path.Combine(uploadsDirectory, uniqueFileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var assignment = new Assignment
        {
            EnrollmentId = enrollmentId,
            FilePath = filePath,
            UploadedAt = DateTime.Now
        };

        _context.Assignments.Add(assignment);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "作業上傳成功！";
        return RedirectToAction("StudentProgress", "Enrollment");
    }

    // 下載作業檔案
    public async Task<IActionResult> DownloadFile(string studentEmail, string fileName)
    {
        // 根據學生 Email 和檔案名稱來查找作業
        var assignment = await _context.Assignments
            .Include(a => a.Enrollment) // 加載 Enrollment 物件
            .ThenInclude(e => e.Course) // 加載對應的 Course 物件
            .FirstOrDefaultAsync(a => a.FilePath.EndsWith(fileName) && a.Enrollment.Student.Email == studentEmail);

        if (assignment == null)
        {
            return NotFound();  // 若找不到對應作業，返回 404
        }

        // 直接使用 Assignment 中的 FilePath 來查找檔案
        string filePath = assignment.FilePath;

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound();  // 檔案不存在，返回 404
        }

        var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
        fileName = Path.GetFileName(assignment.FilePath);
        var fileExtension = Path.GetExtension(fileName);
        var contentType = GetContentType(fileExtension);
        return File(fileBytes, contentType, fileName);
    }


    // 用於根據檔案擴展名獲取 MIME 類型的輔助方法
    private string GetContentType(string fileExtension)
    {
        switch (fileExtension.ToLower())
        {
            case ".pdf": return "application/pdf";
            case ".zip": return "application/zip";
            case ".jpg": return "image/jpeg";
            case ".png": return "image/png";
            case ".txt": return "text/plain";
            default: return "application/octet-stream";
        }
    }

    // 生成檔案路徑的輔助方法
    private string GenerateFilePath(string studentEmail, string courseName, string fileName)
    {
        string fileNameWithoutPath = Path.GetFileName(fileName);
        return Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", studentEmail, courseName, fileNameWithoutPath);
    }

    [HttpPost]
    public IActionResult DeleteFile(string studentEmail, string fileName, int enrollmentId)
    {
        // 構造完整檔案路徑
        string uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", studentEmail);
        string filePath = Path.Combine(uploadsDirectory, fileName);

        if (System.IO.File.Exists(filePath))
        {
            try
            {
                // 刪除檔案
                System.IO.File.Delete(filePath);

                // 從資料庫中刪除對應的 Assignment 記錄
                var assignment = _context.Assignments.FirstOrDefault(a => a.FilePath == filePath);
                if (assignment != null)
                {
                    _context.Assignments.Remove(assignment);
                    _context.SaveChanges();
                }

                TempData["SuccessMessage"] = "檔案已成功刪除。";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"刪除檔案時發生錯誤：{ex.Message}";
            }
        }
        else
        {
            TempData["ErrorMessage"] = "檔案不存在，無法刪除。";
        }

        // 返回至作業詳細頁面
        return RedirectToAction("ViewAssignments", new { enrollmentId });
    }
}
