using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class RaceController : Controller
    {
        private readonly ApplicationDbContext _context;
        public RaceController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()    //contorler
        {
            List<Race> race = _context.Races.ToList();   //model
            return View(race);     //view
        }
        public IActionResult Detail(int id)
        {
            Race race = _context.Races.Include(a => a.Address).FirstOrDefault(c => c.Id == id);
            return View(race);
        }

    }
}
