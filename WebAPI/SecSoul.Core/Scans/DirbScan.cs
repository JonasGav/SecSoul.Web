using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SecSoul.Core.Options;
using SecSoul.Core.Services;
using SecSoul.Model;
using SecSoul.Model.Entity;

namespace SecSoul.Core.Scans
{
    public class DirbScan
    {
        private PossibleScansOptions _possibleScans;
        private ShellService _shellService;
        private ILogger<DirbScan> _logger;

        public DirbScan(IOptionsMonitor<PossibleScansOptions> optionsMonitor, ShellService shellService,
            ILogger<DirbScan> logger)
        {
            _possibleScans = optionsMonitor.CurrentValue;
            _shellService = shellService;
            _logger = logger;
        }

        public Task ExecuteDirbScan(ScanRequest request)
        {
            try
            {
                var uri = new Uri(request.WebsiteUrl);

                var dirb = _possibleScans.Scans.First(x => x.Id == (int) ScanEnum.Dirb);

                _shellService.ShellExecute(string.Format(dirb.Script, request.Id, uri.AbsoluteUri));

                ExtractDirbResult(request);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error at ExecuteDirbScan, Error: ");
            }
            return Task.CompletedTask;
        }

        private async void ExtractDirbResult(ScanRequest request)
        {
            DirectoryInfo outputDirectoryInfo = new DirectoryInfo(_possibleScans.ScanOuputLocation);

            var requestDirectoryInfo = outputDirectoryInfo.GetDirectories()
                .FirstOrDefault(x => x.Name == request.Id.ToString());

            if (requestDirectoryInfo == null || !requestDirectoryInfo.Exists) return;

            var nmapDirectoryInfo =
                requestDirectoryInfo.GetDirectories().FirstOrDefault(x => x.Name == "secsoul_dirb");

            if (nmapDirectoryInfo == null || !nmapDirectoryInfo.Exists) return;

            var filesDirectoryInfo = nmapDirectoryInfo.GetDirectories().FirstOrDefault();

            if (filesDirectoryInfo == null || !filesDirectoryInfo.Exists) return;

            var resultFile = filesDirectoryInfo.GetFiles().FirstOrDefault();

            if (resultFile == null || !resultFile.Exists) return;

            using (var st = new StreamReader(resultFile.FullName))
            {
                var regxDir = new Regex("^==> DIRECTORY: (.+)$");
                var regxPage = new Regex("^\\+ (.+?) \\(CODE:([0-9]+)");

                string line;
                while ((line = st.ReadLine()) != null)
                {
                    var matchDir = regxDir.Match(line);
                    var matchPage = regxPage.Match(line);

                    if (matchDir.Success)
                    {
                        var matchGroup = matchDir.Groups[1];
                        request.ScanDirb.Add(new ScanDirb()
                        {
                            IsDirectory = true,
                            FoundUrl = matchGroup.Value
                        });
                        continue;
                    }

                    if (matchPage.Success)
                    {
                        var matchGroup = matchPage.Groups[1];
                        var matchStatus = matchPage.Groups[2];
                        request.ScanDirb.Add(new ScanDirb()
                        {
                            IsDirectory = false,
                            FoundUrl = matchGroup.Value,
                            HttpStatus = matchStatus.Value
                        });
                        //_logger.LogInformation(matchGroup.Value);
                    }
                }
            }

            var content = File.ReadAllText(resultFile.FullName);
            var regxListable =
                new Regex(
                    "---- Entering directory: (.+) ----\n\\(!\\) WARNING: Directory IS LISTABLE. No need to scan it.");
            var regexMatches = regxListable.Matches(content);
            foreach (Match regexMatch in regexMatches)
            {
                foreach (var MatchGroup in regexMatch.Groups)
                {
                    var foundDir = request.ScanDirb.Where(x => x.IsDirectory)
                        .FirstOrDefault(x => x.FoundUrl == MatchGroup.ToString());
                    if (foundDir != null) foundDir.IsListable = true;
                }
            }

        }
    }
}