using TowerSoft.SteamAchievs.Lib.Config;
using TowerSoft.SteamAchievs.Lib.Repository;
using TowerSoft.SteamAchievs.Lib.Services;
using TowerSoft.SteamTower;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddJsonFile("appsecrets.json");

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("appSettings"));
builder.Services.Configure<ApiKeys>(builder.Configuration.GetSection("apikeys"));

AppSettings appSettings = builder.Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();
ApiKeys apiKeys = builder.Configuration.GetSection(nameof(ApiKeys)).Get<ApiKeys>();

builder.Services.AddHttpClient("steamApi", config => {
    config.DefaultRequestHeaders.Add("Cookie", new[] {
            "birthtime=281347201",
            "lastagecheckage=1-0-1979",
            "wants_mature_content=1"
        });
    }).ConfigurePrimaryHttpMessageHandler(() => {
        return new HttpClientHandler {
            UseDefaultCredentials = true
        };
    });

builder.Services.AddHttpClient<HowLongToBeatService>(client => {
    client.BaseAddress = new Uri("https://howlongtobeat.com");
    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:108.0) Gecko/20100101 Firefox/108.0");
    client.DefaultRequestHeaders.Add("origin", "https://howlongtobeat.com/");
    client.DefaultRequestHeaders.Add("referer", "https://howlongtobeat.com/");
});

builder.Services.AddHttpClient<ProtonDbService>(client => {
    client.BaseAddress = new Uri("https://www.protondb.com");
    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:108.0) Gecko/20100101 Firefox/108.0");
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services
    .AddScoped<UnitOfWork>()
    .AddScoped<SteamSyncService>()
    .AddScoped(x => new SteamApiClient(x.GetService<IHttpClientFactory>().CreateClient("steamApi"), apiKeys.Steam));

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
app.MapFallbackToPage("/Index");

app.Run();
