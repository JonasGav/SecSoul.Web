using System;
using System.Collections.Generic;

namespace SecSoul.Model.Entity
{
    public partial class ScanRequest
    {
        public ScanRequest()
        {
            ScanNmap = new HashSet<ScanNmap>();
            ScanVirusTotal = new HashSet<ScanVirusTotal>();
            ScanDirb = new HashSet<ScanDirb>();
        }

        public int Id { get; set; }
        public string WebsiteUrl { get; set; }
        public string WebsiteFtp { get; set; }
        public string FtpUsername { get; set; }
        public string FtpPassword { get; set; }
        public DateTime RequestDate { get; set; }
        public string UserId { get; set; }
        public bool IsProcessed { get; set; }

        public virtual AspNetUsers User { get; set; }
        public virtual ICollection<ScanNmap> ScanNmap { get; set; }
        public virtual ICollection<ScanVirusTotal> ScanVirusTotal { get; set; }
        public virtual ICollection<ScanDirb> ScanDirb { get; set; }
    }
}
