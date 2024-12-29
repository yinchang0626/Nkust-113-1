using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Data.Enum;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.Repository;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class RaceController : Controller
    {
        private readonly IRaceRepository _raceRepository;
        private readonly ApplicationDbContext _context;
        public RaceController(IRaceRepository raceRepository)
        {
            _raceRepository = raceRepository;;
        }
        //public IActionResult Index()    //contorler
        //{
        //    List<Race> race = _context.Races.ToList();   //model
        //    return View(race);     //view
        //}
        public IActionResult Detail(int id)
        {
            Race race = _context.Races.Include(a => a.Address).FirstOrDefault(c => c.Id == id);
            return View(race);
        }
        public async Task<IActionResult> Index(int category = -1, int page = 1, int pageSize = 6)
        {
            if (page < 1 || pageSize < 1)
            {
                return NotFound();
            }

            // if category is -1 (All) dont filter else filter by selected category
            var races = category switch
            {
                -1 => await _raceRepository.GetSliceAsync((page - 1) * pageSize, pageSize),
                _ => await _raceRepository.GetRacesByCategoryAndSliceAsync((RacesCategory)category, (page - 1) * pageSize, pageSize),
            };

            var count = category switch
            {
                -1 => await _raceRepository.GetCountAsync(),
                _ => await _raceRepository.GetCountByCategoryAsync((RacesCategory)category),
            };

            var viewModel = new IndexRaceViewModel
            {
                Races = races,
                Page = page,
                PageSize = pageSize,
                TotalRaces = count,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize),
                Category = category,
            };

            return View(viewModel);
        }
        [HttpGet]
        public IActionResult Create()
        {
            //var curUserId = HttpContext.User.GetUserId();
            //var createClubViewModel = new CreateClubViewModel { AppUserId = curUserId };
            //return View(createClubViewModel);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Race race)
        {
            if (ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage); // Debug 驗證錯誤
                }
                return View(race); // 返回表單和錯誤訊息
            }
            _raceRepository.Add(race);
            return View();
        }
    }
}
