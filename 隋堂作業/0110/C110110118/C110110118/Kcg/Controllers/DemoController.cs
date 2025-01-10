using Kcg.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Kcg.Controllers
{
    public class DemoController : Controller
    {
        private readonly KcgContext _kcgContext;

        public DemoController(KcgContext kcgContext) 
        {
            _kcgContext = kcgContext;
        }

        public IActionResult Index()
        {
            TOPMenu model = _kcgContext.TOPMenu.FirstOrDefault();


            return View(model);
        }

    }
}
