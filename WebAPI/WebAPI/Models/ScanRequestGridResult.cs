using System;

namespace SecSoul.WebAPI.Models
{
    public class ScanRequestGridResult
    {

        public int Id { get; set; }
        public string WebsiteUrl { get; set; }
        public string WebsiteFtp { get; set; }
        public DateTime RequestDate { get; set; }
        public string UserId { get; set; }
        public bool IsProcessed { get; set; }
        public bool NmapScanned { get; set; }
        public bool DirbScanned { get; set; }
        public bool VirusTotalScanned { get; set; }
        public bool HashCheckCompleted { get; set; }
        
    }
}