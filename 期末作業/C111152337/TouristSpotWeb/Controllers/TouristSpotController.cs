using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TouristSpotWeb.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TouristSpotController : ControllerBase
    {
        private readonly Sightseeing_spotsContext _context; // 宣告資料庫上下文物件

        // 依賴注入資料庫上下文
        public TouristSpotController(Sightseeing_spotsContext context)
        {
            _context = context;
        }

        // GET: api/TouristSpot
        [HttpGet]
        public async Task<IActionResult> GetAllTouristSpots()
        {
            var touristSpots = await _context.TouristSpots
                .Select(s => new { s.Id, s.Name,s.Px,s.Py, s.Toldescribe }) // 只選取 Id 和 Name
                .ToListAsync();

            return Ok(touristSpots); // 返回 200 OK 與資料
        }

        // GET: api/TouristSpot/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTouristSpotDetails(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Invalid ID"); // 返回 400 Bad Request
            }

            var touristSpot = await _context.TouristSpots
                .FirstOrDefaultAsync(m => m.Id == id);

            if (touristSpot == null)
            {
                return NotFound(); // 返回 404 Not Found
            }

            return Ok(touristSpot); // 返回 200 OK 與詳細資料
        }
    }
}
