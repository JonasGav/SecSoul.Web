using System;
using System.Collections.Generic;

namespace SecSoul.Model.Entity
{
    public partial class ScanNmap
    {
        public int Id { get; set; }
        public int ScanRequestId { get; set; }
        public string ScanResult { get; set; }

        public virtual ScanRequest ScanRequest { get; set; }
    }
}
