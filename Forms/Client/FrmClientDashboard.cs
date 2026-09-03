using System;
using SkillHub.Forms.Common;
using SkillHub.Models;
using SkillHub.Services;
using SkillHub.Utilities;

namespace SkillHub.Forms.Client
{
    /// <summary>
    /// Authorized client landing page.
    /// </summary>
    public sealed class FrmClientDashboard : DashboardFormBase
    {
        public FrmClientDashboard(
        AuthenticationService authentication)
        : base(
        authentication,
        UserRoles.Client,
        "SkillHub | Client Dashboard")
        {
            AddMainCardWithAction(
                "Discover your next technology expert",
                ReadCount(
                    "SELECT COUNT(DISTINCT FreelancerId) FROM dbo.vw_ServiceCatalog "
                    + "WHERE IsActive = 1 AND AvailableSlots > 0;")
                + " active freelancer(s) are ready to help with your next project.",
                "Explore Marketplace",
                BrowseServicesButtonClick);

            AddMainCardWithAction(
                "Browse the visual service catalogue",
                ReadCount(
                    "SELECT COUNT(*) FROM dbo.vw_ServiceCatalog "
                    + "WHERE IsActive = 1 AND AvailableSlots > 0;")
                + " service package(s) can be searched, filtered, compared and opened for details.",
                "View Services",
                BrowseServicesButtonClick);


        AddMainCardWithAction(
                "Your project shortlist",
            ReadCount(
                "SELECT COUNT(*) FROM dbo.CartItems AS items "
                + "INNER JOIN dbo.Carts AS carts "
                + "ON carts.CartId = items.CartId "
                + "WHERE carts.ClientId = @UserId;",
                UserSession.UserId)
                + " selected service item(s) are waiting in your cart.",
            "View Cart",
            CartButtonClick);

            AddMainCardWithAction(
                "Track your projects",
                ReadCount(
                    "SELECT COUNT(*) FROM dbo.Orders "
                    + "WHERE ClientId = @UserId;",
                    UserSession.UserId)
                + " order(s) are available in your project history.",
                "My Orders",
                OrdersButtonClick);

            AddMainCard(
                "A simple way to hire",
                "Explore a service, review the complete offer and freelancer profile, "
                + "add it to your cart, then place a simulated order securely.");

            AddSideCard(
                "Marketplace tip",
                "Use category and price sorting to compare services. Open any card to read the full description before adding it to your cart.");
        }

        private void BrowseServicesButtonClick(
            object sender,
            EventArgs arguments)
        {
            try
            {
                using (FrmBrowseServices browseServices =
                       new FrmBrowseServices())
                {
                    browseServices.ShowDialog(this);
                }
            }
            catch (Exception exception)
            {
                UiFactory.ShowError(
                    this,
                    exception);
            }
        }

        private void CartButtonClick(
            object sender,
            EventArgs arguments)
        {
            try
            {
                using (FrmCart cart =
                       new FrmCart())
                {
                    cart.ShowDialog(this);
                }
            }
            catch (Exception exception)
            {
                UiFactory.ShowError(
                    this,
                    exception);
            }
        }

        private void OrdersButtonClick(
            object sender,
            EventArgs arguments)
        {
            try
            {
                using (FrmClientOrders orders =
                       new FrmClientOrders())
                {
                    orders.ShowDialog(this);
                }
            }
            catch (Exception exception)
            {
                UiFactory.ShowError(
                    this,
                    exception);
            }
        }
    }


}
