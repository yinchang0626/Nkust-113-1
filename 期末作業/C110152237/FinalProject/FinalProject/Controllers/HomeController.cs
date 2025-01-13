using System.Diagnostics;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly userContext _userContext;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, userContext userContext)
        {
            _logger = logger;
            _userContext = userContext;
        }

        public IActionResult Index()
        {
            var hotel = _userContext.hotels.ToList();
            return View(hotel);
        }

        public IActionResult Detail()
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
