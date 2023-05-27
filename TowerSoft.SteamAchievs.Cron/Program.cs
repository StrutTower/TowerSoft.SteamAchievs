using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using TowerSoft.SteamAchievs.Lib.Config;
using TowerSoft.SteamAchievs.Lib.Repository;
using TowerSoft.SteamAchievs.Lib.Services;
using TowerSoft.SteamGridDbWrapper;
using TowerSoft.SteamTower;

ServiceProvider services = ConfigureServices();


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

    IServiceCollection services = new ServiceCollection()
        .AddLogging(c => c.AddSerilog())
        .AddSingleton(configuration)
        .Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)))
        .AddScoped(x => new SteamApiClient(apiKeys.Steam))
        .AddScoped(x => new SteamGridClient(apiKeys.SteamGridDb))
        //.AddScoped<SteamScraper>()
        .AddScoped<UnitOfWork>();
        //.AddScoped<GameDataSync>()
        //.AddScoped<UserStatsUpdater>()
        //.AddScoped<GetPlaytimes>()
        //.AddScoped<SteamDataLoader>()
        ////.AddScoped<LastPlayedUpdate>()
        //.AddScoped<SyncRecentGames>()
        //.AddScoped<ScanPerfectGames>()
        //.AddScoped<SyncProtonDbRating>();

    //services.AddHttpClient<SteamApiClient>();
    //services.AddHttpClient<SteamScraper>();
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
        .WriteTo.Console()
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