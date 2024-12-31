using Microsoft.AspNetCore.Mvc;
using final_project.Models;
using System.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using final_project.Data;
using Microsoft.EntityFrameworkCore;

namespace final_project.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 顯示登入頁面
        public IActionResult Login()
        {
            return View();
        }

        // 處理登入邏輯
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || user.Password != password) // 直接比較密碼
            {
                // 如果登入失敗，將錯誤訊息加入 ModelState
                ModelState.AddModelError("", "無效的帳號或密碼");
                return View();
            }

            //var claims = new List<Claim>
            //{
            //    new Claim(ClaimTypes.Name, user.Name),
            //    new Claim(ClaimTypes.Email, user.Email),
            //    new Claim("UserId", user.Id.ToString())
            //};

            //var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            //var authProperties = new AuthenticationProperties
            //{
            //    IsPersistent = true
            //};

            //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            return RedirectToAction("StudentProgress", "Enrollment", new { studentId = user.Id });
        }

        // 登出邏輯
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string email, string password, string passwordConfirmation)
        {
            // 檢查必要的參數是否為 null 或空值
            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("", "電子郵件地址是必填項");
                return View();
            }

            if (string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "密碼是必填項");
                return View();
            }

            if (string.IsNullOrEmpty(passwordConfirmation))
            {
                ModelState.AddModelError("", "確認密碼是必填項");
                return View();
            }

            // 檢查密碼和確認密碼是否匹配
            if (password != passwordConfirmation)
            {
                ModelState.AddModelError("", "密碼不匹配");
                return View();
            }

            // 檢查該電子郵件是否已經註冊
            if (await _context.Users.AnyAsync(u => u.Email == email))
            {
                ModelState.AddModelError("", "該電子郵件已被註冊");
                return View();
            }

            // 創建新用戶
            var user = new User
            {
                Email = email,
                Password = password, // 不加密密碼
                Name = email.Split('@')[0] // 默認使用電子郵件的前綴作為用戶名
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login", "Account");
        }
    }
}
