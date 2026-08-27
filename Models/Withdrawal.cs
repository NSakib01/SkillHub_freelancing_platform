using System;

namespace SkillHub.Models
{
    public class Withdrawal
    {
        public int WithdrawalId { get; set; }

        public int FreelancerId { get; set; }

        public decimal Amount { get; set; }

        public string Status { get; set; }

        public DateTime RequestDate { get; set; }

        public int? ProcessedBy { get; set; }

        public DateTime? ProcessedAt { get; set; }

        public string AdminNote { get; set; }
    }
}