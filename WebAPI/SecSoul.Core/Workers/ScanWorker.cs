using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SecSoul.Core.Helpers;
using SecSoul.Core.Services;

namespace SecSoul.Core.Workers
{
    public class ScanWorker
    {
        ILogger<ScanWorker> _logger;
        private SecSoulService _secSoulService;
        private ShellHelper _shellHelper;
        public ScanWorker(ILogger<ScanWorker> logger, SecSoulService secSoulService, ShellHelper shellHelper)
        {
            _logger = logger;
            _secSoulService = secSoulService;
            _shellHelper = shellHelper;
        }
        public async Task SyncMain()
        {
            try
            {
                var unprocessedRequests = _secSoulService.GetUnprocessedScanRequest();
                _shellHelper.ShellExecute("echo testing");

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
