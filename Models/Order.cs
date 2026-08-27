using System;

namespace SkillHub.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public int ClientId { get; set; }
        public string ClientName { get; set; }

        public int FreelancerId { get; set; }

        public int ServiceId { get; set; }
        public string ServiceTitle { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal GrossAmount { get; set; }

        public decimal CommissionRate { get; set; }
        public decimal CommissionAmount { get; set; }

        public decimal FreelancerEarning { get; set; }

        public string OrderStatus { get; set; }

        public string DeliveryNote { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? AcceptedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}