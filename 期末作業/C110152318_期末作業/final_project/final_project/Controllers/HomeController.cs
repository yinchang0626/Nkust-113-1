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
            if (User.Identity.IsAuthenticated) // 檢查用戶是否登入
            {
                return RedirectToAction("StudentProgress", "Enrollment"); // 已登入，跳轉到我的課程進度
            }
            return View(); // 如果未登入，顯示歡迎頁面
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
