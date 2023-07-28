using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using System.Diagnostics;
using TowerSoft.SteamAchievs.Cron.Jobs;
using TowerSoft.SteamAchievs.Lib.Config;
using TowerSoft.SteamAchievs.Lib.Repository;
using TowerSoft.SteamAchievs.Lib.Services;
using TowerSoft.SteamGridDbWrapper;
using TowerSoft.SteamTower;


ServiceProvider services = ConfigureServices();

if (Debugger.IsAttached) {
    //services.GetService<FullGameSync>().StartJob();
    //services.GetService<HiddenAchievementSync>().StartJob();
    //services.GetService<RecentGamesSync>().StartJob();
    services.GetService<HowLongToBeatSync>().StartJob();
    //services.GetService<UpdateGameDetails>().StartJob();
    //services.GetService<ProtonDbSync>().StartJob();
    Environment.Exit(1);
}

if (args.Contains("-full"))
    services.GetService<FullGameSync>().StartJob();
if (args.Contains("-recent"))
    services.GetService<RecentGamesSync>().StartJob();
if (args.Contains("-hidden"))
    services.GetService<HiddenAchievementSync>().StartJob();


static ServiceProvider ConfigureServices() {
    IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
#if DEBUG
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
#else
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                //.AddJsonFile("appsettings.Production.json", optional: false, reloadOnChange: true)
#endif
                .AddJsonFile("appsecrets.json", optional: false, reloadOnChange: true)
                .Build();

    AppSettings appSettings = configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();
    ApiKeys apiKeys = configuration.GetSection(nameof(ApiKeys)).Get<ApiKeys>();

    ConfigureLogger(appSettings);

    IServiceCollection services = new ServiceCollection();

    services.AddHttpClient("steamApi", config => {
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

    services.AddHttpClient<AchievementStatsService>("achievementStats", client => { });

    services
        .AddLogging(c => c.AddSerilog())
        .AddSingleton(configuration)
        .Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)))
        .AddScoped<FullGameSync>()
        .AddScoped<HiddenAchievementSync>()
        .AddScoped<RecentGamesSync>()
        .AddScoped<HowLongToBeatSync>()
        .AddScoped<UpdateGameDetails>()
        .AddScoped<ProtonDbSync>()
        .AddScoped<SteamSyncService>()
        .AddScoped(x => new SteamApiClient(x.GetService<IHttpClientFactory>().CreateClient("steamApi"), apiKeys.Steam))
        .AddScoped(x => new SteamGridClient(apiKeys.SteamGridDb))
        .AddScoped(x => new AchievementStatsService(x.GetService<IHttpClientFactory>().CreateClient("achievementStats"), apiKeys.AchievementStats))
        .AddScoped<UnitOfWork>();


    services.AddHttpClient<HowLongToBeatService>(client => {
        client.BaseAddress = new Uri("https://howlongtobeat.com");
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:108.0) Gecko/20100101 Firefox/108.0");
        client.DefaultRequestHeaders.Add("origin", "https://howlongtobeat.com/");
        client.DefaultRequestHeaders.Add("referer", "https://howlongtobeat.com/");
    });
    services.AddHttpClient<ProtonDbService>(client => {
        client.BaseAddress = new Uri("https://www.protondb.com");
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:108.0) Gecko/20100101 Firefox/108.0");
    });

    ServiceProvider serviceProvider = services.BuildServiceProvider();
    return serviceProvider;
}

static void ConfigureLogger(AppSettings appSettings) {
    Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .MinimumLevel.Override("System.Net.Http", LogEventLevel.Warning)
        .WriteTo.Console(Serilog.Events.LogEventLevel.Information)
        .WriteTo.File(
            Environment.ExpandEnvironmentVariables(appSettings.LogPath),
            Serilog.Events.LogEventLevel.Information,
            rollingInterval: RollingInterval.Day)
        .CreateLogger();

    AppDomain.CurrentDomain.UnhandledException += OnException;
}

static void OnException(object sender, UnhandledExceptionEventArgs e) {
    Exception ex = (Exception)e.ExceptionObject;
    Log.Error(ex.ToString());
}