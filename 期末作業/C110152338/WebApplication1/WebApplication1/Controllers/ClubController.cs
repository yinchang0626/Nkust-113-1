using Microsoft.AspNetCore.Mvc;
using WebApplication1;
using WebApplication1.Data.Enum;
//using WebApplication1.Helpers;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class ClubController : Controller
    {
        //private readonly ApplicationDbContext _context;
        private readonly IClubRepository _clubRepository;
        //private readonly IPhotoService _photoService;
        public ClubController(IClubRepository clubRepository)
        {
            //_context = context;   // NO use this
            _clubRepository = clubRepository;
        }
        //public async Task<IActionResult> Index()    //contorler
        //{
        //    //ApplicationDbContext context
        //    //List<Club> clubs = _context.Clubs.ToList();   //model
        //    IEnumerable<Club> clubs = await _clubRepository.GetAll();
        //    return View(clubs);     //view
        //}

        public async Task<IActionResult> Index(int category = -1, int page = 1, int pageSize = 6)
        {
            if (page < 1 || pageSize < 1)
            {
                return NotFound();
            }

            // if category is -1 (All) dont filter else filter by selected category
            var clubs = category switch
            {
                -1 => await _clubRepository.GetSliceAsync((page - 1) * pageSize, pageSize),
                _ => await _clubRepository.GetClubsByCategoryAndSliceAsync((ClubCategory)category, (page - 1) * pageSize, pageSize),
            };

            var count = category switch
            {
                -1 => await _clubRepository.GetCountAsync(),
                _ => await _clubRepository.GetCountByCategoryAsync((ClubCategory)category),
            };

            var clubViewModel = new IndexClubViewModel
            {
                Clubs = clubs,
                Page = page,
                PageSize = pageSize,
                TotalClubs = count,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize),
                Category = category,
            };

            return View(clubViewModel);
        }

        public async Task<IActionResult> Detail(int id)
        {
            //Club club = _context.Clubs.Include(a => a.Address).FirstOrDefault(c => c.Id == id);
            Club club = await _clubRepository.GetByIdAsync(id);
            return View(club); 
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
        public async Task<IActionResult> Create(Club club)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage); // Debug 驗證錯誤
                }
                return View(club);
            }
            _clubRepository.Add(club);
            return View();
        }
        //    public async Task<IActionResult> Create(CreateClubViewModel clubVM)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            //var result = await _photoService.AddPhotoAsync(clubVM.Image);

        //            var club = new Club
        //            {
        //                Title = clubVM.Title,
        //                Description = clubVM.Description,
        //                //Image = result.Url.ToString(),
        //                ClubCategory = clubVM.ClubCategory,
        //                AppUserId = clubVM.AppUserId,
        //                Address = new Address
        //                {
        //                    Street = clubVM.Address.Street,
        //                    City = clubVM.Address.City,
        //                    State = clubVM.Address.State,
        //                }
        //            };
        //            _clubRepository.Add(club);
        //            return RedirectToAction("Index");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "Photo upload failed");
        //        }

        //        return View(clubVM);
        //    }
    }
}
