namespace SecSoul.Model.Entity
{
    public partial class ScanDirb
    {
        public int Id { get; set; }
        public int ScanRequestId { get; set; }
        public string FoundUrl { get; set; }
        public bool IsDirectory { get; set; }

        public virtual ScanRequest ScanRequest { get; set; }
    }
}
    