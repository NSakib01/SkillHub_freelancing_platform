namespace SkillHub.Models
{
    /// <summary>
    /// One row projected from dbo.vw_ServiceCatalog for the Client
    /// marketplace catalog, service details screen and cart pricing.
    /// </summary>
    public sealed class ServiceCatalogItem
    {
        public int ServiceId { get; set; }

        public int FreelancerId { get; set; }

        public string FreelancerName { get; set; }

        public string ProfessionalTitle { get; set; }

        public string FreelancerBiography { get; set; }

        public string FreelancerSkills { get; set; }

        public string FreelancerProfileImagePath { get; set; }

        public bool IsVerified { get; set; }

        public decimal AverageRating { get; set; }

        public int CategoryId { get; set; }

        public string CategoryCode { get; set; }

        public string CategoryName { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ServiceImagePath { get; set; }

        public decimal Price { get; set; }

        public int DeliveryDays { get; set; }

        public int AvailableSlots { get; set; }

        public bool IsActive { get; set; }

        public System.DateTime CreatedAt { get; set; }

        public bool IsAvailable
        {
            get { return IsActive && AvailableSlots > 0; }
        }
    }
}
