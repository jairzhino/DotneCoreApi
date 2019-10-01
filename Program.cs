
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var Logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();  
        
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).ConfigureLogging(
                    (HostingContext, logging)=>{
                        logging.ClearProviders();  
                        logging.AddConfiguration(HostingContext.Configuration.GetSection("Logging"));
                        logging.AddConsole();
                        logging.AddDebug();
                        logging.AddEventSourceLogger();
                        
                    }
                ).UseNLog();
    }
}
