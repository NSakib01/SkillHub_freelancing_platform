using SkillHub.Forms.Common;
using SkillHub.Models;
using SkillHub.Services;
using SkillHub.Utilities;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SkillHub.Forms.Freelancer
{
    [DesignerCategory("Code")]
    public sealed class FrmFreelancerDashboard
        : DashboardFormBase
    {
        // ============================================================
        // CONSTRUCTOR
        // ============================================================

        public FrmFreelancerDashboard(
            AuthenticationService authentication)
            : base(
                authentication,
                UserRoles.Freelancer,
                "SkillHub | Freelancer Dashboard")
        {
            // ========================================================
            // SERVICES
            // ========================================================

            AddMainCardWithAction(
                "Your service catalogue",
                ReadCount(
                    "SELECT COUNT(*) " +
                    "FROM dbo.Services " +
                    "WHERE FreelancerId = @UserId;",
                    UserSession.UserId)
                + " service listing(s) are available in your freelancer workspace.",
                "Manage Services",
                ManageServicesClick);

            // ========================================================
            // ORDERS
            // ========================================================

            AddMainCardWithAction(
                "Projects requiring attention",
                ReadCount(
                    "SELECT COUNT(*) " +
                    "FROM dbo.Orders " +
                    "WHERE FreelancerId = @UserId " +
                    "AND OrderStatus IN " +
                    "(N'Placed', N'In Progress', N'Delivered');",
                    UserSession.UserId)
                + " active order(s) are waiting for progress or client approval.",
                "Manage Orders",
                ManageOrdersClick);

            // ========================================================
            // WALLET
            // ========================================================

            decimal? walletBalance =
                ReadDecimal(
                    "SELECT AvailableBalance " +
                    "FROM dbo.vw_FreelancerWalletBalances " +
                    "WHERE FreelancerId = @UserId;",
                    UserSession.UserId);

            string walletText;

            if (walletBalance.HasValue)
            {
                walletText =
                    "BDT "
                    + walletBalance.Value.ToString("N2")
                    + " is currently available for withdrawal.";
            }
            else
            {
                walletText =
                    "Your wallet balance is currently unavailable. "
                    + "Please check your wallet details.";
            }

            AddMainCardWithAction(
                "Available wallet balance",
                walletText,
                "View Wallet",
                ViewWalletClick);

            // ========================================================
            // WORKSPACE
            // ========================================================

            AddMainCardWithAction(
                "Build a stronger public profile",
                "Add a professional portrait, focused title, biography and skills so clients can confidently choose your services.",
                "Edit Profile",
                EditProfileClick);

            // ========================================================
            // SIDE INFORMATION
            // ========================================================

            AddSideCard(
                "Your freelancer toolkit",
                "Profile • Service images • Orders • Wallet • Withdrawals");

            AddSideCard(
                "Publishing tip",
                "Use a clear service image and a detailed description. Clients can now open every listing before adding it to their cart.");
        }

        // ============================================================
        // PROFILE
        // ============================================================

        protected override void OpenProfile()
        {
            using (FrmFreelancerProfile profile =
                   new FrmFreelancerProfile(
                       UserSession.UserId))
            {
                profile.ShowDialog(this);
            }
        }

        private void EditProfileClick(object sender, EventArgs e)
        {
            OpenProfile();
            RefreshIdentity();
        }

        // ============================================================
        // MANAGE SERVICES
        // ============================================================

        private void ManageServicesClick(
            object sender,
            EventArgs e)
        {
            try
            {
                using (FrmManageServices services =
                       new FrmManageServices())
                {
                    services.ShowDialog(this);
                }
            }
            catch (Exception exception)
            {
                UiFactory.ShowError(
                    this,
                    exception);
            }
        }

        // ============================================================
        // MANAGE ORDERS
        // ============================================================

        private void ManageOrdersClick(
            object sender,
            EventArgs e)
        {
            try
            {
                using (FrmFreelancerOrders orders =
                       new FrmFreelancerOrders())
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

        // ============================================================
        // VIEW WALLET
        // ============================================================

        private void ViewWalletClick(
            object sender,
            EventArgs e)
        {
            try
            {
                using (FrmWallet wallet = new FrmWallet(UserSession.UserId))
                {
                    wallet.ShowDialog(this);
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
