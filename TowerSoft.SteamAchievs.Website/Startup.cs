using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System.Net.Http;
using TowerSoft.SteamAchievs.Lib.Config;
using TowerSoft.SteamAchievs.Lib.Repository;
using TowerSoft.SteamAchievs.Website.Areas.Admin.Services;
using TowerSoft.SteamAchievs.Website.HtmlRenderers;
using TowerSoft.SteamAchievs.Website.Services;
using TowerSoft.SteamTower;
using TowerSoft.TagHelpers.Options;

namespace TowerSoft.SteamAchievs.Website {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(x => x.LoginPath = new PathString("/Account/Login"));

            services.AddControllersWithViews(options => {
                options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
            }).AddRazorRuntimeCompilation();

            services.Configure<AppSettings>(Configuration.GetSection("appSettings"));
            services.Configure<ApiKeys>(Configuration.GetSection("apikeys"));

            AppSettings appSettings = Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();
            ApiKeys apiKeys = Configuration.GetSection(nameof(ApiKeys)).Get<ApiKeys>();

            RendererRegistration.RegisterDefaultRenderers();
            RendererRegistration.Add<ColorPickerHtmlRenderer>("color");

            services.AddHttpClient("steamApi", config => {
                config.DefaultRequestHeaders.Add("Cookie", new[] { "birthtime =281347201", "lastagecheckage=1-0-1979", "wants_mature_content=1" });
            }).ConfigurePrimaryHttpMessageHandler(() => {
                return new HttpClientHandler {
                    UseDefaultCredentials = true
                };
            });

            services
                .AddHttpContextAccessor()
                .AddScoped<UnitOfWork>()
                .AddScoped<HomeDataService>()
                .AddScoped<GameDataService>()
                .AddScoped<AchievementDataService>()
                .AddScoped<AdminTagDataService>()
                .AddScoped(x => new SteamApiClient(x.GetService<IHttpClientFactory>().CreateClient("steamApi"), apiKeys.Steam));

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Debug()
                .CreateLogger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                //app.UseHttpsRedirection();
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseStatusCodePagesWithReExecute("/Error/Code/{0}");

            app.UseStaticFiles();
            app.UseSerilogRequestLogging();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
