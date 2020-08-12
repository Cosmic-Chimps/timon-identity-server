using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TimonIdentityServer.Data;

[assembly: HostingStartup(typeof(TimonIdentityServer.Areas.Identity.IdentityHostingStartup))]
namespace TimonIdentityServer.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
            // builder.ConfigureServices((context, services) => {
            //     services.AddDbContext<TimonIdentityServerIdentityDbContext>(options =>
            //         options.UseSqlServer(
            //             context.Configuration.GetConnectionString("TimonIdentityServerIdentityDbContextConnection")));
            //
            //     services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //         .AddEntityFrameworkStores<TimonIdentityServerIdentityDbContext>();
            // });
        }
    }
}