using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using System.Formats.Asn1;
using ExtractDataCSV.Pages.Shared;
using CsvHelper.Configuration;
using Newtonsoft.Json;

namespace ExtractDataCSV.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
        [BindProperty]
        public IFormFile UploadedFile { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (UploadedFile == null || UploadedFile.Length == 0)
            {
                ModelState.AddModelError("UploadedFile", "Please select a file to upload.");
                return Page();
            }

            var result = new List<DrunkDrivingStats>();

            using (var reader = new StreamReader(UploadedFile.OpenReadStream()))
            {
                // Skip the header
                await reader.ReadLineAsync();

                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    var values = line.Split(',');
                    if (!int.TryParse(values[2],out _) && !int.TryParse(values[3], out _))
                    {
                        continue; // skip header if got 2
                    }
                    if (values.Length == 6)
                    {
                        result.Add(new DrunkDrivingStats
                        {
                            Year = values[0],
                            A1Count = int.Parse(values[1]),
                            A2Count = int.Parse(values[2]),
                            Dead = int.Parse(values[3]),
                            A1Hurt = int.Parse(values[4]),
                            A2Hurt = int.Parse(values[5])
                        });
                        
                    }
                }
            }
            var jsonResult = JsonConvert.SerializeObject(result);
            System.Diagnostics.Debug.WriteLine(result.Count);
            return RedirectToPage("Index1", new { passedObject = jsonResult });
        }
    }
}
