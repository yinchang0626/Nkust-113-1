using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Nkust.Web.Data;
using Nkust.Web.Entities;

namespace Nkust.Web.Controllers
{
    public class CameraController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CameraController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var datas = _context.Cameras.Skip(100).Take(10).ToList();

            return View(datas);
        }

        public async Task<IActionResult> Seeding()
        {
            if (_context.Cameras.Any())
            {
                return View();
            }

            var jsonFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "Cameras.json");

            var jsonString = System.IO.File.ReadAllText(jsonFilePath);

            var items = System.Text.Json.JsonSerializer.Deserialize<List<Camera>>(jsonString);

            items?.ForEach(item =>
            {
                _context.Cameras.Add(item);
            });

            await _context.SaveChangesAsync();

            return View();
        }
    }
}
