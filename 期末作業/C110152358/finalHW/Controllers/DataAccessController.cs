using finalHW.Data;
using finalHW.Models;
using Microsoft.AspNetCore.Mvc;

namespace finalHW.Controllers
{
    public class DataAccessController : Controller
    {

        private readonly DatabaseContext _db;

        public DataAccessController(DatabaseContext db)
        {
            _db = db;
        }

        public IActionResult Index(string? searchTerm)
        {
            // Get the data from the database
            List<DataContent> objList = _db.Datas.ToList();

            // Check if a search term is provided
            if (!string.IsNullOrEmpty(searchTerm))
            {
                // Filter the list based on the search term
                objList = objList.Where(d => d.FirstName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                                          || d.LastName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                                          || d.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                                          || d.Gender.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                                          || d.CompanyName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Pass the search term back to the view to keep it in the input field
            ViewBag.SearchTerm = searchTerm;

            // Return the filtered or full list to the view
            return View(objList);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(DataContent obj)
        {

            _db.Datas.Add(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            DataContent? dataContent = _db.Datas.Find(id);

            if (dataContent == null)
            {
                return NotFound();
            }
            
            return View(dataContent);
        }

        [HttpPost]
        public IActionResult Edit(DataContent obj)
        {

            _db.Datas.Update(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            DataContent? dataContent = _db.Datas.Find(id);

            if (dataContent == null)
            {
                return NotFound();
            }

            return View(dataContent);
        }

        [HttpPost]
        public IActionResult Delete(DataContent obj)
        {

            _db.Datas.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}

