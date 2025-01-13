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
            // �O�d�j�M�Ѽƪ���
            ViewData["CurrentSort"] = sortOrder;
            ViewData["CurrentSearchArea"] = searchArea;
            ViewData["CurrentSearchMarketName"] = searchMarketName;
            ViewData["CurrentSearchProductName"] = searchProductName;

            // �ƧǰѼ�
            ViewData["DateSortParam"] = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewData["AreaSortParam"] = sortOrder == "Area" ? "area_desc" : "Area";
            ViewData["PriceSortParam"] = sortOrder == "Price" ? "price_desc" : "Price";
            ViewData["ProductNameSortParam"] = sortOrder == "ProductName" ? "product_desc" : "ProductName";

            var sheepQuery = from s in _context.Sheep
                             select s;

            // �j�M�޿�
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

            // �Ƨ��޿�
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

            // �]�w�ثe�����X
            int pageNumber = (page ?? 1);

            // ����d�ߨ��ഫ���C��
            var sheepList = await sheepQuery.AsNoTracking().ToListAsync();

            // �إߤ������G
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
