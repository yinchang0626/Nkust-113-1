using Microsoft.AspNetCore.Mvc;
//using TouristSpotWeb.Data;
using TouristSpotWeb.Models;
using System.Security.Cryptography;
using System.Text;

public class AccountController : Controller
{
    private readonly Sightseeing_spotsContext _context;

    public AccountController(Sightseeing_spotsContext context)
    {
        _context = context;
    }

    // 註冊頁面
    [HttpGet]
    public IActionResult Register() => View();

    // 註冊 POST
    [HttpPost]
    public IActionResult Register(string username, string password, string email)
    {
        if (_context.Users.Any(u => u.Username == username))
        {
            ModelState.AddModelError("", "此帳號已被註冊");
            return View();
        }

        var user = new Users
        {
            Username = username,
            PasswordHash = HashPassword(password),
            Email = email
        };
        _context.Users.Add(user);
        _context.SaveChanges();

        // 註冊成功後顯示訊息並導向首頁
        ViewData["SuccessMessage"] = "註冊成功！3秒後跳轉至首頁!";
        return View();
    }

    // 登入頁面
    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        // 查詢使用者是否存在
        var user = _context.Users.FirstOrDefault(u => u.Username == username);

        if (user == null || !VerifyPassword(password, user.PasswordHash))
        {
            // 登入失敗，將錯誤訊息放入 ViewBag 以顯示在前端
            ViewBag.ErrorMessage = "登入失敗，請重新登入";
            return View(); // 返回登入頁面並顯示錯誤訊息
        }

        // 登入成功後，將用戶的 ID 存入 Session
        HttpContext.Session.SetString("UserId", user.Id.ToString());
        HttpContext.Session.SetString("Username", user.Username ?? string.Empty); // 可選：儲存用戶名

        return RedirectToAction("Index", "Home"); // 登入成功後跳轉到首頁
    }



    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    private bool VerifyPassword(string password, string? storedHash)
    {
        if (string.IsNullOrEmpty(storedHash))
        {
            return false; // 如果 storedHash 為 null 或空字串，直接返回 false。
        }

        return HashPassword(password) == storedHash;
    }
}
