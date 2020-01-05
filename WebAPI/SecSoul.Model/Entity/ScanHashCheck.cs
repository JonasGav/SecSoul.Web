namespace SecSoul.Model.Entity
{
    public partial class ScanHashCheck
    {
        public int Id { get; set; }
        public int ScanRequestId { get; set; }
        public string Hash { get; set; }
        public string Location { get; set; }
        public string MalwarePercentage { get; set; }

        public virtual ScanRequest ScanRequest { get; set; }
    }
}