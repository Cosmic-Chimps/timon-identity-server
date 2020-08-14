using Microsoft.AspNetCore.Hosting;
using TimonIdentityServer.Areas.Identity;

[assembly: HostingStartup(typeof(IdentityHostingStartup))]

namespace TimonIdentityServer.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => { });
            // builder.ConfigureServices((context, services) => {
            //     services.AddDbContext<TimonIdentityServerIdentityDbContext>(options =>
            //         options.UseSqlServer(
            //             context.Configuration.GetConnectionString("TimonIdentityServerIdentityDbContextConnection")));
            //
            //     services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //         .AddEntityFrameworkStores<TimonIdentityServerIdentityDbContext>();
            // });
        }
    }
}