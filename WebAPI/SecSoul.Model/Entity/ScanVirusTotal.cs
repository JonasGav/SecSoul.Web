namespace SecSoul.Model.Entity
{
    public partial class ScanVirusTotal
    {
        public int Id { get; set; }
        public int ScanRequestId { get; set; }
        public string ScanProvider { get; set; }
        public bool ScanResult { get; set; }

        public virtual ScanRequest ScanRequest { get; set; }
    }
}