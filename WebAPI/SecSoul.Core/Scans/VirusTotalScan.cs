using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SecSoul.Core.Options;
using SecSoul.Core.Services;
using SecSoul.Model.Entity;
using VirusTotalNet;
using VirusTotalNet.Objects;
using VirusTotalNet.ResponseCodes;
using VirusTotalNet.Results;

namespace SecSoul.Core.Scans
{
    public class VirusTotalScan
    {
        
        private PossibleScansOptions _possibleScans;
        private ShellService _shellService;
        private ILogger<VirusTotalScan> _logger;
        public VirusTotalScan(IOptionsMonitor<PossibleScansOptions> optionsMonitor, ShellService shellService, ILogger<VirusTotalScan> logger)
        {
            _possibleScans = optionsMonitor.CurrentValue;
            _shellService = shellService;
            _logger = logger;
        }
        
        public async Task ExecuteVirusTotalScan(ScanRequest request)
        {
            VirusTotal virusTotal = new VirusTotal(_possibleScans.VirusTotalApiKey);

            UrlReport urlReport = await virusTotal.GetUrlReportAsync(request.WebsiteUrl);

            bool hasUrlBeenScannedBefore = urlReport.ResponseCode == UrlReportResponseCode.Present;
            
            //_logger.LogInformation("URL has been scanned before: " + (hasUrlBeenScannedBefore ? "Yes" : "No"));

            //If the url has been scanned before, the results are embedded inside the report.
            if (hasUrlBeenScannedBefore)
            {
                ParseVirusTotalScanResult(urlReport, request);
            }
            else
            {
                UrlScanResult urlResult = await virusTotal.ScanUrlAsync(request.WebsiteUrl);
                int tryCount = 0;
                while (tryCount < 30)
                {
                    UrlReport urlCurrentReport = await virusTotal.GetUrlReportAsync(request.WebsiteUrl);
                    hasUrlBeenScannedBefore = urlCurrentReport.ResponseCode == UrlReportResponseCode.Present;
                    if (hasUrlBeenScannedBefore)
                    {
                        ParseVirusTotalScanResult(urlReport, request);
                        break;
                    }
                    _logger.LogDebug("SKIPPING");
                    await Task.Delay(1000);
                    tryCount++;
                }
            }
            
        }
        private void ParseVirusTotalScanResult(UrlReport urlReport, ScanRequest request)
        {
            _logger.LogDebug("Scan ID: " + urlReport.ScanId);
            _logger.LogDebug("Message: " + urlReport.VerboseMsg);

            if (urlReport.ResponseCode == UrlReportResponseCode.Present)
            {
                foreach (KeyValuePair<string, UrlScanEngine> scan in urlReport.Scans)
                {
                    request.ScanVirusTotal.Add(new ScanVirusTotal()
                    {
                        ScanProvider = scan.Key,
                        ScanResult = scan.Value.Detected
                    });
                    _logger.LogDebug("{0,-25} Detected: {1}", scan.Key, scan.Value.Detected);
                }
            }
        }
    }
}