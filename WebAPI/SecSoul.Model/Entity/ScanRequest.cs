using System;

namespace SecSoul.Model.Entity
{
    public partial class ScanRequest
    {
        public int Id { get; set; }
        public string WebsiteUrl { get; set; }
        public string WebsiteFtp { get; set; }
        public DateTime RequestDate { get; set; }
        public string UserId { get; set; }
        public bool IsProcessed { get; set; }

        public virtual AspNetUsers User { get; set; }
    }
}
