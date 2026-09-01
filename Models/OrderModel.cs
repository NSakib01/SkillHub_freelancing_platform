using System;

namespace SkillHub.Models
{
    /// <summary>
    /// One dbo.Orders row, enriched with the service title and freelancer
    /// name for FrmClientOrders, FrmReview and FrmDispute.
    /// </summary>
    public sealed class OrderModel
    {
        public int OrderId { get; set; }

        public int ClientId { get; set; }

        public int FreelancerId { get; set; }

        public int ServiceId { get; set; }

        public string ServiceTitle { get; set; }

        public string FreelancerName { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal GrossAmount { get; set; }

        public decimal CommissionRate { get; set; }

        public decimal CommissionAmount { get; set; }

        public decimal FreelancerEarning { get; set; }

        public string OrderStatus { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool CanApproveCompletion
        {
            get
            {
                return string.Equals(
                    OrderStatus, "Delivered", StringComparison.OrdinalIgnoreCase);
            }
        }

        public bool CanFileDispute
        {
            get
            {
                return string.Equals(OrderStatus, "Placed", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(OrderStatus, "In Progress", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(OrderStatus, "Delivered", StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}