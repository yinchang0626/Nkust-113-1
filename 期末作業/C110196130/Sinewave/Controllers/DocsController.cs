using Microsoft.AspNetCore.Mvc;
using ServiceStack.Mvc;

namespace Sinewave.Controllers;

public class DocsController(MarkdownPages markdown, IWebHostEnvironment env) : ServiceStackController
{
    [HttpGet]
    [Route("/docs/{slug}")]
    public IActionResult Index(string slug)
    {
        var doc = markdown.GetBySlug(slug);
        if (doc == null)
            return NotFound();
        
        ViewData["Title"] = doc.Title;
        return View(doc);
    }
}
