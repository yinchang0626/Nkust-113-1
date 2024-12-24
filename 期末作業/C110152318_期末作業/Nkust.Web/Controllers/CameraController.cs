using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Nkust.Web.Data;
using Nkust.Web.Entities;

namespace Nkust.Web.Controllers
{
    public class CameraController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CameraController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? policeOfficeId, int? policeStationId)
        {
            var query = _context.Cameras.AsQueryable();

            if (policeOfficeId != null)
            {
                query = query.Where(x => x.PoliceOfficeId == policeOfficeId);
            }

            if (policeStationId != null)
            {
                query = query.Where(x => x.PoliceStationId == policeStationId);
            }

            query = query.Skip(10).Take(10);

            query.Count();


            return View(query.ToList());
        }

        public async Task<IActionResult> Seeding()
        {
            if (_context.Cameras.Any())
            {
                return View();
            }

            var jsonFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "Cameras.json");

            var jsonString = System.IO.File.ReadAllText(jsonFilePath);

            var items = System.Text.Json.JsonSerializer.Deserialize<List<Camera>>(jsonString);

            items?.ForEach(item =>
            {
                _context.Cameras.Add(item);
            });

            await _context.SaveChangesAsync();

            return View();
        }

        public async Task<IActionResult> PoliceOfficeSeeding()
        {
            if (_context.PoliceStation.Any())
            {
                return View();
            }
            //var jsonFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "Cameras.json");
            //var jsonString = System.IO.File.ReadAllText(jsonFilePath);
            var items = _context.Cameras.ToList();//System.Text.Json.JsonSerializer.Deserialize<List<Camera>>(jsonString);

            var stationGroups = items?
                .GroupBy(x => x.PoliceOffice)
                .ToList();

            Console.WriteLine("hello world");

            foreach (var group in stationGroups!)
            {
                var policeOffice = new PoliceOffice()
                {
                    DisplayName = group.Key
                };
                _context.PoliceOffices.Add(policeOffice);
                await _context.SaveChangesAsync();

                foreach (var item in group.ToList())
                {
                    item.PoliceOfficeId = policeOffice.Id;
                }
            };

            await _context.SaveChangesAsync();

            return View();
        }

        public async Task<IActionResult> PoliceStationSeeding()
        {
            if (_context.PoliceStation.Any())
            {
                return View();
            }

            var items = _context.Cameras.ToList();

            var stationGroups = items?
                .GroupBy(x => x.PoliceStation)
                .ToList();

            foreach (var group in stationGroups!)
            {
                var policeStation = new PoliceStation()
                {
                    DisplayName = group.Key,
                    PoliceOfficeId = group.First().PoliceOfficeId
                };
                _context.PoliceStation.Add(policeStation);


                await _context.SaveChangesAsync();


                foreach (var item in group)
                {
                    item.PoliceStationId = policeStation.Id;
                }
            };

            await _context.SaveChangesAsync();

            return View();
        }
    }
}
