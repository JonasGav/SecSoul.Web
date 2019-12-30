namespace SecSoul.Model.Entity
{
    public partial class ScanVirusTotal
    {
        public int Id { get; set; }
        public int ScanRequestId { get; set; }
        public string ScanResult { get; set; }

        public virtual ScanRequest ScanRequest { get; set; }
    }
}