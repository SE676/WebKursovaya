using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using WebKursovaya.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Configuration;
using WebKursovaya.ViewModels;
using WebKursovaya.Models;
using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MyDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddDbContext<UserContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);


builder.Services.AddDbContext<SecondDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SecondConnection"))
);

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => //CookieAuthenticationOptions
                {
                    options.LoginPath = new PathString("/Account/Login");
                    options.AccessDeniedPath = new PathString("/Account/AccessDenied");
                });
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IAuditService, AuditService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();