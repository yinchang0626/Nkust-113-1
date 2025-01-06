using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class CategoriesController : Controller
    {
        public IActionResult Index()
        {
            var categories = CategoriesRepository.getCategories();
            return View(categories);
        }

        public IActionResult Edit(int? id)
        {
            // creating an instance of a category
            var category = CategoriesRepository.GetCategoryById(id.HasValue ? (int)id : 0);

            // passing in the model instance to the view in order to be able to use it in the view
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category, int id)
        {
            if (ModelState.IsValid)
            {
                var updatedCategory = CategoriesRepository.UpdateCategory(id, category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        [HttpPost]
        public IActionResult Add(Category category)
        {
            if (ModelState.IsValid)
            {
                CategoriesRepository.AddCategory(category);
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public IActionResult Add()
        {
            return View();
        }

        public IActionResult Delete(int id)
        {
            bool success = CategoriesRepository.DeleteCategory(id);
            if (success)
            {
                return RedirectToAction(nameof(Index));
            }
            return View("Edit");
        }
    }
}
