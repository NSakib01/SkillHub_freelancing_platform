using System;
using SkillHub.Forms.Common;
using SkillHub.Models;
using SkillHub.Services;

namespace SkillHub.Forms.Admin
{
    /// <summary>
    /// Authorized super-admin landing page with Sakib's completed account CRUD
    /// and clearly reserved integration points for Omi's finance module.
    /// </summary>
    public sealed class FrmAdminDashboard : DashboardFormBase
    {
        public FrmAdminDashboard(AuthenticationService authentication)
            : base(authentication, UserRoles.Admin, "SkillHub | Platform Admin Dashboard")
        {
            AddMainCard(
                "Registered marketplace accounts",
                ReadCount("SELECT COUNT(*) FROM dbo.Users;")
                + " Client, Freelancer and Admin account(s) are stored in SkillHubDB.");

            AddMainCard(
                "Software-service catalogue",
                ReadCount("SELECT COUNT(*) FROM dbo.Services;")
                + " service listing(s) across "
                + ReadCount("SELECT COUNT(*) FROM dbo.Categories WHERE IsActive = 1;")
                + " active software-development categories.");

            AddMainCard(
                "Orders and commission revenue",
                ReadCount("SELECT COUNT(*) FROM dbo.Orders;")
                + " order(s); completed-order platform revenue: BDT "
                + ReadCount(
                    "SELECT COALESCE(SUM(CommissionAmount), 0.00) "
                    + "FROM dbo.Orders WHERE OrderStatus = N'Completed';")
                + ".");

            AddMainCard(
                "Admin and financial module owner: Omi",
                "Attach category/offer CRUD, service moderation, disputes, "
                + "withdrawal approval and revenue reporting to this dashboard.");

            AddMainAction(
                "Open Account / Profile CRUD",
                OpenAccountManager,
                282);

            AddSideCard(
                "Sakib's individual CRUD",
                "Create, search/read, edit and safely deactivate user accounts "
                + "with a connected DataGridView and parameterized SQL.");

            AddSideCard(
                "Admin integration tables",
                "Categories, Offers, Orders, Payments, Disputes, "
                + "WithdrawalRequests and PlatformSettings.");
        }

        private void OpenAccountManager(object sender, EventArgs arguments)
        {
            try
            {
                using (FrmAccountManager manager = new FrmAccountManager(Authentication))
                {
                    manager.ShowDialog(this);
                }

                RefreshIdentity();
            }
            catch (Exception exception)
            {
                UiFactory.ShowError(this, exception);
            }
        }
    }
}
