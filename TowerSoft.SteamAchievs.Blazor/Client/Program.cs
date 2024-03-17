using Blzr.BootstrapSelect;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TowerSoft.SteamAchievs.Blazor.Client;
using TowerSoft.SteamAchievs.Blazor.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp =>
    new HttpClient {
        BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
    }
);

builder.Services
    .AddBootstrapSelect()
    .AddScoped<HomeDataService>()
    .AddScoped<GameDataService>()
    .AddScoped<GameDetailsDataService>()
    .AddScoped<AchievementDataService>()
    .AddScoped<AchievementTagDataService>()
    .AddScoped<GameListDataService>()
    .AddScoped<ReportsDataService>()
    .AddScoped<CompanyDataService>()
    .AddScoped<TagDataService>()
    .AddScoped<TableListDataService>();

await builder.Build().RunAsync();
