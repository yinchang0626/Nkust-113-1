using System.Diagnostics;
using final_project.Models;
using Microsoft.AspNetCore.Mvc;

namespace final_project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        // 登錄處理
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            // 登錄邏輯，例如檢查帳號和密碼是否正確
            if (IsValidLogin(email, password))
            {
                // 登錄成功，重定向到首頁或儀表板
                return RedirectToAction("Index");
            }
            else
            {
                // 登錄失敗，返回首頁或顯示錯誤
                ViewBag.ErrorMessage = "Invalid login attempt.";
                return View("Index");
            }
        }

        private bool IsValidLogin(string email, string password)
        {
            // 這裡進行登錄驗證邏輯（例如與數據庫比對）
            return true; // 假設總是成功
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
