# SkillHub Authentication and Account CRUD Verification

**Owner:** MD. Nazmus Sakib  
**Student ID:** 24-58148-2  
**Module:** Database foundation, authentication, authorization, and profiles

## Database setup checks

| ID | Action | Expected result |
| --- | --- | --- |
| DB-01 | Execute `Database/SkillHubDatabase.sql` on LocalDB | `SkillHubDB` is created without deleting unrelated databases or existing data |
| DB-02 | Run the same script a second time | Existing records remain; no duplicate roles, categories, users, or demo services appear |
| DB-03 | Execute `Database/SkillHub_Verification.sql` | All `PASS` messages are printed and the expected account/category rows are returned |
| DB-04 | Count the required table names | All 16 documented entities exist |
| DB-05 | Inspect `dbo.Users.PasswordHash` | Every seed value begins with `PBKDF2-SHA256$120000$`; no plaintext password is stored |
| DB-06 | Inspect `dbo.PlatformSettings` | `CommissionPercent` is `10.00` |
| DB-07 | Inspect `dbo.Categories` | All 13 software-service categories are present and active |

## Login, registration, and logout checks

| ID | Action | Expected result |
| --- | --- | --- |
| AUTH-01 | Log in as `admin@skillhub.local` with `Admin@123` | Platform Admin Dashboard opens; `UserSession.RoleName = Admin` |
| AUTH-02 | Log in as `freelancer@skillhub.local` with `Freelancer@123` | Freelancer Dashboard opens; `UserSession.UserType = ADMIN` |
| AUTH-03 | Log in as `client@skillhub.local` with `Client@123` | Client Dashboard opens; `UserSession.UserType = CUSTOMER` |
| AUTH-04 | Supply a valid email with the wrong password | Sign-in fails without exposing account/password details |
| AUTH-05 | Leave email or password blank | Sign-in is blocked with a clear validation message |
| AUTH-06 | Enter an invalid email | The application requests a correctly formatted email |
| AUTH-07 | Click `Log Out` and confirm | Session is cleared and the existing login form becomes visible again |
| AUTH-08 | Click `Log Out` and reject confirmation | Session remains active and the dashboard stays open |
| AUTH-09 | Register a Client with a unique valid email | `Users`, `ClientProfiles`, and `Carts` are created atomically |
| AUTH-10 | Register a Freelancer with a unique valid email | `Users` and `FreelancerProfiles` are created atomically |
| AUTH-11 | Attempt to register the same email twice | The duplicate is rejected and no extra profile/cart is created |
| AUTH-12 | Try a weak password | Registration rejects passwords missing uppercase, lowercase, number, or symbol |
| AUTH-13 | Use different password/confirmation values | Registration is rejected before database insertion |
| AUTH-14 | Inspect the public role selector | Only Client and Freelancer are selectable; no public Admin registration exists |
| AUTH-15 | Log in after an account is deactivated | Sign-in is denied because account status is not `Active` |

## Profile, password, authorization, and CRUD checks

| ID | Action | Expected result |
| --- | --- | --- |
| ACCT-01 | Open `My Profile` after login | The actual current user ID, canonical role, compatibility type, and stored account fields appear |
| ACCT-02 | Change full name/phone/address and save | The exact signed-in account updates; `UserSession` refreshes automatically |
| ACCT-03 | Change profile email to another account's email | Update is rejected by both friendly validation and the unique database constraint |
| ACCT-04 | Enter an incorrect current password | Password change is rejected |
| ACCT-05 | Enter a valid different password and matching confirmation | A newly salted PBKDF2 hash is stored and the new password works |
| ACCT-06 | Reuse the current password as the new password | Password change is rejected |
| ACCT-07 | Open `Account / Profile CRUD` as Admin | DataGridView displays joined account rows and role aliases |
| ACCT-08 | Create a Client/Freelancer from the account editor | The record appears in the grid and SQL Server with its proper child records |
| ACCT-09 | Search by name, email, role, or status | Only matching account rows remain visible |
| ACCT-10 | Select an account and update its profile | The database row and refreshed grid show the changes |
| ACCT-11 | Deactivate a selected test account | `Status` changes to `Deactivated`; the account row is not physically deleted |
| ACCT-12 | Attempt to deactivate the current administrator | The action is rejected before modifying the database |
| ACCT-13 | Attempt to open an Admin-only form with Client/Freelancer role | `AuthorizationService` rejects the unauthorized request |
| ACCT-14 | Test the database while LocalDB is unavailable | The application reports a friendly connection failure instead of crashing or exposing the connection string |
| ACCT-15 | Change an existing SQL user's `RoleId` | The database trigger rejects role mutation |
| ACCT-16 | Attempt to remove the last active administrator | The database trigger rejects the change |
| ACCT-17 | Insert a review before its order is completed | The review integrity trigger rejects it |
| ACCT-18 | Insert a second credit for the same completed order | The filtered unique wallet index rejects duplicate settlement |

## Evidence for the individual demonstration

Capture the running login form, all three dashboards, the Admin account grid,
successful create/search/update/deactivate steps, one validation failure,
`dbo.Users`/`dbo.Roles`/profile rows in SQL Server, and the shared
`feature/database-foundation` Git branch.

Suggested opening line:

> My name is MD. Nazmus Sakib, student ID 24-58148-2. My SkillHub module
> implements the shared SQL Server database foundation, secure login and
> registration, role-based session management, and complete account/profile CRUD.
