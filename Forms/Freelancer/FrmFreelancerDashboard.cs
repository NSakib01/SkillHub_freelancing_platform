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
                "Your published service listings",
                ReadCount(
                    "SELECT COUNT(*) " +
                    "FROM dbo.Services " +
                    "WHERE FreelancerId = @UserId;",
                    UserSession.UserId)
                + " software-service listing(s) currently belong to your account.",
                "Manage Services",
                ManageServicesClick);

            // ========================================================
            // ORDERS
            // ========================================================

            AddMainCardWithAction(
                "Your active orders",
                ReadCount(
                    "SELECT COUNT(*) " +
                    "FROM dbo.Orders " +
                    "WHERE FreelancerId = @UserId " +
                    "AND OrderStatus IN " +
                    "(N'Placed', N'In Progress', N'Delivered');",
                    UserSession.UserId)
                + " order(s) are waiting for freelancer processing "
                + "or client approval.",
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

            AddMainCard(
                "Freelancer workspace",
                "Manage your profile, publish software services, "
                + "process client orders and manage your wallet.");

            // ========================================================
            // SIDE INFORMATION
            // ========================================================

            AddSideCard(
                "Freelancer module",
                "Profile • Services • Orders • Wallet • Withdrawals");

            AddSideCard(
                "Security",
                "All freelancer data is loaded using the signed-in "
                + "UserSession.UserId.");
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
                decimal? balance =
                    ReadDecimal(
                        "SELECT AvailableBalance " +
                        "FROM dbo.vw_FreelancerWalletBalances " +
                        "WHERE FreelancerId = @UserId;",
                        UserSession.UserId);

                if (!balance.HasValue)
                {
                    MessageBox.Show(
                        this,
                        "Your wallet balance could not be loaded.",
                        "Wallet",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                MessageBox.Show(
                    this,
                    "Freelancer Wallet\r\n"
                    + "────────────────────────\r\n\r\n"
                    + "Available Balance\r\n"
                    + "BDT "
                    + balance.Value.ToString("N2")
                    + "\r\n\r\n"
                    + "This is the amount currently available "
                    + "for withdrawal.",
                    "Wallet Balance",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
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