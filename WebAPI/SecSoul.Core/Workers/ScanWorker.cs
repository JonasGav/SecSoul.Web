using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SecSoul.Core.Helpers;
using SecSoul.Core.Options;
using SecSoul.Core.Services;

namespace SecSoul.Core.Workers
{
    public class ScanWorker
    {
        ILogger<ScanWorker> _logger;
        private PossibleScansOptions _possibleScans;
        private SecSoulService _secSoulService;
        private ShellHelper _shellHelper;
        public ScanWorker(ILogger<ScanWorker> logger, SecSoulService secSoulService, ShellHelper shellHelper, IOptionsMonitor<PossibleScansOptions> scanOptions)
        {
            _logger = logger;
            _secSoulService = secSoulService;
            _shellHelper = shellHelper;
            _possibleScans = scanOptions.CurrentValue;
        }
        public async Task SyncMain()
        {
            try
            {
                var unprocessedRequests = _secSoulService.GetUnprocessedScanRequest();
                foreach (var request in unprocessedRequests)
                {
                    foreach (var scan in _possibleScans.Scans)
                    {
                        _shellHelper.ShellExecute( string.Format(scan.Script, "Localhost", request.UserId));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
