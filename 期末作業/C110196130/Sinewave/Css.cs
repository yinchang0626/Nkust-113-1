#nullable enable
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceStack;

namespace Sinewave;

public static class Css
{
    public const string H1 = "text-3xl font-bold tracking-tight text-gray-900 dark:text-gray-50 sm:text-4xl";
    public const string H2 = "text-3xl md:text-4xl font-bold tracking-tighter leading-tight md:pr-8 whitespace-nowrap pt-8";
    public const string H3 = "mb-4 text-2xl font-semibold text-gray-900 dark:text-gray-100 leading-tight";
    public const string H4 = "text-xl font-semibold text-gray-900 dark:text-gray-100 leading-tight";
    public const string Link = "font-semibold text-indigo-600 dark:text-indigo-300 hover:text-indigo-500 dark:hover:text-indigo-400";
    public const string LinkUnderline = "underline hover:text-success duration-200 transition-colors";
    public const string PrimaryButton = "cursor-pointer inline-flex justify-center rounded-md border border-transparent py-2 px-4 text-sm font-medium shadow-sm focus:outline-none focus:ring-2 focus:ring-offset-2 dark:ring-offset-black focus:ring-2 focus:ring-offset-2 text-white bg-indigo-600 hover:bg-indigo-700 focus:ring-indigo-500 dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800";
    public const string SecondaryButton = "cursor-pointer inline-flex justify-center rounded-md border border-gray-300 py-2 px-4 text-sm font-medium shadow-sm focus:outline-none focus:ring-2 focus:ring-offset-2 bg-white dark:bg-gray-800 border-gray-300 dark:border-gray-600 text-gray-700 dark:text-gray-400 dark:hover:text-white hover:bg-gray-50 dark:hover:bg-gray-700 focus:ring-indigo-500 dark:focus:ring-indigo-600 dark:ring-offset-black";
    public const string DangerButton = "cursor-pointer inline-flex justify-center rounded-md border border-transparent py-2 px-4 text-sm font-medium shadow-sm focus:outline-none focus:ring-2 focus:ring-offset-2 dark:ring-offset-black focus:ring-red-300 text-white bg-red-600 hover:bg-red-700 focus:ring-red-500 dark:bg-red-600 dark:hover:bg-red-700 dark:focus:ring-red-500 dark:ring-offset-black";
    public const string LabelClasses = "block text-sm font-medium text-gray-700 dark:text-gray-300";
    public static string InputText => CssUtils.Tailwind.Input("");
    public const string AlertDanger = "mb-2 p-2 px-4 flex items-center rounded text-danger bg-red-100 font-semibold";
    public const string AlertSuccess = "mb-2 p-2 px-4 flex items-center rounded text-success bg-green-100 font-semibold";
}

public static class CssHtmlExtensions
{
    public const string BaseUrl = "https://github.com/NetCoreTemplates/mvc/blob/main";

    public static IHtmlContent SrcComponent(this IHtmlHelper html, string path, string? size = null, string? cls = null) =>
        html.SrcLink($"/Sinewave/wwwroot/mjs/components/{path}", size, cls);
     
    public static IHtmlContent SrcPageView(this IHtmlHelper html, string path, string? size = null, string? cls = null) =>
        html.SrcLink($"/Sinewave/Views/{path}", size, cls);
     
    public static IHtmlContent SrcPageMjs(this IHtmlHelper html, string path, string? size = null, string? cls = null) =>
        html.SrcLink($"/Sinewave/wwwroot/pages/{path}", size, cls);
   
    public static IHtmlContent SrcLink(this IHtmlHelper html, string path, string? size = null, string? cls = null)
    {
        size ??= "w-8 h-8";
        cls ??= "text-slate-300 hover:text-slate-400 dark:text-slate-600 dark:hover:text-slate-500";
        return html.Raw($"<a href=\"{BaseUrl + path}\" title=\"Source Code\"><svg class=\"{size} {cls}\" xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 24 24\"><path fill=\"none\" stroke=\"currentColor\" stroke-linecap=\"round\" stroke-linejoin=\"round\" stroke-width=\"2\" d=\"m10 20l4-16m4 4l4 4l-4 4M6 16l-4-4l4-4\"/></svg></a>");
    }
}
