namespace SkillHub.Models
{
    /// <summary>
    /// One line of the signed-in client's cart, enriched with catalog data
    /// (service title, freelancer name) for display in FrmCart.
    /// </summary>
    public sealed class CartItem
    {
        public int CartItemId { get; set; }

        public int CartId { get; set; }

        public int ServiceId { get; set; }

        public string ServiceTitle { get; set; }

        public string FreelancerName { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Subtotal
        {
            get { return UnitPrice * Quantity; }
        }
    }
}
