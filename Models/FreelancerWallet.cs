namespace SkillHub.Models
{
    public class FreelancerWallet
    {
        public int FreelancerId { get; set; }

        public string FullName { get; set; }

        public decimal LedgerBalance { get; set; }

        public decimal PendingWithdrawalAmount { get; set; }

        public decimal AvailableBalance { get; set; }
    }
}