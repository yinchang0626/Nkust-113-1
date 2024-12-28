using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class ClubController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
