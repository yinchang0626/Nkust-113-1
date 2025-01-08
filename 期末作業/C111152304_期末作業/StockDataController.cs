using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class StockDataController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StockDataController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            var stockDataQuery = from s in _context.StockData
                                 select s;

            if (!string.IsNullOrEmpty(searchString))
            {
                stockDataQuery = stockDataQuery.Where(s => s.SecurityCode.Contains(searchString)
                                                          || s.SecurityName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "securitycode_desc":
                    stockDataQuery = stockDataQuery.OrderByDescending(s => s.SecurityCode);
                    break;
                case "securityname_desc":
                    stockDataQuery = stockDataQuery.OrderByDescending(s => s.SecurityName);
                    break;
                case "tradevalue_desc":
                    stockDataQuery = stockDataQuery.OrderByDescending(s => s.TradeValue);
                    break;
                default:
                    stockDataQuery = stockDataQuery.OrderBy(s => s.SecurityCode);
                    break;
            }

            var stockData = await stockDataQuery
                .AsNoTracking()
                .ToListAsync();

            var distinctStockData = stockData
                .GroupBy(s => new { s.SecurityCode, s.SecurityName })
                .Select(g => g.First())  
                .ToList();

            return View(distinctStockData);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SecurityCode,SecurityName,TradeVolume,TradeValue,OpeningPrice,HighestPrice,LowestPrice,ClosingPrice,PriceDifference,TradeCount")] StockData stockData)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stockData);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stockData);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockData = await _context.StockData.FindAsync(id);
            if (stockData == null)
            {
                return NotFound();
            }
            return View(stockData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SecurityCode,SecurityName,TradeVolume,TradeValue,OpeningPrice,HighestPrice,LowestPrice,ClosingPrice,PriceDifference,TradeCount")] StockData stockData)
        {
            if (id != stockData.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stockData);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockDataExists(stockData.Id))
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
            return View(stockData);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockData = await _context.StockData
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stockData == null)
            {
                return NotFound();
            }

            return View(stockData);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stockData = await _context.StockData.FindAsync(id);
            _context.StockData.Remove(stockData);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StockDataExists(int id)
        {
            return _context.StockData.Any(e => e.Id == id);
        }
    }
}
