using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mvc.Data;
using mvc.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace mvc.Controllers
{
    public class Crime_dataController : Controller
    {
        private readonly ApplicationDbContext _context;

        public Crime_dataController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Crime_data
        public async Task<IActionResult> Index()
        {
            return View(await _context.Crime_data.ToListAsync());
        }

        // GET: Crime_data/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var crime_data = await _context.Crime_data
                .FirstOrDefaultAsync(m => m.Id == id);
            if (crime_data == null)
            {
                return NotFound();
            }

            return View(crime_data);
        }

        // GET: Crime_data/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Crime_data/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Type,Date,Country,Region")] Crime_data crime_data)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ModelState.IsValid)
            {
                crime_data.OwnerID = userId;
                crime_data.Year = crime_data.Date.Year - 1911;
                _context.Add(crime_data);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(crime_data);
        }

        // GET: Crime_data/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var crime_data = await _context.Crime_data.FindAsync(id);
            if (crime_data == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("Admin") && crime_data.OwnerID != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Forbid();
            }

            return View(crime_data);
        }

        // POST: Crime_data/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type,Year,Date,Country,Region,OwnerID")] Crime_data crime_data)
        {
            if (id != crime_data.Id)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!User.IsInRole("Admin") && crime_data.OwnerID != userId)
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(crime_data);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Crime_dataExists(crime_data.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(crime_data);
        }

        // GET: Crime_data/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var crime_data = await _context.Crime_data
                .FirstOrDefaultAsync(m => m.Id == id);
            if (crime_data == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("Admin") && crime_data.OwnerID != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Forbid();
            }

            return View(crime_data);
        }

        // POST: Crime_data/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var crime_data = await _context.Crime_data.FindAsync(id);
            if (crime_data == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("Admin") && crime_data.OwnerID != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Forbid();
            }

            _context.Crime_data.Remove(crime_data);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Crime_dataExists(int id)
        {
            return _context.Crime_data.Any(e => e.Id == id);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UploadCsv(IFormFile csvFile)
        {
            if (csvFile != null && csvFile.Length > 0)
            {
                using (var stream = new StreamReader(csvFile.OpenReadStream()))
                {
                    // Process the CSV file and save data to the database
                    // Example: Read the CSV file line by line
                    while (!stream.EndOfStream)
                    {
                        var line = await stream.ReadLineAsync();
                        var values = line.Split(',');
                        // remove the first line
                        if (values[0] == "案類" || values[0] == "type")
                        {
                            continue;
                        }
                        if (values.Length != 5)
                        {
                            // Invalid data
                            continue;
                        }
                        // Save values to the database
                        // data required to be in the format of "yyyy-MM-dd"
                        var monthint = int.Parse(values[2].Substring(0, 2));
                        var dayint = int.Parse(values[2].Substring(2, 2));
                        if (values[2].Length >= 4 && monthint <= 12 && dayint <= 31 && monthint > 0 && dayint > 0)
                        {
                            var nyear = int.Parse(values[1]) + 1911;
                            var syear = nyear.ToString();
                            var sdate = syear + "-" + values[2].Substring(0, 2) + "-" + values[2].Substring(2, 2);
                            var crime_data = new Crime_data
                            {
                                Type = values[0],
                                Year = int.Parse(values[1]),
                                Date = DateTime.Parse(sdate),
                                Country = values[3],
                                Region = values[4],
                                OwnerID = User.FindFirstValue(ClaimTypes.NameIdentifier)
                            };
                            // Add code to save crime_data to the database
                            _context.Add(crime_data);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            // Handle invalid date format
                            continue;
                        }
                    }
                }
            }

            return RedirectToAction("Index");
        }
    }
}
