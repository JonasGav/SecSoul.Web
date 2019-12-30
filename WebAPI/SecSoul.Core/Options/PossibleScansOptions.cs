using System.Collections.Generic;

namespace SecSoul.Core.Options
{
    public class PossibleScansOptions
    {
        public IList<Scans> Scans { get; set; }
        public string ScanOuputLocation { get; set; }
        public string VirusTotalApiKey { get; set; }
    }
    public class Scans
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Script { get; set; }
    }
}