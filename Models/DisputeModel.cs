namespace SkillHub.Models
{
    /// <summary>
    /// Maps directly to dbo.Disputes for the File Dispute flow.
    /// </summary>
    public sealed class DisputeModel
    {
        public int OrderId { get; set; }

        public int OpenedBy { get; set; }

        public string Reason { get; set; }

        public string Status { get; set; }
    }
}