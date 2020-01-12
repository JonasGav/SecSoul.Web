using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SecSoul.Core.Options;
using SecSoul.Core.Services;
using SecSoul.Model;
using SecSoul.Model.Entity;

namespace SecSoul.Core.Scans
{
    public class HashCheckScan
    {
        private PossibleScansOptions _possibleScans;
        private ShellService _shellService;
        private ILogger<HashCheckScan> _logger;

        public HashCheckScan(IOptionsMonitor<PossibleScansOptions> optionsMonitor, ShellService shellService,
            ILogger<HashCheckScan> logger)
        {
            _possibleScans = optionsMonitor.CurrentValue;
            _shellService = shellService;
            _logger = logger;
        }

        public Task ExecuteHashCheckScan(ScanRequest request)
        {
            try
            {
                var uri = new Uri(request.WebsiteFtp);

                var nmap = _possibleScans.Scans.First(x => x.Id == (int) ScanEnum.HashScan);

                _shellService.ShellExecute(string.Format(nmap.Script, request.Id, uri.AbsoluteUri, request.FtpUsername,
                    request.FtpPassword));

                ExtractHashCheckResult(request);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error at ExecuteHashCheckScan, Error: ");
            }

            return Task.CompletedTask;
        }

        private async void ExtractHashCheckResult(ScanRequest request)
        {
            DirectoryInfo outputDirectoryInfo = new DirectoryInfo(_possibleScans.ScanOuputLocation);
            var requestDirectoryInfo = outputDirectoryInfo.GetDirectories()
                .FirstOrDefault(x => x.Name == request.Id.ToString());

            if (requestDirectoryInfo == null || !requestDirectoryInfo.Exists) return;

            var aaa = outputDirectoryInfo.GetDirectories();
            var nmapDirectoryInfo =
                requestDirectoryInfo.GetDirectories().FirstOrDefault(x => x.Name == "secsoul_hashcheck");

            if (nmapDirectoryInfo == null || !nmapDirectoryInfo.Exists) return;

            var filesDirectoryInfo = nmapDirectoryInfo.GetDirectories().FirstOrDefault();
            if (filesDirectoryInfo == null || !filesDirectoryInfo.Exists) return;

            var resultFiles = filesDirectoryInfo.GetFiles();

            FileInfo resultFile = resultFiles.FirstOrDefault(x => x.Name.Contains("hashcheckResult"));

            if (resultFile == null) return;


            using (var fs = resultFile.OpenRead())
            using (var sr = new StreamReader(fs))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Contains('#'))
                    {
                        continue;
                    }

                    var words = line.Split(' ');
                    if (words.Last().Contains("NO_DATA"))
                    {
                        continue;
                    }

                    var originalFile = resultFiles.FirstOrDefault(x => x.Name.Contains("hashcheckOriginal"));

                    if (originalFile == null || !originalFile.Exists) return;
                    using (var rfs = originalFile.OpenRead())
                    using (var rsr = new StreamReader(rfs))
                    {
                        string originalResultLine;
                        while ((originalResultLine = rsr.ReadLine()) != null)
                        {
                            var wordsInResultLine = originalResultLine.Split(' ');
                            if (wordsInResultLine.FirstOrDefault() == words.FirstOrDefault())
                            {
                                var newHashCheckResult = new ScanHashCheck()
                                {
                                    Hash = wordsInResultLine.FirstOrDefault(),
                                    Location = originalResultLine.Substring(originalResultLine.IndexOf(' ')),
                                    MalwarePercentage = words.LastOrDefault()
                                };
                                request.ScanHashCheck.Add(newHashCheckResult);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}