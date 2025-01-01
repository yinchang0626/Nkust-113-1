using final_project.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

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
        // 從資料庫查詢 Enrollment 資料
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
        var enrollment = _context.Enrollments
            .Include(e => e.Assignments)
            .Include(e => e.Course)
            .FirstOrDefault(e => e.Id == enrollmentId);

        if (enrollment == null)
        {
            TempData["ErrorMessage"] = "找不到該課程的作業資料。";
            return RedirectToAction("Index", "Home");
        }

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

        var enrollment = await _context.Enrollments
            .Include(e => e.Course)
            .Include(e => e.Student)  // 確保 Student 也被載入
            .FirstOrDefaultAsync(e => e.Id == enrollmentId);

        if (enrollment == null)
        {
            TempData["ErrorMessage"] = "無效的報名資料。";
            return RedirectToAction("Index", "Home");
        }

        // 取得原始檔案的副檔名
        string fileExtension = Path.GetExtension(file.FileName);

        // 取得原始檔案名稱（不包含副檔名）
        string originalFileName = Path.GetFileNameWithoutExtension(file.FileName);

        // 取得當前日期
        string currentDate = DateTime.Now.ToString("yyyyMMdd_HHmmss");

        // 使用 GUID + 原始檔案名稱 + 副檔名組合成唯一的檔案名稱
        string uniqueFileName = currentDate + "_" + originalFileName + fileExtension;

        // 取得學生的 email 和課程名稱
        string studentEmail = enrollment.Student?.Email ?? "未提供電子郵件";
        string courseName = enrollment.Course?.Name ?? "未提供課程名稱";

        // 設定儲存路徑，使用學生 email 和課程名稱作為路徑
        string uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", studentEmail, courseName);

        // 確保資料夾存在
        Directory.CreateDirectory(uploadsDirectory);

        // 儲存文件（這裡的檔案名稱為 uniqueFileName，不包含路徑資訊）
        string filePath = Path.Combine(uploadsDirectory, uniqueFileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // 創建 Assignment 實體並儲存至資料庫
        var assignment = new Assignment
        {
            EnrollmentId = enrollmentId,
            FilePath = filePath,  // 儲存完整的檔案路徑
            UploadedAt = DateTime.Now
        };

        _context.Assignments.Add(assignment);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "作業上傳成功！";
        return RedirectToAction("StudentProgress", "Enrollment");
    }


    // 下載作業檔案
    public Task<IActionResult> DownloadFile(string studentEmail, string courseName, string fileName)
    {
        // 去除路徑，僅保留檔案名稱
        string fileNameWithoutPath = Path.GetFileName(fileName);

        // 生成檔案路徑，根據學生的 email 和課程名稱來構建路徑
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", studentEmail, courseName, fileNameWithoutPath);

        // 檢查檔案是否存在
        if (!System.IO.File.Exists(filePath))
        {
            return Task.FromResult<IActionResult>(NotFound()); // 如果檔案不存在，返回 404
        }

        // 傳回檔案下載
        var fileBytes = System.IO.File.ReadAllBytes(filePath);
        var fileExtension = Path.GetExtension(fileNameWithoutPath);
        var contentType = GetContentType(fileExtension); // 根據檔案類型獲取 MIME 類型
        return Task.FromResult<IActionResult>(File(fileBytes, contentType, fileNameWithoutPath));
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
            // 你可以根據需要擴展更多檔案類型
            default: return "application/octet-stream";
        }
    }
}
