using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

public class SourcesController : Controller
{
    public IActionResult Index(){
      return View("_404");
    }
}
