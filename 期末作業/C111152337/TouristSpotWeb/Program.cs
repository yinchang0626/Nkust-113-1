using Microsoft.EntityFrameworkCore;
using System;
using TouristSpotWeb.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// 註冊 IHttpClientFactory
builder.Services.AddHttpClient();
// 註冊 IHttpContextAccessor
builder.Services.AddHttpContextAccessor();

// 與景點資料庫連線
builder.Services.AddDbContext<Sightseeing_spotsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WebDatabase"))
);

// 新增 Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// 配置身份驗證
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)  // 默認使用 Cookie 驗證
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";  // 登入頁面
        options.AccessDeniedPath = "/Account/AccessDenied";  // 權限不足頁面
    });

builder.Services.AddControllersWithViews();

var app = builder.Build();

// 配置 HTTP 請求管道
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 啟用身份驗證和授權
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();  // 啟用 Session

// 配置路由
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");
app.MapControllerRoute(
    name: "TouristSpotDetails",
    pattern: "TouristSpotDetails/{id?}",
    defaults: new { controller = "Home", action = "TouristSpotDetails" });
app.MapControllerRoute(
    name: "TouristSpotChange",
    pattern: "TouristSpot/Manage/{id?}",
    defaults: new { controller = "Home", action = "Manage" });

app.Run();
