using Kcg.Dots;
using Kcg.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kcg.Controllers
{
    public class News2Controller : Controller
    {
        private readonly KcgContext _context;

        public News2Controller(KcgContext context)
        {
            _context = context;
        }

        public IActionResult Index(string keyword)
        {
            var result = from a in _context.News
                         join b in _context.Department on a.DepartmentId equals b.DepartmentId
                         join c in _context.Employee on a.UpdateEmployeeId equals c.EmployeeId
                         select new NewsDto
                         {
                             Click = a.Click,
                             Enable = a.Enable,
                             EndDateTime = a.EndDateTime,
                             NewsId = a.NewsId,
                             StartDateTime = a.StartDateTime,
                             Title = a.Title,
                             UpdateDateTime = a.UpdateDateTime,
                             UpdateEmployeeName = c.Name,
                             DepartmentName = b.Name
                         };

            if (!string.IsNullOrEmpty(keyword))  //這邊再進行關鍵字過濾
            {
                result = result.Where(x => x.Title.Contains(keyword));
            }

            return View(result.ToList());
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewsCreateDto news)
        {
            if (ModelState.IsValid)
            {
                News insert = new News()
                {
                    Title = news.Title,
                    Contents = news.Contents,
                    DepartmentId = news.DepartmentId,
                    StartDateTime = news.StartDateTime,
                    EndDateTime = news.EndDateTime,
                    Click = 0,
                    Enable = true,
                    InsertEmployeeId = 1,
                    UpdateEmployeeId = 1
                };

                _context.News.Add(insert);

                await _context.SaveChangesAsync();

                foreach (var item in news.Files)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", item.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await item.CopyToAsync(stream);
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            return View(news);
        }
    }
}
