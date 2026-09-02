using System;
using SkillHub.Forms.Common;
using SkillHub.Models;
using SkillHub.Services;

namespace SkillHub.Forms.Admin
{
    /// <summary>
    /// Authorized super-admin landing page.
    /// Provides account, category, service, offer, review and dispute management.
    /// </summary>
    public sealed class FrmAdminDashboard : DashboardFormBase
    {
        public FrmAdminDashboard(AuthenticationService authentication)
            : base(
                authentication,
                UserRoles.Admin,
                "SkillHub | Platform Admin Dashboard")
        {
            // ============================================================
            // DASHBOARD SUMMARY
            // ============================================================

            AddMainCard(
                "Registered marketplace accounts",
                ReadCount("SELECT COUNT(*) FROM dbo.Users;")
                + " Client, Freelancer and Admin account(s) are stored in SkillHubDB.");

            AddMainCard(
                "Software-service catalogue",
                ReadCount("SELECT COUNT(*) FROM dbo.Services;")
                + " service listing(s) across "
                + ReadCount(
                    "SELECT COUNT(*) FROM dbo.Categories WHERE IsActive = 1;")
                + " active software-development categories.");

            AddMainCard(
                "Orders and commission revenue",
                ReadCount("SELECT COUNT(*) FROM dbo.Orders;")
                + " order(s); completed-order platform revenue: BDT "
                + ReadCount(
                    "SELECT COALESCE(SUM(CommissionAmount), 0.00) "
                    + "FROM dbo.Orders "
                    + "WHERE OrderStatus = N'Completed';")
                + ".");

            AddMainCard(
                "Admin and financial module owner: Aumi",
                "Manage categories, offers, service moderation, reviews, "
                + "disputes, withdrawal approval and revenue reporting.");

            // ============================================================
            // SUPER ADMIN ACTIONS
            // ============================================================

            AddMainAction(
                "Open Account / Profile CRUD",
                OpenAccountManager,
                282);

            AddMainAction(
                "Manage Categories",
                OpenCategoryManager,
                282);

            AddMainAction(
                "Manage Services",
                OpenServiceManager,
                282);

            AddMainAction(
                "Manage Offers",
                OpenOfferManager,
                282);

            AddMainAction(
                "Manage Reviews",
                OpenReviewManager,
                282);

            AddMainAction(
                "Manage Disputes",
                OpenDisputeManager,
                282);

            // ============================================================
            // INFORMATION CARDS
            // ============================================================

            AddSideCard(
                "Sakib's individual CRUD",
                "Create, search/read, edit and safely deactivate user accounts "
                + "with a connected DataGridView and parameterized SQL.");

            AddSideCard(
                "Service moderation",
                "Review freelancer services and safely deactivate or "
                + "reactivate service listings.");

            AddSideCard(
                "Offer management",
                "Review marketplace offers connected to service listings.");

            AddSideCard(
                "Review management",
                "View customer reviews, ratings, comments, clients and "
                + "freelancers for marketplace monitoring.");

            AddSideCard(
                "Dispute management",
                "Review customer complaints and update dispute status, "
                + "resolution and resolution history.");

            AddSideCard(
                "Admin integration tables",
                "Categories, Offers, Orders, Payments, Disputes, "
                + "Reviews, WithdrawalRequests and PlatformSettings.");
        }

        // ================================================================
        // ACCOUNT / PROFILE CRUD
        // ================================================================

        private void OpenAccountManager(
            object sender,
            EventArgs arguments)
        {
            try
            {
                Hide();

                using (FrmAccountManager manager =
                    new FrmAccountManager(Authentication))
                {
                    manager.ShowDialog();
                }

                Show();
                RefreshIdentity();
            }
            catch (Exception exception)
            {
                Show();
                UiFactory.ShowError(this, exception);
            }
        }

        // ================================================================
        // CATEGORY MANAGEMENT
        // ================================================================

        private void OpenCategoryManager(
            object sender,
            EventArgs arguments)
        {
            try
            {
                Hide();

                using (FrmManageCategories manager =
                    new FrmManageCategories(Authentication))
                {
                    manager.ShowDialog();
                }

                Show();
                RefreshIdentity();
            }
            catch (Exception exception)
            {
                Show();
                UiFactory.ShowError(this, exception);
            }
        }

        // ================================================================
        // SERVICE MANAGEMENT
        // ================================================================

        private void OpenServiceManager(
            object sender,
            EventArgs arguments)
        {
            try
            {
                Hide();

                using (FrmManageServices manager =
                    new FrmManageServices(Authentication))
                {
                    manager.ShowDialog();
                }

                Show();
                RefreshIdentity();
            }
            catch (Exception exception)
            {
                Show();
                UiFactory.ShowError(this, exception);
            }
        }

        // ================================================================
        // OFFER MANAGEMENT
        // ================================================================

        private void OpenOfferManager(
            object sender,
            EventArgs arguments)
        {
            try
            {
                Hide();

                using (FrmManageOffers manager =
                    new FrmManageOffers())
                {
                    manager.ShowDialog();
                }

                Show();
                RefreshIdentity();
            }
            catch (Exception exception)
            {
                Show();
                UiFactory.ShowError(this, exception);
            }
        }

        // ================================================================
        // REVIEW MANAGEMENT
        // ================================================================

        private void OpenReviewManager(
            object sender,
            EventArgs arguments)
        {
            try
            {
                Hide();

                using (FrmManageReviews manager =
                    new FrmManageReviews())
                {
                    manager.ShowDialog();
                }

                Show();
                RefreshIdentity();
            }
            catch (Exception exception)
            {
                Show();
                UiFactory.ShowError(this, exception);
            }
        }

        // ================================================================
        // DISPUTE MANAGEMENT
        // ================================================================

        private void OpenDisputeManager(
            object sender,
            EventArgs arguments)
        {
            try
            {
                Hide();

                using (FrmManageDisputes manager =
                    new FrmManageDisputes())
                {
                    manager.ShowDialog();
                }

                Show();
                RefreshIdentity();
            }
            catch (Exception exception)
            {
                Show();
                UiFactory.ShowError(this, exception);
            }
        }
    }
}