using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace FinalProject.Controllers
{
    public class ManagerController : Controller
    {
        private readonly userContext _userContext;

        public ManagerController(userContext userContext)
        {
            _userContext = userContext;
        }
        public IActionResult HotelManage()
        {
            var hotel = _userContext.hotels.ToList();
            return View(hotel);
        }
        public IActionResult MemberManage()
        {
            var member = _userContext.members.ToList();
            return View(member);
        }
        public IActionResult CreateHotel()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateHotel(hotels hotel)
        {
            _userContext.hotels.Add(hotel);
            _userContext.SaveChanges();
            return RedirectToAction("HotelManage");
        }

        public ActionResult EditHotel(int id)
        {
            var hotel = _userContext.hotels.Where(m => m.id == id).FirstOrDefault();
            return View(hotel);
        }

        [HttpPost]
        public ActionResult EditHotel(hotels hotel)
        {
            var hotelData = _userContext.hotels.Where(m => m.id == hotel.id).FirstOrDefault();
            hotelData._class = hotel._class;
            hotelData.star = hotel.star;
            hotelData.name = hotel.name;
            hotelData.tel = hotel.tel;
            hotelData.lng = hotel.lng;
            hotelData.lat = hotel.lat;
            _userContext.SaveChanges();

            return RedirectToAction("HotelManage");
        }
        public IActionResult DeleteHotel(int id)
        {
            var hotel = _userContext.hotels.Where(m => m.id == id).FirstOrDefault();
            _userContext.hotels.Remove(hotel);
            _userContext.SaveChanges();

            return RedirectToAction("HotelManage");
        }

        public IActionResult DetailHotel(int id)
        {
            var hotel = _userContext.hotels.Where(m => m.id == id).FirstOrDefault();

            return View(hotel);
        }
    }
}
