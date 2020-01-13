using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SecSoul.Core.DbService;
using SecSoul.Core.Options;
using SecSoul.Core.Scans;
using SecSoul.Core.Services;

namespace SecSoul.Core.Workers
{
    public class ScanWorker
    {
        ILogger<ScanWorker> _logger;
        private PossibleScansOptions _possibleScans;
        private SecSoulService _secSoulService;
        private ShellService _shellService;
        private VirusTotalScan _virusTotalScan;
        private NmapScan _nmapScan;
        private DirbScan _dirbScan;
        private HashCheckScan _hashCheckScan;
        private WebCrawlerService _webCrawlerService;

        public ScanWorker(ILogger<ScanWorker> logger, SecSoulService secSoulService, ShellService shellService,
            IOptionsMonitor<PossibleScansOptions> scanOptions, VirusTotalScan virusTotalScan, NmapScan nmapScan, DirbScan dirbScan,
            WebCrawlerService webCrawlerService, HashCheckScan hashCheckScan)
        {
            _logger = logger;
            _secSoulService = secSoulService;
            _shellService = shellService;
            _possibleScans = scanOptions.CurrentValue;
            _virusTotalScan = virusTotalScan;
            _nmapScan = nmapScan;
            _dirbScan = dirbScan;
            _webCrawlerService = webCrawlerService;
            _hashCheckScan = hashCheckScan;
        }

        public async Task SyncMain()
        {
            try
            {
                var unprocessedRequests = _secSoulService.GetUnprocessedScanRequest();
                foreach (var request in unprocessedRequests)
                {
                    _logger.LogDebug("Starting scan for {0} request", request.Id);
                    
                    var scanTasksList = new List<Task>();

                    scanTasksList.Add(Task.Run(() => _nmapScan.ExecuteNmapScan(request)));
                    
                    scanTasksList.Add(Task.Run(() => _dirbScan.ExecuteDirbScan(request)));
                    
                    scanTasksList.Add(Task.Run(() => _virusTotalScan.ExecuteVirusTotalScan(request)));
                    
                    if(!string.IsNullOrEmpty(request.WebsiteFtp))
                         scanTasksList.Add(Task.Run(() => _hashCheckScan.ExecuteHashCheckScan(request)));
                    
                    _logger.LogDebug("Scan finished");

                    await Task.WhenAll(scanTasksList);
                    request.IsProcessed = true;
                    _secSoulService.UpdateUnprocessedScanRequest(request);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception in ScanWorker, exception: ");
                throw;
            }
        }
    }
}
