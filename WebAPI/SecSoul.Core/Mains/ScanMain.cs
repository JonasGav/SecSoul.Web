using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SecSoul.Core.Options;
using SecSoul.Core.Workers;

namespace SecSoul.Core.Mains
{
    public class ScanMain : IHostedService
    {
        private readonly ILogger<ScanMain> _logger;
        private IServiceScopeFactory _factory;
        System.Timers.Timer timer = new System.Timers.Timer();
        private TimersOption _options;

        private bool isFinished = false;

        public ScanMain(ILogger<ScanMain> logger, IServiceScopeFactory factory, IOptionsMonitor<TimersOption> options)
        {
            _logger = logger;
            _factory = factory;
            _options = options.CurrentValue;

            timer.Interval = _options.ScanTimer;
            timer.AutoReset = false;
            timer.Elapsed += MainSyncThread;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer.Start();
            _logger.LogInformation("HomeSec started");
            return Task.CompletedTask;
        }

        public async void MainSyncThread(object sender, ElapsedEventArgs e)
        {
            try
            {
                using (var scope = _factory.CreateScope())
                {
                    var engine = scope.ServiceProvider.GetService<ScanWorker>();
                    await engine.SyncMain();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error: ");
            }
            finally
            {
                isFinished = true;
                timer.Start();
            }
        }
        //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        //{
        //    _logger.LogInformation("ScanMain started running at: {time}", DateTimeOffset.Now);

        //    while (!stoppingToken.IsCancellationRequested)
        //    {
        //        _logger.LogInformation("ScanMain running at: {time}", DateTimeOffset.Now);
        //        await Task.Delay(1000, stoppingToken);
        //    }

        //    _logger.LogInformation("ScanMain stopped running at: {time}", DateTimeOffset.Now);
        //}
        public Task StopAsync(CancellationToken cancellationToken)
        {
            while (!isFinished)
            {
                Task.Delay(1000).Wait();

            }
            _logger.LogInformation("SecSoul stopped");
            return Task.CompletedTask;
        }
    }
}
