using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SecSoul.Core;
using SecSoul.Core.Helpers;
using SecSoul.Core.Mains;
using SecSoul.Core.Options;
using SecSoul.Core.Services;
using SecSoul.Core.Workers;
using SecSoul.Model.Context;
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
                    services.AddDbContext<SecSoulContext>(options =>
                        options.UseSqlServer(hostContext.Configuration.GetConnectionString("SecSoulConnectionString")));

                    services.AddScoped<SecSoulRepository>();
                    services.AddScoped<SecSoulService>();
                    services.AddSingleton<ShellHelper>();

                    services.Configure<TimersOption>(hostContext.Configuration.GetSection("Timers"));
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
