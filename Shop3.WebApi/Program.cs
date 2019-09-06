using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shop3.Data.EF;

namespace Shop3.WebApi
{
    public class Program
    {
        //public static void Main(string[] args)
        //{
        //    var host = CreateWebHostBuilder(args).Build();
        //    using (var scope = host.Services.CreateScope())
        //    {
        //        var services = scope.ServiceProvider;

        //        try
        //        {
        //            var dbInitializer = services.GetService<DbInitializer>();
        //            dbInitializer.Seed().Wait();
        //        }
        //        catch (Exception ex)
        //        {
        //            var logger = services.GetService<ILogger<Program>>();
        //            logger.LogError(ex, "An error occurred while seeding the database");
        //        }
        //    }
        //    host.Run();
        //}


        //public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //        .UseStartup<Startup>();

        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseUrls("http://localhost:5051/")
                .Build();
    }
}
