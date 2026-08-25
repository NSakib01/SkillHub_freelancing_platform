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
    public sealed class FrmFreelancerDashboard : DashboardFormBase
    {
        // ============================================================
        // CONSTRUCTOR
        // ============================================================

        public FrmFreelancerDashboard(AuthenticationService authentication)
            : base(
                authentication,
                UserRoles.Freelancer,
                "SkillHub | Freelancer Dashboard")
        {
            // ========================================================
            // SERVICE CARD + MANAGE SERVICES ACTION
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
            // ACTIVE ORDERS
            // ========================================================

            AddMainCard(
                "Your active orders",
                ReadCount(
                    "SELECT COUNT(*) " +
                    "FROM dbo.Orders " +
                    "WHERE FreelancerId = @UserId " +
                    "AND OrderStatus IN " +
                    "(N'Placed', N'In Progress', N'Delivered');",
                    UserSession.UserId)
                + " order(s) are waiting for freelancer processing "
                + "or client approval.");

            // ========================================================
            // WALLET
            // ========================================================

            AddMainCard(
                "Available wallet balance",
                "BDT "
                + ReadCount(
                    "SELECT AvailableBalance " +
                    "FROM dbo.vw_FreelancerWalletBalances " +
                    "WHERE FreelancerId = @UserId;",
                    UserSession.UserId)
                + " is available after pending withdrawal requests.");

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
                   new FrmFreelancerProfile(UserSession.UserId))
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
                UiFactory.ShowError(this, exception);
            }
        }
    }
}