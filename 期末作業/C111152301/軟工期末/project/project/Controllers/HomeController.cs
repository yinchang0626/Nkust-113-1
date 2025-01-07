using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project.Data;
using project.Models;
using X.PagedList.Extensions;

namespace project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly int _pageSize = 150;
        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        { 
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string sortOrder, string searchArea, string searchMarketName, string searchProductName, int? page)
        {
            // 保留搜尋參數的值
            ViewData["CurrentSort"] = sortOrder;
            ViewData["CurrentSearchArea"] = searchArea;
            ViewData["CurrentSearchMarketName"] = searchMarketName;
            ViewData["CurrentSearchProductName"] = searchProductName;

            // 排序參數
            ViewData["DateSortParam"] = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewData["AreaSortParam"] = sortOrder == "Area" ? "area_desc" : "Area";
            ViewData["PriceSortParam"] = sortOrder == "Price" ? "price_desc" : "Price";
            ViewData["ProductNameSortParam"] = sortOrder == "ProductName" ? "product_desc" : "ProductName";

            var sheepQuery = from s in _context.Sheep
                             select s;

            // 搜尋邏輯
            if (!string.IsNullOrEmpty(searchProductName))
            {
                sheepQuery = sheepQuery.Where(s => s.ProductName == searchProductName);
            }

            // 排序邏輯
            switch (sortOrder)
            {
                case "date_desc":
                    sheepQuery = sheepQuery.OrderByDescending(s => s.Date);
                    break;
                case "Area":
                    sheepQuery = sheepQuery.OrderBy(s => s.Area);
                    break;
                case "area_desc":
                    sheepQuery = sheepQuery.OrderByDescending(s => s.Area);
                    break;
                case "Price":
                    sheepQuery = sheepQuery.OrderBy(s => s.AvgPrice);
                    break;
                case "price_desc":
                    sheepQuery = sheepQuery.OrderByDescending(s => s.AvgPrice);
                    break;
                case "ProductName":
                    sheepQuery = sheepQuery.OrderBy(s => s.ProductName);
                    break;
                case "product_desc":
                    sheepQuery = sheepQuery.OrderByDescending(s => s.ProductName);
                    break;
                default:
                    sheepQuery = sheepQuery.OrderBy(s => s.Date);
                    break;
            }
            // 設定目前的頁碼
            int pageNumber = (page ?? 1);

            // 執行查詢並轉換為列表
            var sheepList = await sheepQuery.AsNoTracking().ToListAsync();

            // 建立分頁結果
            var pagedList = sheepList.ToPagedList(pageNumber, _pageSize);

            return View(pagedList);
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
