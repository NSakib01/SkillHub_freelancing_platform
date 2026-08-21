using SkillHub.Forms.Common;
using SkillHub.Models;
using SkillHub.Services;
using SkillHub.Utilities;

namespace SkillHub.Forms.Client
{
    /// <summary>
    /// Authorized client landing page. Anika attaches her marketplace screens
    /// here without changing the shared authentication or session contract.
    /// </summary>
    public sealed class FrmClientDashboard : DashboardFormBase
    {
        public FrmClientDashboard(AuthenticationService authentication)
            : base(authentication, UserRoles.Client, "SkillHub | Client Dashboard")
        {
            AddMainCard(
                "Services available to browse",
                ReadCount(
                    "SELECT COUNT(*) FROM dbo.vw_ServiceCatalog "
                    + "WHERE IsActive = 1 AND AvailableSlots > 0;")
                + " active software-development services are currently available.");

            AddMainCard(
                "Your current shopping cart",
                ReadCount(
                    "SELECT COUNT(*) FROM dbo.CartItems AS items "
                    + "INNER JOIN dbo.Carts AS carts ON carts.CartId = items.CartId "
                    + "WHERE carts.ClientId = @UserId;",
                    UserSession.UserId)
                + " item(s) belong to your personal cart.");

            AddMainCard(
                "Your order history",
                ReadCount(
                    "SELECT COUNT(*) FROM dbo.Orders WHERE ClientId = @UserId;",
                    UserSession.UserId)
                + " order(s) are linked to your signed-in client account.");

            AddMainCard(
                "Client module owner: Anika",
                "Attach FrmBrowseServices, FrmServiceDetails, FrmCart, FrmCheckout, "
                + "FrmClientOrders, FrmReview and FrmDispute to this landing page.");

            AddSideCard(
                "Client integration tables",
                "Carts, CartItems, Orders, Payments, Reviews and Disputes. "
                + "Read catalogue data from dbo.vw_ServiceCatalog.");
        }
    }
}
