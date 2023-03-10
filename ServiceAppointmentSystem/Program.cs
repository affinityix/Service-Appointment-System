using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using ServiceAppointmentSystem.Data;
using ServiceAppointmentSystem.Models.Entities;
using ServiceAppointmentSystem.Repositories;
using ServiceAppointmentSystem.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configurations = builder.Configuration;

var connectionString = configurations.GetConnectionString("DefaultConnection");

services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

services.AddTransient<IEmailSender, EmailSender>();

services.AddTransient<IUnitOfWork, UnitOfWork>();

services.AddControllersWithViews();

services.AddRazorPages();

services.ConfigureApplicationCookie(options =>
{
    options.LogoutPath = $"/Identity/Account/Logout";
    options.LoginPath = $"/Identity/Account/Login";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{Area=User}/{controller=Home}/{action=Index}/{id?}");

app.Run();
