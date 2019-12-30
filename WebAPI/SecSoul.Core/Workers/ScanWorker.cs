using System;
using System.Collections.Generic;
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
        private ScanHelper _scanHelper;
        public ScanWorker(ILogger<ScanWorker> logger, SecSoulService secSoulService,  ScanHelper scanHelper, ShellHelper shellHelper, IOptionsMonitor<PossibleScansOptions> scanOptions)
        {
            _logger = logger;
            _secSoulService = secSoulService;    
            _shellHelper = shellHelper;
            _possibleScans = scanOptions.CurrentValue;
            _scanHelper = scanHelper;
        }
        public async Task SyncMain()
        {
            try
            {
                var unprocessedRequests = _secSoulService.GetUnprocessedScanRequest();
                foreach (var request in unprocessedRequests)
                {

                    var scanTasksList = new List<Task>();
                    
                    scanTasksList.Add(Task.Run(() => _scanHelper.NmapHelper(request)));
                    //scanTasksList.Add(Task.Run(() => _scanHelper.VirusTotalHelper(request)));
                    
                    await Task.WhenAll(scanTasksList);
                    request.IsProcessed = true;
                }

                _secSoulService.UpdateUnprocessedScanRequest(unprocessedRequests);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception in ScanWorker, exception: ");
                throw;
            }
        }
    }
}
