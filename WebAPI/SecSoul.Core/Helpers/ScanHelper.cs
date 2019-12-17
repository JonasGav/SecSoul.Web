using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SecSoul.Core.Options;
using SecSoul.Model;
using SecSoul.Model.Entity;

namespace SecSoul.Core.Helpers
{
    public class ScanHelper
    {
        private PossibleScansOptions _possibleScans;
        private ShellHelper _shellHelper;
        public ScanHelper(IOptionsMonitor<PossibleScansOptions> optionsMonitor, ShellHelper shellHelper)
        {
            _possibleScans = optionsMonitor.CurrentValue;
            _shellHelper = shellHelper;

        }

        public Task NmapHelper(ScanRequest request)
        {
            var nmap = _possibleScans.Scans.First(x => x.Id == (int) ScanEnum.Nmap);
            var resultName = _possibleScans.ScanOuputLocation + request.UserId + "_" + request.WebsiteUrl; 
            _shellHelper.ShellExecute(string.Format(nmap.Script, "Localhost", resultName));
            _shellHelper.ShellExecute(string.Format("xsltproc {0} -o {1}", resultName + ".xml", resultName + ".html"));
            return Task.CompletedTask;
        }
    }
}