using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TimonIdentityServer.Config;
using TimonIdentityServer.Data;
using TimonIdentityServer.Models;
using TimonIdentityServer.Services;
using ConfigurationDbContext = TimonIdentityServer.Data.ConfigurationDbContext;

namespace TimonIdentityServer
{
    public class Startup
    {
        public static void ConfigureAppConfiguration(IConfigurationBuilder config)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

            config.AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .Build();
        }

        public Startup(IHostEnvironment hosEnvironment, IConfiguration configuration)
        {
            HosEnvironment = hosEnvironment;
            Configuration = configuration;
        }

        private IHostEnvironment HosEnvironment { get; }
        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var identityServerConfigurationSection = Configuration.GetSection(nameof(IdentityServerOptions));

            var identityServerOptions = new IdentityServerOptions();
            identityServerConfigurationSection.Bind(identityServerOptions);

            services.Configure<IdentityServerOptions>(identityServerConfigurationSection);

            services.AddControllersWithViews();

            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly))
            );

            services.AddDbContext<ConfigurationDbContext>(options =>
                options.UseNpgsql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.SignIn.RequireConfirmedEmail = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>();

            var builder = services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                    options.UserInteraction.LoginUrl = "/Account/Login";
                    options.UserInteraction.LogoutUrl = "/Account/Logout";
                })
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseNpgsql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseNpgsql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
                    options.EnableTokenCleanup = true;
                })
                .AddProfileService<ProfileService>()
                .AddJwtBearerClientAuthentication()
                .AddAspNetIdentity<ApplicationUser>();

            services.AddTransient<IProfileService, ProfileService>();

            if (HosEnvironment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                var cert = new X509Certificate2(Path.Combine(HosEnvironment.ContentRootPath, "cert.pfx"), "");
                builder.AddSigningCredential(cert);
            }


            services.AddRazorPages();

            services.AddAuthentication()
                .AddOpenIdConnect("oidc", config =>
                {
                    config.Authority = identityServerOptions.EndPoint;
                    config.ClientId = "timon";
                    config.ClientSecret = "secret";
                    config.SaveTokens = true;
                    config.ResponseType = "code";

                    config.Scope.Clear();
                    config.Scope.Add("openid");
                    config.Scope.Add("profile");
                    config.Scope.Add("timon");
                    config.Scope.Add("offline_access");
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            UpdateDatabase(app);

            app.UseStaticFiles();
            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });
        }

        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            using var applicationDbContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
            applicationDbContext.Database.Migrate();

            using var configurationDbContext = serviceScope.ServiceProvider.GetService<ConfigurationDbContext>();
            configurationDbContext.Database.Migrate();

            using var persistedGrantDbContext = serviceScope.ServiceProvider.GetService<PersistedGrantDbContext>();
            persistedGrantDbContext.Database.Migrate();
        }
    }
}