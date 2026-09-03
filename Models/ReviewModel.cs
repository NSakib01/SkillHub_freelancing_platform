namespace SkillHub.Models
{
    /// <summary>
    /// Maps directly to dbo.Reviews for the Approve Completion + Review flow.
    /// </summary>
    public sealed class ReviewModel
    {
        public int OrderId { get; set; }

        public int ClientId { get; set; }

        public int FreelancerId { get; set; }

        public byte Rating { get; set; }

        public string Comment { get; set; }
    }
}
