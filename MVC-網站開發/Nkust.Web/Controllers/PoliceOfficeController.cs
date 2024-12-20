using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nkust.Web.Data;

namespace Nkust.Web.Controllers
{
    public class PoliceOfficeController : Controller
    {
        private readonly ApplicationDbContext context;

        public PoliceOfficeController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            var query = context.PoliceOffices
                .Include(e => e.PoliceStations);

            var datas = query.ToList();

            foreach (var data in datas)
            {
                
            }

            return View();
        }
    }
}
