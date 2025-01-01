using Microsoft.AspNetCore.Mvc;
using final_project.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using final_project.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace final_project.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
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
            var users = await _context.Users.ToListAsync();
            var user = users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.Ordinal));

            if (user == null)
            {
                ModelState.AddModelError("", "無效的帳號或密碼");
                return View();
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);

            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("", "無效的帳號或密碼");
                return View();
            }

            // 清除先前的 ModelState 錯誤
            ModelState.Clear();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("UserId", user.Id.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            return RedirectToAction("StudentProgress", "Enrollment", new { studentId = user.Id });
        }

        // 登出邏輯
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Register()
        {
            return View();
        }

        // 註冊處理邏輯
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string email, string password, string passwordConfirmation)
        {
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

            if (password != passwordConfirmation)
            {
                ModelState.AddModelError("", "密碼不匹配");
                return View();
            }

            if (await _context.Users.AnyAsync(u => u.Email == email))
            {
                ModelState.AddModelError("", "該電子郵件已被註冊");
                return View();
            }

            var user = new User
            {
                Email = email,
                Password = _passwordHasher.HashPassword(null, password), // 密碼哈希處理
                Name = email.Split('@')[0] // 使用電子郵件前綴作為用戶名
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }
    }
}
