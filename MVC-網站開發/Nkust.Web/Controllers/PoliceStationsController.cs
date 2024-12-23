using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Nkust.Web.Data;
using Nkust.Web.Entities;

namespace Nkust.Web.Controllers
{
    public class PoliceStationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PoliceStationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PoliceStations
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PoliceStation
                .Include(p => p.PoliceOffice);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PoliceStations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var policeStation = await _context.PoliceStation
                .Include(p => p.PoliceOffice)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (policeStation == null)
            {
                return NotFound();
            }

            return View(policeStation);
        }

        // GET: PoliceStations/Create
        public IActionResult Create()
        {
            ViewBag.PoliceOffices = _context.PoliceOffices.ToList();
            ViewData["PoliceOfficeId"] = new SelectList(_context.PoliceOffices, "Id", "Id");
            return View();
        }

        // POST: PoliceStations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PoliceOfficeId,DisplayName")] PoliceStation policeStation)
        {
            ViewBag.PoliceOffices = _context.PoliceOffices.ToList();
            if (ModelState.IsValid)
            {
                _context.Add(policeStation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PoliceOfficeId"] = new SelectList(_context.PoliceOffices, "Id", "Id", policeStation.PoliceOfficeId);
            return View(policeStation);
        }

        // GET: PoliceStations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var policeStation = await _context.PoliceStation.FindAsync(id);
            if (policeStation == null)
            {
                return NotFound();
            }
            ViewData["PoliceOfficeId"] = new SelectList(_context.PoliceOffices, "Id", "Id", policeStation.PoliceOfficeId);
            return View(policeStation);
        }

        // POST: PoliceStations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PoliceOfficeId,DisplayName")] PoliceStation policeStation)
        {
            if (id != policeStation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(policeStation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PoliceStationExists(policeStation.Id))
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
            ViewData["PoliceOfficeId"] = new SelectList(_context.PoliceOffices, "Id", "Id", policeStation.PoliceOfficeId);
            return View(policeStation);
        }

        // GET: PoliceStations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var policeStation = await _context.PoliceStation
                .Include(p => p.PoliceOffice)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (policeStation == null)
            {
                return NotFound();
            }

            return View(policeStation);
        }

        // POST: PoliceStations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var policeStation = await _context.PoliceStation.FindAsync(id);
            if (policeStation != null)
            {
                _context.PoliceStation.Remove(policeStation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PoliceStationExists(int id)
        {
            return _context.PoliceStation.Any(e => e.Id == id);
        }
    }
}
