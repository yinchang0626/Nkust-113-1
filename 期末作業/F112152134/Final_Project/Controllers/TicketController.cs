using System.Formats.Asn1;
using System.Globalization;
using System.Xml.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using Final_Project.Data;
using Final_Project.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project.Controllers
{
    public class TicketController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Tickets()
        {
            if (_context.Ticket_Deals.Any())
            {
                return View();
            }
            var csvFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Opendata", "E-tickets.csv");
            using (var reader = new StreamReader(csvFilePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true // 確保 CSV 檔案有標題列
            }))
            {
                var items = csv.GetRecords<Ticket_deals>().ToList();
                items?.ForEach(item =>
                {
                    _context.Ticket_Deals.Add(item);
                });

            }
            await _context.SaveChangesAsync();
            return View();
        }
    }
}
