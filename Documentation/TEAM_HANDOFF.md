# SkillHub Team Handoff - Shared Foundation Contract

**Prepared by:** MD. Nazmus Sakib  
**Student ID:** 24-58148-2  
**Project:** SkillHub software-service freelance marketplace  
**Foundation branch:** `feature/database-foundation`

## Files every teammate asked for

| Requested item | Exact delivered path | What it provides |
| --- | --- | --- |
| Database connection class | `Data/DatabaseConnection.cs` | Shared `SqlConnection` creation, safe opening, typed parameters, and connection testing |
| Connection string | `App.config` | `SkillHubConnection` targeting `SkillHubDB` |
| Session and login user ID | `Utilities/UserSession.cs` | `UserSession.UserId`, `LoginUserId`, `RoleName`, `UserType`, `Email`, and `FullName` |
| Complete database SQL | `Database/SkillHubDatabase.sql` | Database, 16 tables, keys, constraints, indexes, roles, categories, users, views, and triggers |
| Database verification | `Database/SkillHub_Verification.sql` | Read-only setup validation and demonstration queries |
| Authorization guard | `Services/AuthorizationService.cs` | Enforce the correct account role before opening a form |
| Role constants | `Models/UserRoles.cs` | Canonical roles and compatibility mapping |

## One-time setup for everyone

1. Execute the full `Database/SkillHubDatabase.sql` against SQL Server LocalDB
   or SQL Server Express.
2. Execute `Database/SkillHub_Verification.sql` and check the `PASS` results.
3. Open `SkillHub.sln` in Visual Studio 2022.
4. Adjust only the SQL Server `Data Source` in `App.config` when your local
   instance name differs.
5. Build the project and test all three seeded roles.
6. Create your own branch from the shared foundation.

## Shared sign-in accounts

| Role | Email | Password |
| --- | --- | --- |
| Platform Admin | `admin@skillhub.local` | `Admin@123` |
| Freelancer | `freelancer@skillhub.local` | `Freelancer@123` |
| Client | `client@skillhub.local` | `Client@123` |

## Naming values that must not drift

| Contract | Approved values |
| --- | --- |
| Database | `SkillHubDB` |
| Connection-string key | `SkillHubConnection` |
| SQL provider | `System.Data.SqlClient` |
| Framework | `.NET Framework 4.8` |
| Canonical roles | `Admin`, `Freelancer`, `Client` |
| Legacy role aliases | `SUPER_ADMIN`, `ADMIN`, `CUSTOMER` |
| Account statuses | `Active`, `Suspended`, `Deactivated` |
| Order statuses | `Pending Payment`, `Placed`, `In Progress`, `Delivered`, `Completed`, `Disputed`, `Cancelled`, `Refunded` |
| Payment statuses | `Pending`, `Paid`, `Failed`, `Refunded` |
| Simulated payment methods | `Simulated Card`, `Simulated Mobile Banking`, `Simulated Bank Transfer`, `Demo Payment`, `Card`, `Mobile Banking`, `Bank Transfer`, `bKash`, `Nagad` |
| Withdrawal statuses | `Pending`, `Approved`, `Rejected` |
| Dispute statuses | `Open`, `Under Review`, `Resolved`, `Rejected` |
| Default commission setting | `PlatformSettings.SettingKey = 'CommissionPercent'`, value `10.00` |

The role mapping is **Admin -> SUPER_ADMIN; Freelancer -> ADMIN; Client ->
CUSTOMER**. New code should use canonical role names.

## Required imports and current signed-in user

```csharp
using System.Data;
using System.Data.SqlClient;
using SkillHub.Data;
using SkillHub.Models;
using SkillHub.Services;
using SkillHub.Utilities;

int signedInUserId = UserSession.UserId;
string signedInRole = UserSession.RoleName;
string compatibilityUserType = UserSession.UserType;
```

Never assign a guessed ID such as `1` or `2`. Database identities are generated
by SQL Server and can differ between each teammate's computer.

## Required parameterized query pattern

```csharp
DatabaseConnection database = new DatabaseConnection();

using (SqlConnection connection = database.OpenConnection())
using (SqlCommand command = new SqlCommand(
    "SELECT ServiceId, Title, Price "
    + "FROM dbo.Services WHERE FreelancerId = @FreelancerId;",
    connection))
{
    DatabaseConnection.AddParameter(
        command,
        "@FreelancerId",
        SqlDbType.Int,
        UserSession.UserId);

    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
    {
        DataTable table = new DataTable();
        adapter.Fill(table);
        myDataGridView.DataSource = table;
    }
}
```

Use `DatabaseConnection.AddParameter` with a precise `SqlDbType`; do not build
SQL using textbox concatenation or use a second connection manager.

## Sadman - Freelancer module integration

**Branch:** `feature/freelancer-services`  
**Landing page:** `Forms/Freelancer/FrmFreelancerDashboard.cs`  
**Authorization:** `AuthorizationService.DemandRole(UserRoles.Freelancer);`

Primary tables:

- `dbo.FreelancerProfiles`: professional title, biography, skills, verification,
  and average rating.
- `dbo.Categories`: category selectors for service editors.
- `dbo.Services`: service listing CRUD; `FreelancerId = UserSession.UserId`.
- `dbo.Orders`: accept, mark in progress, and deliver freelancer-owned orders.
- `dbo.WalletTransactions`: transaction history and completed-order credits.
- `dbo.WithdrawalRequests`: create pending withdrawal requests.
- `dbo.vw_FreelancerWalletBalances`: available amount after pending requests.

Expected forms: `FrmFreelancerProfile`, `FrmManageServices`, `FrmServiceEditor`,
`FrmFreelancerOrders`, `FrmWallet`, and `FrmWithdrawal`.

Important rules: prices cannot be negative; delivery days must be greater than
zero; capacity cannot be negative; only a completed order can create its one
freelancer wallet credit.

## Anika - Client module integration

**Branch:** `feature/client-orders`  
**Landing page:** `Forms/Client/FrmClientDashboard.cs`  
**Authorization:** `AuthorizationService.DemandRole(UserRoles.Client);`

Primary tables and views:

- `dbo.vw_ServiceCatalog`: service, freelancer, category, rating, delivery days,
  price, and availability for browse/filter screens.
- `dbo.Carts`: one cart is automatically created during Client registration.
- `dbo.CartItems`: add, read, update quantity, and remove service lines.
- `dbo.Offers`: active service-specific or platform-wide discounts.
- `dbo.Orders`: create one order per cart line, including financial snapshots.
- `dbo.Payments`: create one simulated payment record per order.
- `dbo.Reviews`: one review per completed order; database checks the client and
  freelancer against the original order.
- `dbo.Disputes`: open an order dispute with the signed-in client's user ID.

Expected forms: `FrmBrowseServices`, `FrmServiceDetails`, `FrmCart`,
`FrmCheckout`, `FrmClientOrders`, `FrmReview`, and `FrmDispute`.

Checkout must use one SQL transaction for cart validation, available-slot
reduction, order creation, payment insertion, and cart cleanup. Store `decimal`
prices and commission snapshots. The composite order foreign key prevents using
a service with the wrong freelancer.

## Omi / Aumi - Admin and financial module integration

**Branch:** `feature/admin-finance`  
**Landing page:** `Forms/Admin/FrmAdminDashboard.cs`  
**Authorization:** `AuthorizationService.DemandRole(UserRoles.Admin);`

Primary tables and views:

- `dbo.vw_UserAccounts`: moderation and search without exposing password hashes.
- `dbo.Categories`: Category CRUD; use `IsActive` when history exists.
- `dbo.Offers`: platform or service offers, date range, and percentage.
- `dbo.Services`: listing moderation and deactivation.
- `dbo.Disputes`: decisions, resolutions, and administrator ownership.
- `dbo.WithdrawalRequests`: approve or reject pending requests.
- `dbo.WalletTransactions`: approved withdrawal debit and completed-order
  credit rules.
- `dbo.PlatformSettings`: change the future `CommissionPercent`.
- `dbo.vw_OrderFinancialSummary`: administration and revenue reporting.
- `dbo.vw_FreelancerWalletBalances`: protect against excess withdrawals.

Expected forms: `FrmManageUsers`, `FrmManageCategories`, `FrmModerateServices`,
`FrmManageOffers`, `FrmDisputes`, `FrmWithdrawals`, and `FrmRevenueReport`.

Sakib's `FrmAccountManager` is his account/profile CRUD demonstration. Omi
still owns broader platform moderation, suspensions, categories, offers,
disputes, withdrawal approval, and financial reporting.

## Safe multi-table transaction pattern

```csharp
DatabaseConnection database = new DatabaseConnection();

using (SqlConnection connection = database.OpenConnection())
using (SqlTransaction transaction = connection.BeginTransaction())
{
    try
    {
        using (SqlCommand command = new SqlCommand(
            "UPDATE dbo.Services "
            + "SET AvailableSlots = AvailableSlots - @Quantity "
            + "WHERE ServiceId = @ServiceId "
            + "AND AvailableSlots >= @Quantity;",
            connection,
            transaction))
        {
            DatabaseConnection.AddParameter(
                command, "@Quantity", SqlDbType.Int, requestedQuantity);
            DatabaseConnection.AddParameter(
                command, "@ServiceId", SqlDbType.Int, selectedServiceId);

            if (command.ExecuteNonQuery() != 1)
            {
                throw new InvalidOperationException(
                    "The selected service has insufficient available slots.");
            }
        }

        // Insert Orders, Payments, or wallet rows with the SAME transaction.
        transaction.Commit();
    }
    catch
    {
        transaction.Rollback();
        throw;
    }
}
```

## Merge and integration checklist

1. Pull the current integrated foundation before creating your branch.
2. Keep one solution, one `Program.Main`, one `App.config`, and one database.
3. Add teammate-owned forms under that teammate's role folder.
4. Reuse the shared session, role guards, database class, models, and constants.
5. Add schema changes to the shared SQL script only after team agreement.
6. Do not edit another teammate's form/designer at the same time.
7. Verify each feature under the correct seeded account role.
8. Confirm all SQL writes use typed parameters and transactions when needed.
9. Update the module's README section before opening a pull request.
10. Merge reviewed branches into `develop`; keep `main` submission-ready.
