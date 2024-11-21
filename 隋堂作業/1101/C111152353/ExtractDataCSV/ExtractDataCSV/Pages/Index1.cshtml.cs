using ExtractDataCSV.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace ExtractDataCSV.Pages
{
    public class Index1Model : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public List<DrunkDrivingStats> Data { get; set; }
        public IActionResult OnGet(string passedObject)
        {
            if (string.IsNullOrEmpty(passedObject))
            {
                return BadRequest("Passed object is null or empty.");
            }

            try
            {
                Data = JsonConvert.DeserializeObject<List<DrunkDrivingStats>>(passedObject);
                if (Data == null)
                {
                    return NotFound();
                }
            }
            catch (JsonException)
            {
                return BadRequest("Invalid JSON format.");
            }

            return Page();
        }

    }
}
