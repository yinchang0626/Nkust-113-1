namespace Sinewave.Models;

public class BlogEditViewModel
{
    /// <summary>
    /// 唯一标识符，例如 Slug
    /// </summary>
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// 博客文章的标题
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 博客文章的内容（Markdown 格式）
    /// </summary>
    public string Content { get; set; } = string.Empty;
}
