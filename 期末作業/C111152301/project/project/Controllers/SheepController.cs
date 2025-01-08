using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using project.Data;
using project.Models;
using CsvHelper.Configuration;
using System.IO;
using System.Security.Claims;
using X.PagedList;
using X.PagedList.Extensions;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace project.Controllers
{
    public class SheepController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly int _pageSize = 150;
        [HttpPost]
        [Authorize(Roles = "Member, Manager, Admin")]
        public async Task<IActionResult> ImportCsv(IFormFile csvFile)
        {
            if (csvFile == null || csvFile.Length == 0)
            {
                TempData["Error"] = "請選擇檔案上傳";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                using var reader = new StreamReader(csvFile.OpenReadStream());

                // Basic validation and preprocessing
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    var values = line.Split(',');
                    if (values[0] == "marketID")
                    {
                        continue;
                    }
                    
                    var sheep_data = new Sheep
                    {
                        Date = DateTime.Parse(values[3]),
                        Area = values[1],
                        MarketName = values[2],
                        ProductName = values[5],
                        Num = int.Parse(values[7]),
                        AvgWeight = int.Parse(values[8]),
                        AvgPrice = int.Parse(values[9]),
                        HighestPrice = int.Parse(values[10]),
                    };


                    // Bulk insert valid records
                    await _context.Sheep.AddRangeAsync(sheep_data);
                    await _context.SaveChangesAsync();
                    
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error processing file: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }

        public SheepController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string sortOrder, string searchArea,
        string searchMarketName, string searchProductName, int? page)
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
            if (!string.IsNullOrEmpty(searchArea))
            {
                sheepQuery = sheepQuery.Where(s => s.Area == searchArea);
            }

            if (!string.IsNullOrEmpty(searchMarketName))
            {
                sheepQuery = sheepQuery.Where(s => s.MarketName == searchMarketName);
            }

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
        public async Task<IActionResult> Charts()
        {
            try
            {
                // 取得所有數據
                var sheepData = await _context.Sheep.ToListAsync();

                // 準備價格趨勢數據
                var priceData = sheepData
                    .Select(s => new
                    {
                        productName = s.ProductName,
                        area = s.Area,
                        date = s.Date.ToString("yyyy-MM-dd"),
                        avgPrice = s.AvgPrice,
                        num = s.Num
                    })
                    .OrderBy(x => x.date)
                    .ToList();

                // 準備羊隻種類銷售比例數據
                var productRatioData = sheepData
                    .GroupBy(s => s.ProductName)
                    .Select(g => new
                    {
                        name = g.Key,
                        value = g.Sum(s => s.Num)
                    })
                    .ToList();

                // 準備地區銷售比例數據
                var areaRatioData = sheepData
                    .GroupBy(s => s.Area)
                    .Select(g => new
                    {
                        area = g.Key,
                        value = g.Sum(s => s.Num)
                    })
                    .ToList();

                // 將數據序列化時設置日期格式
                var jsonSettings = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };

                // 將數據傳遞給視圖
                ViewBag.PriceData = JsonSerializer.Serialize(priceData, jsonSettings);
                ViewBag.ProductRatioData = JsonSerializer.Serialize(productRatioData, jsonSettings);
                ViewBag.AreaRatioData = JsonSerializer.Serialize(areaRatioData, jsonSettings);

                return View();
            }
            catch (Exception ex)
            {
                // 加入錯誤日誌
                //_logger.LogError(ex, "Error generating charts");
                return View("Error");
            }
        }
        // GET: Sheep/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sheep = await _context.Sheep
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sheep == null)
            {
                return NotFound();
            }

            return View(sheep);
        }
        [Authorize(Roles = "Member, Manager, Admin")]
        // GET: Sheep/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sheep/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Member, Manager, Admin")]
        public async Task<IActionResult> Create([Bind("Id,Date,Area,MarketName,ProductName,Num,AvgWeight,HighestPrice,AvgPrice")] Sheep sheep)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sheep);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sheep);
        }
        [Authorize(Roles = "Manager, Admin")]
        // GET: Sheep/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sheep = await _context.Sheep.FindAsync(id);
            if (sheep == null)
            {
                return NotFound();
            }
            return View(sheep);
        }

        // POST: Sheep/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Area,MarketName,ProductName,Num,AvgWeight,HighestPrice,AvgPrice")] Sheep sheep)
        {
            if (id != sheep.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sheep);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SheepExists(sheep.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(sheep);
        }
        [Authorize(Roles = "Admin")]
        // GET: Sheep/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sheep = await _context.Sheep
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sheep == null)
            {
                return NotFound();
            }

            return View(sheep);
        }

        // POST: Sheep/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sheep = await _context.Sheep.FindAsync(id);
            if (sheep != null)
            {
                _context.Sheep.Remove(sheep);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SheepExists(int id)
        {
            return _context.Sheep.Any(e => e.Id == id);
        }
    }
}
