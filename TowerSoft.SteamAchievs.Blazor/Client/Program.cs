using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TowerSoft.SteamAchievs.Blazor.Client;
using TowerSoft.SteamAchievs.Blazor.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
//builder.RootComponents.Add<HeadOutlet>("head::after");
builder.RootComponents.Add<HeadOutlet>("head");

builder.Services.AddScoped(sp =>
    new HttpClient {
        BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
    }
);

builder.Services
    .AddScoped<HomeDataService>()
    .AddScoped<GameDataService>()
    .AddScoped<AchievementDataService>()
    .AddScoped<GameListDataService>();

await builder.Build().RunAsync();
