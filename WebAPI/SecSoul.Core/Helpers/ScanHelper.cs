using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SecSoul.Core.Options;
using SecSoul.Model;
using SecSoul.Model.Entity;
using VirusTotalNet;
using VirusTotalNet.Objects;
using VirusTotalNet.ResponseCodes;
using VirusTotalNet.Results;

namespace SecSoul.Core.Helpers
{
    public class ScanHelper
    {
        private PossibleScansOptions _possibleScans;
        private ShellHelper _shellHelper;
        private ILogger<ScanHelper> _logger;
        public ScanHelper(IOptionsMonitor<PossibleScansOptions> optionsMonitor, ShellHelper shellHelper, ILogger<ScanHelper> logger)
        {
            _possibleScans = optionsMonitor.CurrentValue;
            _shellHelper = shellHelper;
            _logger = logger;

        }

        public Task NmapHelper(ScanRequest request)
        {
            var nmap = _possibleScans.Scans.First(x => x.Id == (int) ScanEnum.Nmap);

            var resultName = _possibleScans.ScanOuputLocation + request.UserId + "_" + request.WebsiteUrl;

            _shellHelper.ShellExecute(string.Format(nmap.Script, request.Id, request.WebsiteUrl));

            ExtractNmapResult(request);
            //_shellHelper.ShellExecute(string.Format("xsltproc {0} -o {1}", resultName + ".xml", resultName + ".html"));
            

            return Task.CompletedTask;
        }

        public async Task VirusTotalHelper(ScanRequest request)
        {
            VirusTotal virusTotal = new VirusTotal(_possibleScans.VirusTotalApiKey);

            
            UrlReport urlReport = await virusTotal.GetUrlReportAsync(request.WebsiteUrl);

            bool hasUrlBeenScannedBefore = urlReport.ResponseCode == UrlReportResponseCode.Present;
            Console.WriteLine("URL has been scanned before: " + (hasUrlBeenScannedBefore ? "Yes" : "No"));

            //If the url has been scanned before, the results are embedded inside the report.
            if (hasUrlBeenScannedBefore)
            {
                PrintScan(urlReport);
            }
            else
            {
                UrlScanResult urlResult = await virusTotal.ScanUrlAsync(request.WebsiteUrl);
                PrintScan(urlResult);
            }

            Console.WriteLine("hello");
        }
        private async void ExtractNmapResult(ScanRequest request)
        {
            DirectoryInfo outputDirectoryInfo = new DirectoryInfo(_possibleScans.ScanOuputLocation);
            var requestDirectoryInfo = outputDirectoryInfo.GetDirectories().FirstOrDefault(x => x.Name == request.Id.ToString());
            if (requestDirectoryInfo.Exists)
            {
                var aaa = outputDirectoryInfo.GetDirectories();
                var nmapDirectoryInfo =
                    requestDirectoryInfo.GetDirectories().FirstOrDefault(x => x.Name == "secsoul_nmap");
                if (nmapDirectoryInfo.Exists)
                {
                    var filesDirectoryInfo = nmapDirectoryInfo.GetDirectories().FirstOrDefault();
                    if (filesDirectoryInfo.Exists)
                    {
                        var xmlFile = filesDirectoryInfo.GetFiles().FirstOrDefault(x => x.Extension == ".xml");
                        if (xmlFile.Exists)
                        {
                            request.ScanNmap.Add(new ScanNmap()
                            {
                                ScanRequestId =  request.Id,
                                ScanResult = File.ReadAllText(xmlFile.FullName)
                            });
                        }

                    }
                }
            }
        }

        private static void PrintScan(UrlScanResult scanResult)
        {
            Console.WriteLine("Scan ID: " + scanResult.ScanId);
            Console.WriteLine("Message: " + scanResult.VerboseMsg);
            Console.WriteLine();
        }
        private static void PrintScan(UrlReport urlReport)
        {
            Console.WriteLine("Scan ID: " + urlReport.ScanId);
            Console.WriteLine("Message: " + urlReport.VerboseMsg);

            if (urlReport.ResponseCode == UrlReportResponseCode.Present)
            {
                foreach (KeyValuePair<string, UrlScanEngine> scan in urlReport.Scans)
                {
                    Console.WriteLine("{0,-25} Detected: {1}", scan.Key, scan.Value.Detected);
                }
            }

            Console.WriteLine();
        }
    }
}