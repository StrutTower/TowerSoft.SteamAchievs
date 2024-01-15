using AutoMapper;
using Microsoft.AspNetCore.ResponseCompression;
using TowerSoft.SteamAchievs.Blazor.Server.Infrastructure;
using TowerSoft.SteamAchievs.Lib.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddJsonFile("appsecrets.json");

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<UnitOfWork>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseWebAssemblyDebugging();
    //app.UseExceptionHandler("/Error");
} else {
    app.UseExceptionHandler("/Error");
}

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
