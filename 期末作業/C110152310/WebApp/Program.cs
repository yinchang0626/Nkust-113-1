var builder = WebApplication.CreateBuilder(args);

// Add services required by MapControllerRoute method to work
builder.Services.AddControllersWithViews();

var app = builder.Build();

// configure app to serve static files
app.UseStaticFiles();

// configure routing service and middleware
app.UseRouting(); // add routing middleware to the app

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}"); // add endpoints for controller actions
// The endpoints would match with controller name and action(method names) and ids if any. Default values are provided that is if no controller is found, Home would be used and Index action would be used.

app.Run();
