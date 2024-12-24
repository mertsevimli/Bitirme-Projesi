using System.Security.Claims;
using Sarideniz.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Sarideniz.Service.Abstract;
using Sarideniz.Service.Concrete;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = "Sarideniz.Cookie";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromDays(365);
});

builder.Services.AddDbContext<DatabaseContext>();
builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
{
    x.LoginPath = "/Account/SignIn";
    x.AccessDeniedPath = "/AccessDenied";
    x.Cookie.Name = "Account";
    x.Cookie.MaxAge = TimeSpan.FromDays(1);
    x.Cookie.IsEssential = true;
});
builder.Services.AddAuthorization(x =>
{
    x.AddPolicy("AdminPolicy", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
    x.AddPolicy("UserPolicy", policy => policy.RequireClaim(ClaimTypes.Role, "Admin", "User", "Customer"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession(); // Session middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "Admin",
    pattern: "{area:exists}/{controller=Main}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();