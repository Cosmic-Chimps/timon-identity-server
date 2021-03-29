using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace TimonIdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var x = Environment.GetEnvironmentVariable("CONNECTION_STRING");
            System.Console.WriteLine(x);
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(Startup.ConfigureAppConfiguration)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}
