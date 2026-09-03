/*
    Read-only verification for Sakib's SkillHub database foundation.
    Run after SkillHubDatabase.sql; the script never inserts, updates or deletes.
*/

USE [SkillHubDB];
GO

SET NOCOUNT ON;
GO

DECLARE @ExpectedTables TABLE (TableName SYSNAME PRIMARY KEY);

INSERT INTO @ExpectedTables (TableName)
VALUES
    (N'Roles'), (N'Users'), (N'ClientProfiles'), (N'FreelancerProfiles'),
    (N'Categories'), (N'Services'), (N'Offers'), (N'Carts'), (N'CartItems'),
    (N'Orders'), (N'Payments'), (N'WalletTransactions'),
    (N'WithdrawalRequests'), (N'Reviews'), (N'Disputes'),
    (N'PlatformSettings');

IF EXISTS
(
    SELECT 1
    FROM @ExpectedTables AS expected
    WHERE OBJECT_ID(N'dbo.' + expected.TableName, N'U') IS NULL
)
BEGIN
    SELECT expected.TableName AS MissingTable
    FROM @ExpectedTables AS expected
    WHERE OBJECT_ID(N'dbo.' + expected.TableName, N'U') IS NULL;

    THROW 52001, 'One or more required SkillHub tables are missing.', 1;
END;

IF (SELECT COUNT(*) FROM dbo.Roles) <> 3
BEGIN
    THROW 52002, 'SkillHub must contain exactly three roles.', 1;
END;

IF EXISTS
(
    SELECT expected.RoleName
    FROM
    (
        VALUES (N'Admin'), (N'Freelancer'), (N'Client')
    ) AS expected (RoleName)
    WHERE NOT EXISTS
    (
        SELECT 1 FROM dbo.Roles AS actual
        WHERE actual.RoleName = expected.RoleName
    )
)
BEGIN
    THROW 52003, 'A required Admin, Freelancer or Client role is missing.', 1;
END;

IF (SELECT COUNT(*) FROM dbo.Categories) < 13
BEGIN
    THROW 52004, 'At least 13 approved software-service categories are required.', 1;
END;

IF COL_LENGTH(N'dbo.Users', N'ProfileImagePath') IS NULL
   OR COL_LENGTH(N'dbo.Services', N'ImagePath') IS NULL
BEGIN
    THROW 52009, 'The marketplace image columns are missing.', 1;
END;

IF NOT EXISTS
(
    SELECT 1 FROM dbo.PlatformSettings
    WHERE SettingKey = N'CommissionPercent'
      AND TRY_CONVERT(DECIMAL(5, 2), SettingValue) IS NOT NULL
)
BEGIN
    THROW 52005, 'A valid commission-percentage platform setting is required.', 1;
END;

IF
(
    SELECT COUNT(*)
    FROM dbo.Users AS users
    INNER JOIN dbo.Roles AS roles ON roles.RoleId = users.RoleId
    WHERE roles.RoleName = N'Freelancer' AND users.Status = N'Active'
) < 6
BEGIN
    THROW 52010, 'At least six active freelancer demo accounts are required.', 1;
END;

IF
(
    SELECT COUNT(*)
    FROM dbo.Users AS users
    INNER JOIN dbo.Roles AS roles ON roles.RoleId = users.RoleId
    WHERE roles.RoleName = N'Client' AND users.Status = N'Active'
) < 4
BEGIN
    THROW 52011, 'At least four active client demo accounts are required.', 1;
END;

IF (SELECT COUNT(*) FROM dbo.Services WHERE IsActive = 1 AND ImagePath IS NOT NULL) < 13
BEGIN
    THROW 52012, 'At least thirteen active services with images are required.', 1;
END;

IF EXISTS
(
    SELECT expected.Email
    FROM
    (
        VALUES
            (N'admin@skillhub.local'),
            (N'freelancer@skillhub.local'),
            (N'client@skillhub.local')
    ) AS expected (Email)
    WHERE NOT EXISTS
    (
        SELECT 1 FROM dbo.Users AS actual
        WHERE actual.Email = expected.Email
          AND actual.Status = N'Active'
          AND actual.PasswordHash LIKE N'PBKDF2-SHA256$120000$%'
    )
)
BEGIN
    THROW 52006, 'A required active, hashed-password demo account is missing.', 1;
END;

IF EXISTS
(
    SELECT expected.ViewName
    FROM
    (
        VALUES
            (N'vw_UserAccounts'),
            (N'vw_ServiceCatalog'),
            (N'vw_OrderFinancialSummary'),
            (N'vw_FreelancerWalletBalances')
    ) AS expected (ViewName)
    WHERE OBJECT_ID(N'dbo.' + expected.ViewName, N'V') IS NULL
)
BEGIN
    THROW 52007, 'One or more shared integration views are missing.', 1;
END;

IF
(
    SELECT COUNT(*)
    FROM sys.triggers AS triggers
    WHERE triggers.parent_id IN
    (
        OBJECT_ID(N'dbo.Users'),
        OBJECT_ID(N'dbo.ClientProfiles'),
        OBJECT_ID(N'dbo.FreelancerProfiles'),
        OBJECT_ID(N'dbo.Carts'),
        OBJECT_ID(N'dbo.Services'),
        OBJECT_ID(N'dbo.Orders'),
        OBJECT_ID(N'dbo.Reviews'),
        OBJECT_ID(N'dbo.WalletTransactions')
    )
) < 8
BEGIN
    THROW 52008, 'Required role, settlement or review guard triggers are missing.', 1;
END;

PRINT N'PASS: all 16 required database tables exist.';
PRINT N'PASS: Admin, Freelancer and Client roles exist.';
PRINT N'PASS: all software-service categories are seeded.';
PRINT N'PASS: commission setting and hashed demo accounts exist.';
PRINT N'PASS: shared views and business-integrity triggers exist.';
PRINT N'PASS: six freelancers, four clients and thirteen visual services exist.';

SELECT
    tables.name AS TableName,
    SUM(partitions.rows) AS ApproximateRowCount
FROM sys.tables AS tables
INNER JOIN sys.partitions AS partitions
    ON partitions.object_id = tables.object_id
   AND partitions.index_id IN (0, 1)
WHERE tables.schema_id = SCHEMA_ID(N'dbo')
GROUP BY tables.name
ORDER BY tables.name;

SELECT RoleId, RoleName, Description
FROM dbo.Roles
ORDER BY RoleId;

SELECT UserId, RoleName, UserType, FullName, Email, ProfileImagePath, Status
FROM dbo.vw_UserAccounts
ORDER BY UserId;

SELECT CategoryCode, CategoryName, IsActive
FROM dbo.Categories
ORDER BY CategoryCode;

SELECT ServiceId, FreelancerName, CategoryName, Title, ServiceImagePath,
       Price, DeliveryDays, AvailableSlots
FROM dbo.vw_ServiceCatalog
ORDER BY ServiceId;

SELECT SettingKey, SettingValue, Description
FROM dbo.PlatformSettings
ORDER BY SettingKey;
GO
