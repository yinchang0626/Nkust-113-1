using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ClubController : Controller
    {
        //private readonly ApplicationDbContext _context;
        private readonly IClubRepository _clubRepository;
        public ClubController(IClubRepository clubRepository)
        {
            //_context = context;   // NO use this
            _clubRepository = clubRepository;
        }
        public async Task<IActionResult> Index()    //contorler
        {
            //List<Club> clubs = _context.Clubs.ToList();   //model
            IEnumerable<Club> clubs = await _clubRepository.GetAll();
            return View(clubs);     //view
        }
        public async Task<IActionResult> Detail(int id)
        {
            //Club club = _context.Clubs.Include(a => a.Address).FirstOrDefault(c => c.Id == id);
            Club club = await _clubRepository.GetByIdAsync(id);
            return View(club); 
        }
    }
}
