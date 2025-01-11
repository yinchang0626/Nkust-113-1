using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TouristSpotWeb.Models;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;  // 引入 EF Core 的命名空間

namespace TouristSpotWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly Sightseeing_spotsContext _context; // 注入 DbContext

        // 注入 IHttpClientFactory 來創建 HttpClient 實例
        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory, Sightseeing_spotsContext context)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _context = context; // 註入 ApplicationDbContext 以進行資料庫操作
        }

        public IActionResult Index()
        {
            return View();
        }

        // 顯示景點詳細資料
        public async Task<IActionResult> TouristSpotDetails(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound(); // 如果 id 不存在則返回 404
            }

            // 創建 HttpClient 實例
            var client = _httpClientFactory.CreateClient();

            // 發送 GET 請求以獲取景點詳細資料
            var response = await client.GetAsync($"https://localhost:7015/api/TouristSpot/{id}");

            // 如果請求失敗，返回 404 頁面
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            // 解析 API 返回的 JSON 資料
            var spotData = await response.Content.ReadFromJsonAsync<TouristSpots>();

            // 將資料傳遞給視圖
            return View(spotData);
        }

        // 顯示景點管理頁面
        //[Authorize] // 只有登入的使用者可以進入此方法
        public IActionResult Manage(string id)
        {
            // 檢查用戶是否已登入
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                Console.WriteLine("User not logged in");
                // 如果沒有登入，重定向至登入頁面
                return RedirectToAction("Login", "Account");
            }

            var spot = _context.TouristSpots.FirstOrDefault(s => s.Id == id); // 從資料庫查找景點
            if (spot == null)
            {
                return NotFound();
            }
            return View(spot);
        }

        // 更新景點資料
        [HttpPost]
        public IActionResult Update(TouristSpots spot)
        {
            if (ModelState.IsValid)
            {
                var existingSpot = _context.TouristSpots.FirstOrDefault(s => s.Id == spot.Id);
                if (existingSpot == null)
                {
                    return NotFound();
                }

                // 更新景點的屬性
                existingSpot.Name = spot.Name;
                existingSpot.Zone = spot.Zone;
                existingSpot.Toldescribe = spot.Toldescribe;
                existingSpot.Description = spot.Description;
                existingSpot.Tel = spot.Tel;
                existingSpot.Address = spot.Address;
                existingSpot.Zipcode = spot.Zipcode;
                existingSpot.Region = spot.Region;
                existingSpot.Town = spot.Town;
                existingSpot.Travellinginfo = spot.Travellinginfo;
                existingSpot.Opentime = spot.Opentime;
                existingSpot.Px = spot.Px;
                existingSpot.Py = spot.Py;
                existingSpot.Website = spot.Website;
                existingSpot.Parkinginfo = spot.Parkinginfo;
                existingSpot.Ticketinfo = spot.Ticketinfo;
                existingSpot.Remarks = spot.Remarks;

                _context.SaveChanges(); // 保存更新

                return RedirectToAction("TouristSpotDetails", new { id = spot.Id }); // 返回詳細頁
            }

                return View("Manage", spot); // 指定顯示 "Manage" 視圖而不是 "Update"
        }

        //搜尋景點
        public IActionResult Search(string keyword, string region, string town)
        {
            // 預設搜尋結果為 null 或空
            ViewBag.SearchResults = null;

            // 取得區域、城鎮清單，並傳遞給視圖
            ViewBag.RegionList = _context.TouristSpots
                                          .Select(s => s.Region)
                                          .Distinct()
                                          .ToList();

            ViewBag.TownList = _context.TouristSpots
                                         .Select(s => s.Town)
                                         .Distinct()
                                         .ToList();

            // 只有在關鍵字、區域或城鎮有值的情況下才執行搜尋
            if (!string.IsNullOrEmpty(keyword) || !string.IsNullOrEmpty(region) || !string.IsNullOrEmpty(town))
            {
                var query = _context.TouristSpots.AsQueryable();

                // 根據關鍵字過濾 Description
                if (!string.IsNullOrEmpty(keyword))
                {
                    query = query.Where(s => s.Description.Contains(keyword));
                }

                // 根據區域過濾
                if (!string.IsNullOrEmpty(region))
                {
                    query = query.Where(s => s.Region.Contains(region));
                }

                //// 根據城鎮過濾
                //if (!string.IsNullOrEmpty(town))
                //{
                //    query = query.Where(s => s.Town.Contains(town));
                //}

                // 執行查詢並獲得結果
                ViewBag.SearchResults = query.ToList();
                Console.WriteLine(ViewBag.SearchResults.Count);
            }

            return View();
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
