using System;

namespace SkillHub.Models
{
    public class Service
    {
        public int ServiceId { get; set; }

        public int FreelancerId { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public decimal Price { get; set; }

        public int DeliveryDays { get; set; }

        public int AvailableSlots { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
