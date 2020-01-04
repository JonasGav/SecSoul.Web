using System;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SecSoul.Core.Options;
using SecSoul.Core.Services;
using SecSoul.Model;
using SecSoul.Model.Entity;

namespace SecSoul.Core.Scans
{
    public class NmapScan
    {
        private PossibleScansOptions _possibleScans;
        private ShellService _shellService;
        private ILogger<NmapScan> _logger;
        public NmapScan(IOptionsMonitor<PossibleScansOptions> optionsMonitor, ShellService shellService, ILogger<NmapScan> logger)
        {
            _possibleScans = optionsMonitor.CurrentValue;
            _shellService = shellService;
            _logger = logger;
            
        }
        public Task ExecuteNmapScan(ScanRequest request)
        {
            var uri = new Uri(request.WebsiteUrl);
            
            var nmap = _possibleScans.Scans.First(x => x.Id == (int) ScanEnum.Nmap);
            

            _shellService.ShellExecute(string.Format(nmap.Script, request.Id, uri.Host));

            ExtractNmapResult(request);
            
            return Task.CompletedTask;
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
    }
}