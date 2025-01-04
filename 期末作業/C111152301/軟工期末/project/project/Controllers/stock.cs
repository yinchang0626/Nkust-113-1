using Microsoft.AspNetCore.Mvc;

namespace project.Controllers
{
    public class stock : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
