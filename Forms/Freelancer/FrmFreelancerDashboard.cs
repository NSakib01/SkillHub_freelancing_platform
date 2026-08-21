using SkillHub.Forms.Common;
using SkillHub.Models;
using SkillHub.Services;
using SkillHub.Utilities;

namespace SkillHub.Forms.Freelancer
{
    /// <summary>
    /// Authorized freelancer landing page for Sadman's service and wallet forms.
    /// </summary>
    public sealed class FrmFreelancerDashboard : DashboardFormBase
    {
        public FrmFreelancerDashboard(AuthenticationService authentication)
            : base(authentication, UserRoles.Freelancer, "SkillHub | Freelancer Dashboard")
        {
            AddMainCard(
                "Your published service listings",
                ReadCount(
                    "SELECT COUNT(*) FROM dbo.Services WHERE FreelancerId = @UserId;",
                    UserSession.UserId)
                + " software-service listing(s) currently belong to your account.");

            AddMainCard(
                "Your active orders",
                ReadCount(
                    "SELECT COUNT(*) FROM dbo.Orders "
                    + "WHERE FreelancerId = @UserId "
                    + "AND OrderStatus IN (N'Placed', N'In Progress', N'Delivered');",
                    UserSession.UserId)
                + " order(s) are waiting for freelancer processing or client approval.");

            AddMainCard(
                "Available wallet balance",
                "BDT "
                + ReadCount(
                    "SELECT AvailableBalance "
                    + "FROM dbo.vw_FreelancerWalletBalances "
                    + "WHERE FreelancerId = @UserId;",
                    UserSession.UserId)
                + " is available after pending withdrawal requests.");

            AddMainCard(
                "Freelancer module owner: Sadman",
                "Attach FrmFreelancerProfile, FrmManageServices, FrmServiceEditor, "
                + "FrmFreelancerOrders, FrmWallet and FrmWithdrawal here.");

            AddSideCard(
                "Freelancer integration tables",
                "FreelancerProfiles, Services, Orders, WalletTransactions "
                + "and WithdrawalRequests. Always filter by UserSession.UserId.");
        }
    }
}
