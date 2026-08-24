using System;

namespace SkillHub.Models
{
    public sealed class FreelancerProfile
    {
        public int UserId { get; set; }

        public string ProfessionalTitle { get; set; }

        public string Biography { get; set; }

        public string Skills { get; set; }

        public bool IsVerified { get; set; }

        public decimal AverageRating { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}