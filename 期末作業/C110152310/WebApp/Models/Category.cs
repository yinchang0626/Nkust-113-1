using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class Category
{
   public int CategoryId { get; set; }

   [Required]
   public string Name { get; set; } = string.Empty;

   [MinLength(5, ErrorMessage ="Length cannot be less than 5 letters")]
  // [EnumDataType(typeof(Desc),ErrorMessage = $"Accepted values are Yes and No ") ]

   public string Description { get; set; } = string.Empty;
}

public enum Desc {
   Yes,
   No
}