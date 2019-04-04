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

namespace Shop3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // CreateWebHostBuilder(args).Build().Run();

            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider; // lấy các service trong startup(ConfigureServices)

                try
                { // dùng service locator để lấy ra nhưng thằng đã được register trong startup(ConfigureServices)
                    var dbInitializer = services.GetService<DbInitializer>(); 
                    dbInitializer.Seed().Wait(); // chạy phương thức seed chứa các dữ liệu mẫu reder ra cùng vs database
                }
                catch (Exception ex)
                {// dùng logger để xuất ra error
                    var logger = services.GetService<ILogger<Program>>();
                    logger.LogError(ex, "lỗi chạy seed khi khởi tạo database");
                }
            }
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseApplicationInsights()
                .UseStartup<Startup>();
    }
}
