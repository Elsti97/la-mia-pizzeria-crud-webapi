using la_mia_pizzeria_static.Models;
using la_mia_pizzeria_static.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PizzeriaContext>();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<PizzeriaContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.Use(async (context, next) =>
{
    var customCulture = new CultureInfo("en-US");
    customCulture.NumberFormat.NumberDecimalSeparator = ".";
    Thread.CurrentThread.CurrentCulture = customCulture;

    await next.Invoke();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Pizza}/{action=Index}/{id?}");

app.MapRazorPages();




using (var scope = app.Services.CreateScope())
using (var ctx = scope.ServiceProvider.GetService<PizzeriaContext>())
{
    ctx!.Seed();
}

app.Run();
