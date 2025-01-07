namespace WebApp.Models;

public static class CategoriesRepository
{
   private static List<Category> _categories =
   [
      new Category()
      {
         CategoryId = 1,
         Name = "Fruits",
         Description = "Provides vitamins and minerals"
      },
      new Category()
      {
         CategoryId = 2,
         Name = "Carbohydrates",
         Description = "Provides energy"
      },
      new Category()
      {
         CategoryId = 3,
         Name = "Protein",
         Description = "For growth and tissue repair"
      },new Category()
   
   ];

   public static void AddCategory(Category category)
   {
      var maxId = _categories.Max(c => c.CategoryId);
      category.CategoryId = maxId + 1;
      _categories.Add(category);
   }

   public static List<Category> getCategories() => _categories;

   public static Category? GetCategoryById(int categoryId)
   {
      var category = _categories.FirstOrDefault(c => c.CategoryId == categoryId);

      // return a copy of the category and not the actual  object in the collect
      if (category != null)
      {
         return new Category()
         {
            CategoryId = category.CategoryId,
            Name = category.Name,
            Description = category.Description
         };
      }

      return category;
   }

   public static bool DeleteCategory(int categoryId)
   {
      var totalDeleted = _categories.RemoveAll(c => c.CategoryId == categoryId);

      if (totalDeleted == 0)
      {
         return false;
      }
      return true;
   }

   public static Category? UpdateCategory(int categoryId, Category category)
   {
      var categoryToUpdate = _categories.Where(c => c.CategoryId == categoryId).First();

      //  var categoryToUpdate = GetCategoryById(categoryId);

      if (categoryToUpdate is null)
      {
         return null;
      }

      categoryToUpdate.CategoryId = categoryId;
      categoryToUpdate.Description = category.Description;
      categoryToUpdate.Name = category.Name;

      return new Category()
      {
         CategoryId = categoryToUpdate.CategoryId,
         Description = categoryToUpdate.Description,
         Name = categoryToUpdate.Name
      };
   }
}
