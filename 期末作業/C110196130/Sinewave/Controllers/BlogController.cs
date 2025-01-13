using Microsoft.AspNetCore.Mvc;
using ServiceStack.Mvc;
using Sinewave.Models; 
namespace Sinewave.Controllers;

public class BlogController(MarkdownBlog blog, IWebHostEnvironment env) : ServiceStackController
{
    [HttpGet]
    [Route("/posts")]
    public IActionResult Index(string? author = null, string? tag = null)
    {
        return View(blog.GetPosts(author, tag));
    }

    [HttpGet]
    [Route("/posts/{slug}")]
    public IActionResult Post(string slug)
    {
        var doc = blog.FindPostBySlug(slug);
        if (doc == null)
            return NotFound();
        if (env.IsDevelopment())
            doc = blog.Load(doc.Path);

        return View(doc);
    }
    [HttpGet]
    [Route("/posts/edit/{slug}")]
    public IActionResult Edit(string slug)
    {
        var doc = blog.FindPostBySlug(slug);
        if (doc == null)
            return NotFound();

        var content = System.IO.File.ReadAllText(doc.Path); // 读取 Markdown 文件内容
        var model = new BlogEditViewModel
        {
            Slug = slug,
            Title = doc.Title,
            Content = content
        };

        return View(model); // 返回一个视图，用户可以在表单中编辑内容
    }

    [HttpPost]
    [Route("/posts/edit/{slug}")]
    public IActionResult Edit(string slug, BlogEditViewModel model)
    {
        var doc = blog.FindPostBySlug(slug);
        if (doc == null)
            return NotFound();

        // 更新文件内容
        System.IO.File.WriteAllText(doc.Path, model.Content);

        return RedirectToAction("Post", new { slug }); // 编辑完成后重定向到该文章页面
    }

    [HttpPost]
    [Route("/posts/delete/{slug}")]
    public IActionResult Delete(string slug)
    {
        var doc = blog.FindPostBySlug(slug);
        if (doc == null)
            return NotFound();

        // 删除文件
        System.IO.File.Delete(doc.Path);

        // 删除成功后，可以直接返回成功状态，不必再重定向
        return Ok();  // 返回成功状态
    }


}
