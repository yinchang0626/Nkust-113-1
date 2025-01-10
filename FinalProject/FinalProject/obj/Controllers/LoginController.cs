using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace FinalProject.Controllers
{
    public class LoginController : Controller
    {
        private readonly userContext _userContext;
        

        public LoginController(userContext userContext)
        {
            _userContext = userContext;
        }

        public IActionResult Index()
        {
            var member = _userContext.members.ToList();
            return View(member);
        }
        public IActionResult Signup()
        {
            var member = _userContext.members.ToList();
            return View();
        }
    }
}
