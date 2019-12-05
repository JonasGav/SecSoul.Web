using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.Entity
{
    public partial class ScanRequest
    {
        public int Id { get; set; }
        public string WebsiteUrl { get; set; }
        public string WebsiteFtp { get; set; }
        public DateTime RequestDate { get; set; }
    }
}
