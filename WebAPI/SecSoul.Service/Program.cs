using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SecSoul.Core;
using SecSoul.Core.DbService;
using SecSoul.Core.Mains;
using SecSoul.Core.Options;
using SecSoul.Core.Scans;
using SecSoul.Core.Services;
using SecSoul.Core.Workers;
using SecSoul.Model.Context;
using SecSoul.Model.ContextFactories;
using SecSoul.Model.Repository;

namespace SecSoul.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp.SetBasePath(Directory.GetCurrentDirectory());
                    configApp.AddJsonFile("appsettings.json");
                })
                .ConfigureServices((hostContext, services) =>
                {
                    var connetionString = "";
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        connetionString = hostContext.Configuration.GetConnectionString("SecSoulConnectionString");
                    }
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    {
                        connetionString = hostContext.Configuration.GetConnectionString("LinuxSecSoulConnectionString");
                    }
                    services.AddDbContext<SecSoulContext>(options =>
                        options.UseSqlServer(connetionString));
                    services.AddSingleton(new SecSoulContextFactory(new DbContextOptionsBuilder()
                        .UseSqlServer(connetionString).Options));
                    
                    services.AddScoped<SecSoulRepository>();
                    services.AddScoped<SecSoulService>();
                    services.AddSingleton<ShellService>();
                    
                    services.AddSingleton<NmapScan>();
                    services.AddSingleton<VirusTotalScan>();
                    services.AddSingleton<HashCheckScan>();
                    services.AddSingleton<DirbScan>();
                    services.AddSingleton<WebCrawlerService>();
                    services.AddSingleton<HashCheckScan>();

                    services.Configure<TimersOption>(hostContext.Configuration.GetSection("Timers"));
                    services.Configure<PossibleScansOptions>(hostContext.Configuration.GetSection("PossibleScans"));
                    services.AddScoped<ScanMain>();
                    services.AddScoped<ScanWorker>();
                    services.AddHostedService<ScanMain>();

                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                });
    }
}
