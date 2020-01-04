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

                    var scanTasksList = new List<Task>();

                    //scanTasksList.Add(Task.Run(() => _nmapScan.ExecuteNmapScan(request)));
                    //
                    // scanTasksList.Add(Task.Run(() => _dirbScan.ExecuteDirbScan(request)));
                    //
                    // scanTasksList.Add(Task.Run(() => _virusTotalScan.ExecuteVirusTotalScan(request)));
                    //
                     scanTasksList.Add(Task.Run(() => _hashCheckScan.ExecuteHashCheckScan(request)));

                    await Task.WhenAll(scanTasksList);

                    // if (request.ScanDirb.Any(x => !x.IsDirectory))
                    // {
                    //     foreach (var pageUri in request.ScanDirb.Where(x => !x.IsDirectory))
                    //     {
                    //         var pageHtml = _webCrawlerService.GetHtml(pageUri.FoundUrl);
                    //         if (!String.IsNullOrEmpty(pageHtml))
                    //         {
                    //
                    //         }
                    //     }
                    // }

                    request.IsProcessed = true;
                }

                if (unprocessedRequests.Count > 0)
                {
#if !DEBUG
                    _secSoulService.UpdateUnprocessedScanRequest(unprocessedRequests);
#endif
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
