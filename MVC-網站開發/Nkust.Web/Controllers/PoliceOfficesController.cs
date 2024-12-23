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
    public class PoliceOfficesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PoliceOfficesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PoliceOffices
        public async Task<IActionResult> Index()
        {
            return View(await _context.PoliceOffices.ToListAsync());
        }

        // GET: PoliceOffices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var policeOffice = await _context.PoliceOffices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (policeOffice == null)
            {
                return NotFound();
            }

            return View(policeOffice);
        }

        // GET: PoliceOffices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PoliceOffices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DisplayName")] PoliceOffice policeOffice)
        {


            if (ModelState.IsValid)
            {
                _context.Add(policeOffice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(policeOffice);
        }

        // GET: PoliceOffices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var policeOffice = await _context.PoliceOffices.FindAsync(id);
            if (policeOffice == null)
            {
                return NotFound();
            }
            return View(policeOffice);
        }

        // POST: PoliceOffices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DisplayName")] PoliceOffice policeOffice)
        {
            if (id != policeOffice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(policeOffice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PoliceOfficeExists(policeOffice.Id))
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
            return View(policeOffice);
        }

        // GET: PoliceOffices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var policeOffice = await _context.PoliceOffices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (policeOffice == null)
            {
                return NotFound();
            }

            return View(policeOffice);
        }

        // POST: PoliceOffices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var policeOffice = await _context.PoliceOffices.FindAsync(id);
            if (policeOffice != null)
            {
                _context.PoliceOffices.Remove(policeOffice);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PoliceOfficeExists(int id)
        {
            return _context.PoliceOffices.Any(e => e.Id == id);
        }
    }
}
