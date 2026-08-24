using SkillHub.Forms.Common;
using SkillHub.Forms.Freelancer;
using SkillHub.Models;
using SkillHub.Services;
using SkillHub.Utilities;
using System.ComponentModel;

namespace SkillHub.Forms.Freelancer
{
    [DesignerCategory("Code")]
    public sealed class FrmFreelancerDashboard : DashboardFormBase

    {
        protected override void OpenProfile()
        {
            FrmFreelancerProfile profile = new FrmFreelancerProfile(UserSession.UserId);
            
                profile.ShowDialog(this);
            
        }

        public FrmFreelancerDashboard(AuthenticationService authentication)
            : base(
                authentication,
                UserRoles.Freelancer,
                "SkillHub | Freelancer Dashboard")
        {
            AddMainCard(
                "Your published service listings",
                ReadCount(
                    "SELECT COUNT(*) " +
                    "FROM dbo.Services " +
                    "WHERE FreelancerId = @UserId;",
                    UserSession.UserId)
                + " software-service listing(s) currently belong to your account.");

            AddMainCard(
                "Your active orders",
                ReadCount(
                    "SELECT COUNT(*) " +
                    "FROM dbo.Orders " +
                    "WHERE FreelancerId = @UserId " +
                    "AND OrderStatus IN (N'Placed', N'In Progress', N'Delivered');",
                    UserSession.UserId)
                + " order(s) are waiting for freelancer processing or client approval.");

            AddMainCard(
                "Available wallet balance",
                "BDT "
                + ReadCount(
                    "SELECT AvailableBalance " +
                    "FROM dbo.vw_FreelancerWalletBalances " +
                    "WHERE FreelancerId = @UserId;",
                    UserSession.UserId)
                + " is available after pending withdrawal requests.");

            AddMainCard(
                "Freelancer workspace",
                "Manage your profile, publish software services, "
                + "process client orders and manage your wallet.");

            AddSideCard(
                "Freelancer module",
                "Profile • Services • Orders • Wallet • Withdrawals");

            AddSideCard(
                "Security",
                "All freelancer data is loaded using the signed-in "
                + "UserSession.UserId.");
        }
    }
}