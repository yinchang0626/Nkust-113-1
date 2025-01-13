using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace FinalProject.Controllers
{
    public class MemberController : Controller
    {
        private readonly userContext _userContext; 
        
        public MemberController(userContext userContext)
        {
            _userContext = userContext;
        }
        public IActionResult Index()
        {
            var hotel = _userContext.hotels.ToList();
            return View(hotel);
        }
        public IActionResult DetailHotel (int id)
        {
            var hotel = _userContext.hotels.Where(m => m.id == id).FirstOrDefault();
            return View(hotel);
        }

    }
}
