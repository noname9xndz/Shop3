using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Shop3.ApiServerDefault
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://localhost:9006")
                .UseStartup<Startup>();
        }
    }
}