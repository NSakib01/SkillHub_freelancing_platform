/*
    SkillHub: complete shared SQL Server database foundation.
    Owner: MD. Nazmus Sakib | Student ID: 24-58148-2
    Target: SQL Server LocalDB / SQL Server Express / SQL Server 2016 SP1+

    Run this entire file in SSMS or Visual Studio SQL Server Object Explorer.
    The script is intentionally rerunnable and never drops existing data.
*/

USE [master];
GO

IF DB_ID(N'SkillHubDB') IS NULL
BEGIN
    EXEC(N'CREATE DATABASE [SkillHubDB]');
END;
GO

USE [SkillHubDB];
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

/* ---------------------------------------------------------------
   1. Roles and shared account identities.
   --------------------------------------------------------------- */

IF OBJECT_ID(N'dbo.Roles', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Roles
    (
        RoleId INT IDENTITY(1, 1) NOT NULL,
        RoleName NVARCHAR(30) NOT NULL,
        Description NVARCHAR(200) NULL,
        CreatedAt DATETIME2(0) NOT NULL
            CONSTRAINT DF_Roles_CreatedAt DEFAULT SYSDATETIME(),
        CONSTRAINT PK_Roles PRIMARY KEY (RoleId),
        CONSTRAINT UQ_Roles_RoleName UNIQUE (RoleName),
        CONSTRAINT CK_Roles_RoleName
            CHECK (RoleName IN (N'Admin', N'Freelancer', N'Client'))
    );
END;
GO

IF OBJECT_ID(N'dbo.Users', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Users
    (
        UserId INT IDENTITY(1, 1) NOT NULL,
        RoleId INT NOT NULL,
        FullName NVARCHAR(120) NOT NULL,
        Email NVARCHAR(150) NOT NULL,
        PasswordHash NVARCHAR(300) NOT NULL,
        Phone NVARCHAR(20) NULL,
        Address NVARCHAR(250) NULL,
        Status NVARCHAR(20) NOT NULL
            CONSTRAINT DF_Users_Status DEFAULT N'Active',
        CreatedAt DATETIME2(0) NOT NULL
            CONSTRAINT DF_Users_CreatedAt DEFAULT SYSDATETIME(),
        UpdatedAt DATETIME2(0) NULL,
        LastLoginAt DATETIME2(0) NULL,
        CONSTRAINT PK_Users PRIMARY KEY (UserId),
        CONSTRAINT FK_Users_Roles FOREIGN KEY (RoleId)
            REFERENCES dbo.Roles (RoleId),
        CONSTRAINT UQ_Users_Email UNIQUE (Email),
        CONSTRAINT CK_Users_FullName_NotBlank
            CHECK (LEN(LTRIM(RTRIM(FullName))) >= 2),
        CONSTRAINT CK_Users_Email_HasAtSign
            CHECK (Email LIKE N'%_@_%._%'),
        CONSTRAINT CK_Users_Status
            CHECK (Status IN (N'Active', N'Suspended', N'Deactivated')),
        CONSTRAINT CK_Users_PasswordHash_NotBlank
            CHECK (LEN(PasswordHash) >= 40)
    );
END;
GO

IF OBJECT_ID(N'dbo.ClientProfiles', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ClientProfiles
    (
        UserId INT NOT NULL,
        CompanyName NVARCHAR(120) NULL,
        Notes NVARCHAR(500) NULL,
        CreatedAt DATETIME2(0) NOT NULL
            CONSTRAINT DF_ClientProfiles_CreatedAt DEFAULT SYSDATETIME(),
        UpdatedAt DATETIME2(0) NULL,
        CONSTRAINT PK_ClientProfiles PRIMARY KEY (UserId),
        CONSTRAINT FK_ClientProfiles_Users FOREIGN KEY (UserId)
            REFERENCES dbo.Users (UserId)
    );
END;
GO

IF OBJECT_ID(N'dbo.FreelancerProfiles', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.FreelancerProfiles
    (
        UserId INT NOT NULL,
        ProfessionalTitle NVARCHAR(120) NULL,
        Biography NVARCHAR(1000) NULL,
        Skills NVARCHAR(500) NULL,
        IsVerified BIT NOT NULL
            CONSTRAINT DF_FreelancerProfiles_IsVerified DEFAULT 0,
        AverageRating DECIMAL(3, 2) NOT NULL
            CONSTRAINT DF_FreelancerProfiles_AverageRating DEFAULT 0.00,
        CreatedAt DATETIME2(0) NOT NULL
            CONSTRAINT DF_FreelancerProfiles_CreatedAt DEFAULT SYSDATETIME(),
        UpdatedAt DATETIME2(0) NULL,
        CONSTRAINT PK_FreelancerProfiles PRIMARY KEY (UserId),
        CONSTRAINT FK_FreelancerProfiles_Users FOREIGN KEY (UserId)
            REFERENCES dbo.Users (UserId),
        CONSTRAINT CK_FreelancerProfiles_AverageRating
            CHECK (AverageRating >= 0.00 AND AverageRating <= 5.00)
    );
END;
GO

/* ---------------------------------------------------------------
   2. Software-service catalogue and time-limited offers.
   --------------------------------------------------------------- */

IF OBJECT_ID(N'dbo.Categories', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Categories
    (
        CategoryId INT IDENTITY(1, 1) NOT NULL,
        CategoryCode NVARCHAR(12) NOT NULL,
        CategoryName NVARCHAR(100) NOT NULL,
        Description NVARCHAR(250) NULL,
        IsActive BIT NOT NULL
            CONSTRAINT DF_Categories_IsActive DEFAULT 1,
        CreatedAt DATETIME2(0) NOT NULL
            CONSTRAINT DF_Categories_CreatedAt DEFAULT SYSDATETIME(),
        CONSTRAINT PK_Categories PRIMARY KEY (CategoryId),
        CONSTRAINT UQ_Categories_CategoryCode UNIQUE (CategoryCode),
        CONSTRAINT UQ_Categories_CategoryName UNIQUE (CategoryName)
    );
END;
GO

IF OBJECT_ID(N'dbo.Services', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Services
    (
        ServiceId INT IDENTITY(1, 1) NOT NULL,
        FreelancerId INT NOT NULL,
        CategoryId INT NOT NULL,
        Title NVARCHAR(150) NOT NULL,
        Description NVARCHAR(1500) NOT NULL,
        Price DECIMAL(18, 2) NOT NULL,
        DeliveryDays INT NOT NULL,
        AvailableSlots INT NOT NULL,
        IsActive BIT NOT NULL
            CONSTRAINT DF_Services_IsActive DEFAULT 1,
        CreatedAt DATETIME2(0) NOT NULL
            CONSTRAINT DF_Services_CreatedAt DEFAULT SYSDATETIME(),
        UpdatedAt DATETIME2(0) NULL,
        CONSTRAINT PK_Services PRIMARY KEY (ServiceId),
        CONSTRAINT FK_Services_Users FOREIGN KEY (FreelancerId)
            REFERENCES dbo.Users (UserId),
        CONSTRAINT FK_Services_Categories FOREIGN KEY (CategoryId)
            REFERENCES dbo.Categories (CategoryId),
        CONSTRAINT UQ_Services_ServiceAndFreelancer
            UNIQUE (ServiceId, FreelancerId),
        CONSTRAINT CK_Services_Title_NotBlank
            CHECK (LEN(LTRIM(RTRIM(Title))) >= 3),
        CONSTRAINT CK_Services_Price CHECK (Price >= 0.00),
        CONSTRAINT CK_Services_DeliveryDays CHECK (DeliveryDays > 0),
        CONSTRAINT CK_Services_AvailableSlots CHECK (AvailableSlots >= 0)
    );
END;
GO

IF OBJECT_ID(N'dbo.Offers', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Offers
    (
        OfferId INT IDENTITY(1, 1) NOT NULL,
        ServiceId INT NULL,
        OfferTitle NVARCHAR(120) NOT NULL,
        DiscountPercent DECIMAL(5, 2) NOT NULL,
        StartDate DATETIME2(0) NOT NULL,
        EndDate DATETIME2(0) NOT NULL,
        IsActive BIT NOT NULL
            CONSTRAINT DF_Offers_IsActive DEFAULT 1,
        CreatedBy INT NOT NULL,
        CreatedAt DATETIME2(0) NOT NULL
            CONSTRAINT DF_Offers_CreatedAt DEFAULT SYSDATETIME(),
        CONSTRAINT PK_Offers PRIMARY KEY (OfferId),
        CONSTRAINT FK_Offers_Services FOREIGN KEY (ServiceId)
            REFERENCES dbo.Services (ServiceId),
        CONSTRAINT FK_Offers_CreatedBy FOREIGN KEY (CreatedBy)
            REFERENCES dbo.Users (UserId),
        CONSTRAINT CK_Offers_DiscountPercent
            CHECK (DiscountPercent > 0.00 AND DiscountPercent <= 100.00),
        CONSTRAINT CK_Offers_DateRange CHECK (EndDate >= StartDate)
    );
END;
GO

/* ---------------------------------------------------------------
   3. Client cart and line items.
   --------------------------------------------------------------- */

IF OBJECT_ID(N'dbo.Carts', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Carts
    (
        CartId INT IDENTITY(1, 1) NOT NULL,
        ClientId INT NOT NULL,
        CreatedAt DATETIME2(0) NOT NULL
            CONSTRAINT DF_Carts_CreatedAt DEFAULT SYSDATETIME(),
        UpdatedAt DATETIME2(0) NULL,
        CONSTRAINT PK_Carts PRIMARY KEY (CartId),
        CONSTRAINT FK_Carts_Users FOREIGN KEY (ClientId)
            REFERENCES dbo.Users (UserId),
        CONSTRAINT UQ_Carts_ClientId UNIQUE (ClientId)
    );
END;
GO

IF OBJECT_ID(N'dbo.CartItems', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.CartItems
    (
        CartItemId INT IDENTITY(1, 1) NOT NULL,
        CartId INT NOT NULL,
        ServiceId INT NOT NULL,
        Quantity INT NOT NULL
            CONSTRAINT DF_CartItems_Quantity DEFAULT 1,
        UnitPrice DECIMAL(18, 2) NOT NULL,
        AddedAt DATETIME2(0) NOT NULL
            CONSTRAINT DF_CartItems_AddedAt DEFAULT SYSDATETIME(),
        CONSTRAINT PK_CartItems PRIMARY KEY (CartItemId),
        CONSTRAINT FK_CartItems_Carts FOREIGN KEY (CartId)
            REFERENCES dbo.Carts (CartId) ON DELETE CASCADE,
        CONSTRAINT FK_CartItems_Services FOREIGN KEY (ServiceId)
            REFERENCES dbo.Services (ServiceId),
        CONSTRAINT UQ_CartItems_CartAndService UNIQUE (CartId, ServiceId),
        CONSTRAINT CK_CartItems_Quantity CHECK (Quantity > 0),
        CONSTRAINT CK_CartItems_UnitPrice CHECK (UnitPrice >= 0.00)
    );
END;
GO

/* ---------------------------------------------------------------
   4. Orders, simulated payments and withdrawal requests.
   --------------------------------------------------------------- */

IF OBJECT_ID(N'dbo.Orders', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Orders
    (
        OrderId INT IDENTITY(1, 1) NOT NULL,
        ClientId INT NOT NULL,
        FreelancerId INT NOT NULL,
        ServiceId INT NOT NULL,
        Quantity INT NOT NULL,
        UnitPrice DECIMAL(18, 2) NOT NULL,
        DiscountAmount DECIMAL(18, 2) NOT NULL
            CONSTRAINT DF_Orders_DiscountAmount DEFAULT 0.00,
        GrossAmount DECIMAL(18, 2) NOT NULL,
        CommissionRate DECIMAL(5, 2) NOT NULL,
        CommissionAmount DECIMAL(18, 2) NOT NULL,
        FreelancerEarning DECIMAL(18, 2) NOT NULL,
        OrderStatus NVARCHAR(30) NOT NULL
            CONSTRAINT DF_Orders_OrderStatus DEFAULT N'Pending Payment',
        DeliveryNote NVARCHAR(1000) NULL,
        CreatedAt DATETIME2(0) NOT NULL
            CONSTRAINT DF_Orders_CreatedAt DEFAULT SYSDATETIME(),
        AcceptedAt DATETIME2(0) NULL,
        DeliveredAt DATETIME2(0) NULL,
        CompletedAt DATETIME2(0) NULL,
        CONSTRAINT PK_Orders PRIMARY KEY (OrderId),
        CONSTRAINT UQ_Orders_Identity
            UNIQUE (OrderId, ClientId, FreelancerId),
        CONSTRAINT FK_Orders_Client FOREIGN KEY (ClientId)
            REFERENCES dbo.Users (UserId),
        CONSTRAINT FK_Orders_Freelancer FOREIGN KEY (FreelancerId)
            REFERENCES dbo.Users (UserId),
        CONSTRAINT FK_Orders_ServiceAndFreelancer
            FOREIGN KEY (ServiceId, FreelancerId)
            REFERENCES dbo.Services (ServiceId, FreelancerId),
        CONSTRAINT CK_Orders_DifferentParties CHECK (ClientId <> FreelancerId),
        CONSTRAINT CK_Orders_Quantity CHECK (Quantity > 0),
        CONSTRAINT CK_Orders_UnitPrice CHECK (UnitPrice >= 0.00),
        CONSTRAINT CK_Orders_DiscountAmount
            CHECK (DiscountAmount >= 0.00 AND DiscountAmount <= Quantity * UnitPrice),
        CONSTRAINT CK_Orders_GrossAmount
            CHECK (GrossAmount = ROUND(Quantity * UnitPrice - DiscountAmount, 2)),
        CONSTRAINT CK_Orders_CommissionRate
            CHECK (CommissionRate >= 0.00 AND CommissionRate <= 100.00),
        CONSTRAINT CK_Orders_CommissionAmount
            CHECK (CommissionAmount >= 0.00 AND CommissionAmount <= GrossAmount),
        CONSTRAINT CK_Orders_FreelancerEarning
            CHECK (FreelancerEarning >= 0.00
                AND FreelancerEarning + CommissionAmount = GrossAmount),
        CONSTRAINT CK_Orders_Status CHECK
        (
            OrderStatus IN
            (
                N'Pending Payment', N'Placed', N'In Progress', N'Delivered',
                N'Completed', N'Disputed', N'Cancelled', N'Refunded'
            )
        )
    );
END;
GO

IF OBJECT_ID(N'dbo.Payments', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Payments
    (
        PaymentId INT IDENTITY(1, 1) NOT NULL,
        OrderId INT NOT NULL,
        Amount DECIMAL(18, 2) NOT NULL,
        PaymentMethod NVARCHAR(30) NOT NULL,
        PaymentStatus NVARCHAR(20) NOT NULL
            CONSTRAINT DF_Payments_PaymentStatus DEFAULT N'Pending',
        TransactionReference NVARCHAR(80) NULL,
        PaidAt DATETIME2(0) NULL,
        CreatedAt DATETIME2(0) NOT NULL
            CONSTRAINT DF_Payments_CreatedAt DEFAULT SYSDATETIME(),
        CONSTRAINT PK_Payments PRIMARY KEY (PaymentId),
        CONSTRAINT FK_Payments_Orders FOREIGN KEY (OrderId)
            REFERENCES dbo.Orders (OrderId),
        CONSTRAINT UQ_Payments_OrderId UNIQUE (OrderId),
        CONSTRAINT CK_Payments_Amount CHECK (Amount >= 0.00),
        CONSTRAINT CK_Payments_Method CHECK
        (
            PaymentMethod IN
            (
                N'Simulated Card', N'Simulated Mobile Banking',
                N'Simulated Bank Transfer', N'Demo Payment',
                N'Card', N'Mobile Banking', N'Bank Transfer',
                N'bKash', N'Nagad'
            )
        ),
        CONSTRAINT CK_Payments_Status
            CHECK (PaymentStatus IN (N'Pending', N'Paid', N'Failed', N'Refunded'))
    );
END;
GO

IF OBJECT_ID(N'dbo.WithdrawalRequests', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.WithdrawalRequests
    (
        WithdrawalId INT IDENTITY(1, 1) NOT NULL,
        FreelancerId INT NOT NULL,
        Amount DECIMAL(18, 2) NOT NULL,
        Status NVARCHAR(20) NOT NULL
            CONSTRAINT DF_WithdrawalRequests_Status DEFAULT N'Pending',
        RequestDate DATETIME2(0) NOT NULL
            CONSTRAINT DF_WithdrawalRequests_RequestDate DEFAULT SYSDATETIME(),
        ProcessedBy INT NULL,
        ProcessedAt DATETIME2(0) NULL,
        AdminNote NVARCHAR(500) NULL,
        CONSTRAINT PK_WithdrawalRequests PRIMARY KEY (WithdrawalId),
        CONSTRAINT FK_WithdrawalRequests_Freelancer
            FOREIGN KEY (FreelancerId) REFERENCES dbo.Users (UserId),
        CONSTRAINT FK_WithdrawalRequests_ProcessedBy
            FOREIGN KEY (ProcessedBy) REFERENCES dbo.Users (UserId),
        CONSTRAINT CK_WithdrawalRequests_Amount CHECK (Amount > 0.00),
        CONSTRAINT CK_WithdrawalRequests_Status
            CHECK (Status IN (N'Pending', N'Approved', N'Rejected'))
    );
END;
GO

IF OBJECT_ID(N'dbo.WalletTransactions', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.WalletTransactions
    (
        WalletTxnId INT IDENTITY(1, 1) NOT NULL,
        FreelancerId INT NOT NULL,
        OrderId INT NULL,
        WithdrawalId INT NULL,
        TransactionType NVARCHAR(20) NOT NULL,
        Amount DECIMAL(18, 2) NOT NULL,
        Description NVARCHAR(250) NULL,
        TransactionDate DATETIME2(0) NOT NULL
            CONSTRAINT DF_WalletTransactions_TransactionDate DEFAULT SYSDATETIME(),
        CONSTRAINT PK_WalletTransactions PRIMARY KEY (WalletTxnId),
        CONSTRAINT FK_WalletTransactions_Freelancer
            FOREIGN KEY (FreelancerId) REFERENCES dbo.Users (UserId),
        CONSTRAINT FK_WalletTransactions_Order
            FOREIGN KEY (OrderId) REFERENCES dbo.Orders (OrderId),
        CONSTRAINT FK_WalletTransactions_Withdrawal
            FOREIGN KEY (WithdrawalId)
            REFERENCES dbo.WithdrawalRequests (WithdrawalId),
        CONSTRAINT CK_WalletTransactions_Type
            CHECK (TransactionType IN (N'Credit', N'Withdrawal', N'Refund', N'Adjustment')),
        CONSTRAINT CK_WalletTransactions_Amount CHECK (Amount > 0.00),
        CONSTRAINT CK_WalletTransactions_CreditNeedsOrder
            CHECK (TransactionType <> N'Credit' OR OrderId IS NOT NULL),
        CONSTRAINT CK_WalletTransactions_WithdrawalNeedsRequest
            CHECK (TransactionType <> N'Withdrawal' OR WithdrawalId IS NOT NULL)
    );
END;
GO

/* ---------------------------------------------------------------
   5. Verified reviews, disputes and shared platform settings.
   --------------------------------------------------------------- */

IF OBJECT_ID(N'dbo.Reviews', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Reviews
    (
        ReviewId INT IDENTITY(1, 1) NOT NULL,
        OrderId INT NOT NULL,
        ClientId INT NOT NULL,
        FreelancerId INT NOT NULL,
        Rating TINYINT NOT NULL,
        Comment NVARCHAR(1000) NULL,
        ReviewDate DATETIME2(0) NOT NULL
            CONSTRAINT DF_Reviews_ReviewDate DEFAULT SYSDATETIME(),
        CONSTRAINT PK_Reviews PRIMARY KEY (ReviewId),
        CONSTRAINT UQ_Reviews_OrderId UNIQUE (OrderId),
        CONSTRAINT FK_Reviews_OrderParties
            FOREIGN KEY (OrderId, ClientId, FreelancerId)
            REFERENCES dbo.Orders (OrderId, ClientId, FreelancerId),
        CONSTRAINT CK_Reviews_Rating CHECK (Rating BETWEEN 1 AND 5)
    );
END;
GO

IF OBJECT_ID(N'dbo.Disputes', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Disputes
    (
        DisputeId INT IDENTITY(1, 1) NOT NULL,
        OrderId INT NOT NULL,
        OpenedBy INT NOT NULL,
        Reason NVARCHAR(1000) NOT NULL,
        Status NVARCHAR(20) NOT NULL
            CONSTRAINT DF_Disputes_Status DEFAULT N'Open',
        Resolution NVARCHAR(1000) NULL,
        ResolvedBy INT NULL,
        CreatedAt DATETIME2(0) NOT NULL
            CONSTRAINT DF_Disputes_CreatedAt DEFAULT SYSDATETIME(),
        ResolvedAt DATETIME2(0) NULL,
        CONSTRAINT PK_Disputes PRIMARY KEY (DisputeId),
        CONSTRAINT FK_Disputes_Orders FOREIGN KEY (OrderId)
            REFERENCES dbo.Orders (OrderId),
        CONSTRAINT FK_Disputes_OpenedBy FOREIGN KEY (OpenedBy)
            REFERENCES dbo.Users (UserId),
        CONSTRAINT FK_Disputes_ResolvedBy FOREIGN KEY (ResolvedBy)
            REFERENCES dbo.Users (UserId),
        CONSTRAINT CK_Disputes_Reason_NotBlank
            CHECK (LEN(LTRIM(RTRIM(Reason))) >= 5),
        CONSTRAINT CK_Disputes_Status
            CHECK (Status IN (N'Open', N'Under Review', N'Resolved', N'Rejected'))
    );
END;
GO

IF OBJECT_ID(N'dbo.PlatformSettings', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.PlatformSettings
    (
        SettingKey NVARCHAR(80) NOT NULL,
        SettingValue NVARCHAR(250) NOT NULL,
        Description NVARCHAR(300) NULL,
        UpdatedAt DATETIME2(0) NOT NULL
            CONSTRAINT DF_PlatformSettings_UpdatedAt DEFAULT SYSDATETIME(),
        UpdatedBy INT NULL,
        CONSTRAINT PK_PlatformSettings PRIMARY KEY (SettingKey),
        CONSTRAINT FK_PlatformSettings_UpdatedBy FOREIGN KEY (UpdatedBy)
            REFERENCES dbo.Users (UserId)
    );
END;
GO

/* ---------------------------------------------------------------
   6. Search, join, reporting and duplicate-prevention indexes.
   --------------------------------------------------------------- */

IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.Users') AND name = N'IX_Users_RoleAndStatus'
)
    CREATE NONCLUSTERED INDEX IX_Users_RoleAndStatus
        ON dbo.Users (RoleId, Status) INCLUDE (FullName, Email);
GO

IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.Services') AND name = N'IX_Services_CategorySearch'
)
    CREATE NONCLUSTERED INDEX IX_Services_CategorySearch
        ON dbo.Services (CategoryId, IsActive, AvailableSlots, Price)
        INCLUDE (Title, FreelancerId, DeliveryDays);
GO

IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.Services') AND name = N'IX_Services_Freelancer'
)
    CREATE NONCLUSTERED INDEX IX_Services_Freelancer
        ON dbo.Services (FreelancerId, IsActive, CreatedAt);
GO

IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.Offers') AND name = N'IX_Offers_ActiveDateRange'
)
    CREATE NONCLUSTERED INDEX IX_Offers_ActiveDateRange
        ON dbo.Offers (IsActive, StartDate, EndDate, ServiceId);
GO

IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.Orders') AND name = N'IX_Orders_ClientStatusDate'
)
    CREATE NONCLUSTERED INDEX IX_Orders_ClientStatusDate
        ON dbo.Orders (ClientId, OrderStatus, CreatedAt);
GO

IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.Orders') AND name = N'IX_Orders_FreelancerStatusDate'
)
    CREATE NONCLUSTERED INDEX IX_Orders_FreelancerStatusDate
        ON dbo.Orders (FreelancerId, OrderStatus, CreatedAt);
GO

IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.Payments') AND name = N'IX_Payments_StatusDate'
)
    CREATE NONCLUSTERED INDEX IX_Payments_StatusDate
        ON dbo.Payments (PaymentStatus, PaidAt);
GO

IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.WalletTransactions')
      AND name = N'IX_WalletTransactions_FreelancerDate'
)
    CREATE NONCLUSTERED INDEX IX_WalletTransactions_FreelancerDate
        ON dbo.WalletTransactions (FreelancerId, TransactionDate)
        INCLUDE (TransactionType, Amount);
GO

IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.WalletTransactions')
      AND name = N'UX_WalletTransactions_OneOrderCredit'
)
    CREATE UNIQUE NONCLUSTERED INDEX UX_WalletTransactions_OneOrderCredit
        ON dbo.WalletTransactions (OrderId)
        WHERE TransactionType = N'Credit' AND OrderId IS NOT NULL;
GO

IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.WalletTransactions')
      AND name = N'UX_WalletTransactions_OneWithdrawalDebit'
)
    CREATE UNIQUE NONCLUSTERED INDEX UX_WalletTransactions_OneWithdrawalDebit
        ON dbo.WalletTransactions (WithdrawalId)
        WHERE TransactionType = N'Withdrawal' AND WithdrawalId IS NOT NULL;
GO

IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.WithdrawalRequests')
      AND name = N'IX_WithdrawalRequests_StatusDate'
)
    CREATE NONCLUSTERED INDEX IX_WithdrawalRequests_StatusDate
        ON dbo.WithdrawalRequests (Status, RequestDate, FreelancerId);
GO

IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.Reviews') AND name = N'IX_Reviews_Freelancer'
)
    CREATE NONCLUSTERED INDEX IX_Reviews_Freelancer
        ON dbo.Reviews (FreelancerId, ReviewDate) INCLUDE (Rating);
GO

IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.Disputes') AND name = N'IX_Disputes_StatusDate'
)
    CREATE NONCLUSTERED INDEX IX_Disputes_StatusDate
        ON dbo.Disputes (Status, CreatedAt);
GO

/* ---------------------------------------------------------------
   7. Idempotent seed roles, software categories and demo accounts.
   --------------------------------------------------------------- */

SET XACT_ABORT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    INSERT INTO dbo.Roles (RoleName, Description)
    SELECT seed.RoleName, seed.Description
    FROM
    (
        VALUES
            (N'Admin', N'Platform owner / super administrator'),
            (N'Freelancer', N'Software-service provider'),
            (N'Client', N'Software-service customer')
    ) AS seed (RoleName, Description)
    WHERE NOT EXISTS
    (
        SELECT 1 FROM dbo.Roles AS existing
        WHERE existing.RoleName = seed.RoleName
    );

    INSERT INTO dbo.Categories (CategoryCode, CategoryName, Description)
    SELECT seed.CategoryCode, seed.CategoryName, seed.Description
    FROM
    (
        VALUES
            (N'DEV-01', N'Full-Stack Development', N'Complete software and business applications'),
            (N'DEV-02', N'Frontend Development', N'User interfaces and frontend experiences'),
            (N'DEV-03', N'Backend Development', N'APIs, server logic and databases'),
            (N'MOB-01', N'Android Application Development', N'Android mobile software'),
            (N'MOB-02', N'iOS Application Development', N'iOS mobile software'),
            (N'EMB-01', N'Rust and Firmware Development', N'Embedded, ESP32 and Rust projects'),
            (N'QA-01', N'Quality Assurance', N'Software testing and defect reporting'),
            (N'DAT-01', N'Data Analysis', N'Data preparation, analysis and reporting'),
            (N'SEC-01', N'Cybersecurity', N'Authorized security review and hardening'),
            (N'DES-01', N'Graphic Design', N'Software product graphics and branding'),
            (N'DES-02', N'UI/UX Design', N'Interface and experience design'),
            (N'MED-01', N'Software-Related Animation', N'Software product explainer animation'),
            (N'MED-02', N'Software-Related Video Editing', N'Software demonstration video editing')
    ) AS seed (CategoryCode, CategoryName, Description)
    WHERE NOT EXISTS
    (
        SELECT 1 FROM dbo.Categories AS existing
        WHERE existing.CategoryCode = seed.CategoryCode
    );

    IF NOT EXISTS
    (
        SELECT 1 FROM dbo.PlatformSettings
        WHERE SettingKey = N'CommissionPercent'
    )
    BEGIN
        INSERT INTO dbo.PlatformSettings (SettingKey, SettingValue, Description)
        VALUES
        (
            N'CommissionPercent', N'10.00',
            N'Default platform commission percentage for new orders only.'
        );
    END;

    DECLARE @AdminRoleId INT =
        (SELECT RoleId FROM dbo.Roles WHERE RoleName = N'Admin');
    DECLARE @FreelancerRoleId INT =
        (SELECT RoleId FROM dbo.Roles WHERE RoleName = N'Freelancer');
    DECLARE @ClientRoleId INT =
        (SELECT RoleId FROM dbo.Roles WHERE RoleName = N'Client');

    IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Email = N'admin@skillhub.local')
    BEGIN
        INSERT INTO dbo.Users
        (
            RoleId, FullName, Email, PasswordHash, Phone, Address, Status
        )
        VALUES
        (
            @AdminRoleId,
            N'SkillHub Platform Administrator',
            N'admin@skillhub.local',
            N'PBKDF2-SHA256$120000$U2tpbGxIdWJBZG1pblNlZWQ=$DoJcxgheuzTZBBwSZuN7tPlfTMABbe0aKqCQRf7M50I=',
            N'+8801700000000',
            N'Dhaka, Bangladesh',
            N'Active'
        );
    END;

    IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Email = N'freelancer@skillhub.local')
    BEGIN
        INSERT INTO dbo.Users
        (
            RoleId, FullName, Email, PasswordHash, Phone, Address, Status
        )
        VALUES
        (
            @FreelancerRoleId,
            N'Rafi Ahmed',
            N'freelancer@skillhub.local',
            N'PBKDF2-SHA256$120000$U2tpbGxIdWJGcmVlU2VlZCE=$xJKS/0u8o/XlNX/uUCeaWRrO6N+lukvS7XacweJGCKk=',
            N'+8801700000001',
            N'Dhaka, Bangladesh',
            N'Active'
        );
    END;

    IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Email = N'client@skillhub.local')
    BEGIN
        INSERT INTO dbo.Users
        (
            RoleId, FullName, Email, PasswordHash, Phone, Address, Status
        )
        VALUES
        (
            @ClientRoleId,
            N'Nabila Rahman',
            N'client@skillhub.local',
            N'PBKDF2-SHA256$120000$U2tpbGxIdWJDbGllbnRTZWVk$wwl7WpS2ZgDutEYYKubuYqwE7z2Kh8QaX3hXz6LK5oM=',
            N'+8801700000002',
            N'Dhaka, Bangladesh',
            N'Active'
        );
    END;

    DECLARE @DemoFreelancerId INT =
        (SELECT UserId FROM dbo.Users WHERE Email = N'freelancer@skillhub.local');
    DECLARE @DemoClientId INT =
        (SELECT UserId FROM dbo.Users WHERE Email = N'client@skillhub.local');

    IF NOT EXISTS
    (
        SELECT 1 FROM dbo.FreelancerProfiles
        WHERE UserId = @DemoFreelancerId
    )
    BEGIN
        INSERT INTO dbo.FreelancerProfiles
        (
            UserId, ProfessionalTitle, Biography, Skills, IsVerified
        )
        VALUES
        (
            @DemoFreelancerId,
            N'Full-Stack and Embedded Software Developer',
            N'Develops C# desktop applications, APIs and ESP32 firmware.',
            N'C#, SQL Server, WinForms, REST API, ESP32, Rust',
            1
        );
    END;

    IF NOT EXISTS
    (
        SELECT 1 FROM dbo.ClientProfiles
        WHERE UserId = @DemoClientId
    )
    BEGIN
        INSERT INTO dbo.ClientProfiles (UserId, CompanyName, Notes)
        VALUES
        (
            @DemoClientId,
            N'Nabila Retail Solutions',
            N'Sample client for the SkillHub group demonstration.'
        );
    END;

    IF NOT EXISTS
    (
        SELECT 1 FROM dbo.Carts WHERE ClientId = @DemoClientId
    )
    BEGIN
        INSERT INTO dbo.Carts (ClientId)
        VALUES (@DemoClientId);
    END;

    INSERT INTO dbo.Services
    (
        FreelancerId, CategoryId, Title, Description,
        Price, DeliveryDays, AvailableSlots, IsActive
    )
    SELECT
        @DemoFreelancerId,
        categories.CategoryId,
        seed.Title,
        seed.Description,
        seed.Price,
        seed.DeliveryDays,
        seed.AvailableSlots,
        1
    FROM
    (
        VALUES
            (
                N'DEV-01',
                N'Build a C# WinForms Inventory Application',
                N'Complete desktop inventory solution using C# and SQL Server.',
                CAST(5000.00 AS DECIMAL(18, 2)), 7, 4
            ),
            (
                N'DEV-03',
                N'Design a SQL Server Database and API Backend',
                N'Normalized schema, secure queries and backend integration.',
                CAST(3500.00 AS DECIMAL(18, 2)), 5, 3
            ),
            (
                N'EMB-01',
                N'Develop ESP32 Firmware and Embedded Dashboard',
                N'ESP32 firmware with sensor handling and a local control interface.',
                CAST(6000.00 AS DECIMAL(18, 2)), 10, 2
            )
    ) AS seed
    (
        CategoryCode, Title, Description, Price, DeliveryDays, AvailableSlots
    )
    INNER JOIN dbo.Categories AS categories
        ON categories.CategoryCode = seed.CategoryCode
    WHERE NOT EXISTS
    (
        SELECT 1 FROM dbo.Services AS existing
        WHERE existing.FreelancerId = @DemoFreelancerId
          AND existing.Title = seed.Title
    );

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0
    BEGIN
        ROLLBACK TRANSACTION;
    END;

    THROW;
END CATCH;
GO

/* ---------------------------------------------------------------
   8. Shared read-only integration views for the other three modules.
   --------------------------------------------------------------- */

CREATE OR ALTER VIEW dbo.vw_UserAccounts
AS
SELECT
    users.UserId,
    users.RoleId,
    roles.RoleName,
    CASE roles.RoleName
        WHEN N'Admin' THEN N'SUPER_ADMIN'
        WHEN N'Freelancer' THEN N'ADMIN'
        WHEN N'Client' THEN N'CUSTOMER'
    END AS UserType,
    users.FullName,
    users.Email,
    users.Phone,
    users.Address,
    users.Status,
    users.CreatedAt,
    users.UpdatedAt,
    users.LastLoginAt
FROM dbo.Users AS users
INNER JOIN dbo.Roles AS roles
    ON roles.RoleId = users.RoleId;
GO

CREATE OR ALTER VIEW dbo.vw_ServiceCatalog
AS
SELECT
    services.ServiceId,
    services.FreelancerId,
    freelancer.FullName AS FreelancerName,
    profiles.ProfessionalTitle,
    profiles.IsVerified,
    profiles.AverageRating,
    categories.CategoryId,
    categories.CategoryCode,
    categories.CategoryName,
    services.Title,
    services.Description,
    services.Price,
    services.DeliveryDays,
    services.AvailableSlots,
    services.IsActive,
    services.CreatedAt
FROM dbo.Services AS services
INNER JOIN dbo.Users AS freelancer
    ON freelancer.UserId = services.FreelancerId
INNER JOIN dbo.FreelancerProfiles AS profiles
    ON profiles.UserId = services.FreelancerId
INNER JOIN dbo.Categories AS categories
    ON categories.CategoryId = services.CategoryId
WHERE freelancer.Status = N'Active'
  AND categories.IsActive = 1;
GO

CREATE OR ALTER VIEW dbo.vw_OrderFinancialSummary
AS
SELECT
    orders.OrderId,
    orders.ClientId,
    client.FullName AS ClientName,
    orders.FreelancerId,
    freelancer.FullName AS FreelancerName,
    orders.ServiceId,
    services.Title AS ServiceTitle,
    orders.Quantity,
    orders.UnitPrice,
    orders.DiscountAmount,
    orders.GrossAmount,
    orders.CommissionRate,
    orders.CommissionAmount,
    orders.FreelancerEarning,
    orders.OrderStatus,
    payments.PaymentStatus,
    payments.PaymentMethod,
    orders.CreatedAt,
    orders.CompletedAt
FROM dbo.Orders AS orders
INNER JOIN dbo.Users AS client
    ON client.UserId = orders.ClientId
INNER JOIN dbo.Users AS freelancer
    ON freelancer.UserId = orders.FreelancerId
INNER JOIN dbo.Services AS services
    ON services.ServiceId = orders.ServiceId
LEFT JOIN dbo.Payments AS payments
    ON payments.OrderId = orders.OrderId;
GO

CREATE OR ALTER VIEW dbo.vw_FreelancerWalletBalances
AS
SELECT
    freelancers.UserId AS FreelancerId,
    freelancers.FullName,
    CAST
    (
        COALESCE
        (
            SUM
            (
                CASE
                    WHEN transactions.TransactionType IN (N'Credit', N'Adjustment')
                        THEN transactions.Amount
                    WHEN transactions.TransactionType IN (N'Withdrawal', N'Refund')
                        THEN -transactions.Amount
                    ELSE 0.00
                END
            ),
            0.00
        )
        AS DECIMAL(18, 2)
    ) AS LedgerBalance,
    CAST
    (
        COALESCE
        (
            (
                SELECT SUM(requests.Amount)
                FROM dbo.WithdrawalRequests AS requests
                WHERE requests.FreelancerId = freelancers.UserId
                  AND requests.Status = N'Pending'
            ),
            0.00
        )
        AS DECIMAL(18, 2)
    ) AS PendingWithdrawalAmount,
    CAST
    (
        COALESCE
        (
            SUM
            (
                CASE
                    WHEN transactions.TransactionType IN (N'Credit', N'Adjustment')
                        THEN transactions.Amount
                    WHEN transactions.TransactionType IN (N'Withdrawal', N'Refund')
                        THEN -transactions.Amount
                    ELSE 0.00
                END
            ),
            0.00
        )
        - COALESCE
        (
            (
                SELECT SUM(requests.Amount)
                FROM dbo.WithdrawalRequests AS requests
                WHERE requests.FreelancerId = freelancers.UserId
                  AND requests.Status = N'Pending'
            ),
            0.00
        )
        AS DECIMAL(18, 2)
    ) AS AvailableBalance
FROM dbo.Users AS freelancers
INNER JOIN dbo.Roles AS roles
    ON roles.RoleId = freelancers.RoleId
LEFT JOIN dbo.WalletTransactions AS transactions
    ON transactions.FreelancerId = freelancers.UserId
WHERE roles.RoleName = N'Freelancer'
GROUP BY freelancers.UserId, freelancers.FullName;
GO

/* ---------------------------------------------------------------
   9. Role, accounting and verified-review database guards.
   --------------------------------------------------------------- */

CREATE OR ALTER TRIGGER dbo.tr_ClientProfiles_RequireClientRole
ON dbo.ClientProfiles
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS
    (
        SELECT 1
        FROM inserted AS records
        INNER JOIN dbo.Users AS users ON users.UserId = records.UserId
        INNER JOIN dbo.Roles AS roles ON roles.RoleId = users.RoleId
        WHERE roles.RoleName <> N'Client'
    )
    BEGIN
        THROW 51001, 'A client profile can belong only to a Client account.', 1;
    END;
END;
GO

CREATE OR ALTER TRIGGER dbo.tr_FreelancerProfiles_RequireFreelancerRole
ON dbo.FreelancerProfiles
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS
    (
        SELECT 1
        FROM inserted AS records
        INNER JOIN dbo.Users AS users ON users.UserId = records.UserId
        INNER JOIN dbo.Roles AS roles ON roles.RoleId = users.RoleId
        WHERE roles.RoleName <> N'Freelancer'
    )
    BEGIN
        THROW 51002, 'A freelancer profile can belong only to a Freelancer account.', 1;
    END;
END;
GO

CREATE OR ALTER TRIGGER dbo.tr_Carts_RequireClientRole
ON dbo.Carts
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS
    (
        SELECT 1
        FROM inserted AS records
        INNER JOIN dbo.Users AS users ON users.UserId = records.ClientId
        INNER JOIN dbo.Roles AS roles ON roles.RoleId = users.RoleId
        WHERE roles.RoleName <> N'Client'
    )
    BEGIN
        THROW 51003, 'A cart can belong only to a Client account.', 1;
    END;
END;
GO

CREATE OR ALTER TRIGGER dbo.tr_Services_RequireFreelancerRole
ON dbo.Services
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS
    (
        SELECT 1
        FROM inserted AS records
        INNER JOIN dbo.Users AS users ON users.UserId = records.FreelancerId
        INNER JOIN dbo.Roles AS roles ON roles.RoleId = users.RoleId
        WHERE roles.RoleName <> N'Freelancer'
    )
    BEGIN
        THROW 51004, 'A service can be published only by a Freelancer account.', 1;
    END;
END;
GO

CREATE OR ALTER TRIGGER dbo.tr_Orders_ValidateRolePair
ON dbo.Orders
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS
    (
        SELECT 1
        FROM inserted AS records
        INNER JOIN dbo.Users AS client ON client.UserId = records.ClientId
        INNER JOIN dbo.Roles AS clientRole ON clientRole.RoleId = client.RoleId
        INNER JOIN dbo.Users AS freelancer
            ON freelancer.UserId = records.FreelancerId
        INNER JOIN dbo.Roles AS freelancerRole
            ON freelancerRole.RoleId = freelancer.RoleId
        WHERE clientRole.RoleName <> N'Client'
           OR freelancerRole.RoleName <> N'Freelancer'
    )
    BEGIN
        THROW 51005, 'An order requires one Client and its owning Freelancer.', 1;
    END;
END;
GO

CREATE OR ALTER TRIGGER dbo.tr_Reviews_ValidateCompletedOrderAndRefreshRating
ON dbo.Reviews
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS
    (
        SELECT 1
        FROM inserted AS records
        INNER JOIN dbo.Orders AS orders ON orders.OrderId = records.OrderId
        WHERE orders.OrderStatus <> N'Completed'
    )
    BEGIN
        THROW 51006, 'Only a completed order can receive a verified review.', 1;
    END;

    ;WITH AffectedFreelancers AS
    (
        SELECT FreelancerId FROM inserted
        UNION
        SELECT FreelancerId FROM deleted
    )
    UPDATE profiles
    SET
        AverageRating = COALESCE
        (
            (
                SELECT CAST(AVG(CAST(reviews.Rating AS DECIMAL(5, 2))) AS DECIMAL(3, 2))
                FROM dbo.Reviews AS reviews
                WHERE reviews.FreelancerId = profiles.UserId
            ),
            0.00
        ),
        UpdatedAt = SYSDATETIME()
    FROM dbo.FreelancerProfiles AS profiles
    INNER JOIN AffectedFreelancers AS affected
        ON affected.FreelancerId = profiles.UserId;
END;
GO

CREATE OR ALTER TRIGGER dbo.tr_WalletTransactions_ValidateSettlement
ON dbo.WalletTransactions
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS
    (
        SELECT 1
        FROM inserted AS records
        INNER JOIN dbo.Orders AS orders ON orders.OrderId = records.OrderId
        WHERE records.TransactionType = N'Credit'
          AND
          (
              orders.OrderStatus <> N'Completed'
              OR orders.FreelancerId <> records.FreelancerId
              OR orders.FreelancerEarning <> records.Amount
          )
    )
    BEGIN
        THROW 51007, 'Wallet credit must match the completed order freelancer and earning.', 1;
    END;

    IF EXISTS
    (
        SELECT 1
        FROM inserted AS records
        INNER JOIN dbo.WithdrawalRequests AS requests
            ON requests.WithdrawalId = records.WithdrawalId
        WHERE records.TransactionType = N'Withdrawal'
          AND
          (
              requests.Status <> N'Approved'
              OR requests.FreelancerId <> records.FreelancerId
              OR requests.Amount <> records.Amount
          )
    )
    BEGIN
        THROW 51008, 'Wallet debit must match an approved withdrawal request.', 1;
    END;
END;
GO

CREATE OR ALTER TRIGGER dbo.tr_Users_ProtectRoleAndLastAdmin
ON dbo.Users
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS
    (
        SELECT 1
        FROM inserted AS currentRows
        INNER JOIN deleted AS previousRows
            ON previousRows.UserId = currentRows.UserId
        WHERE currentRows.RoleId <> previousRows.RoleId
    )
    BEGIN
        THROW 51009, 'An existing account role cannot be changed after registration.', 1;
    END;

    IF EXISTS
    (
        SELECT 1
        FROM deleted AS previousRows
        INNER JOIN dbo.Roles AS roles
            ON roles.RoleId = previousRows.RoleId
        WHERE roles.RoleName = N'Admin'
          AND previousRows.Status = N'Active'
    )
    AND NOT EXISTS
    (
        SELECT 1
        FROM dbo.Users AS users
        INNER JOIN dbo.Roles AS roles
            ON roles.RoleId = users.RoleId
        WHERE roles.RoleName = N'Admin'
          AND users.Status = N'Active'
    )
    BEGIN
        THROW 51010, 'At least one active platform administrator must remain.', 1;
    END;
END;
GO

PRINT N'SkillHubDB setup completed successfully.';
PRINT N'Expected foundation: 16 tables, 3 roles, 13 software-service categories and 3 demo accounts.';
PRINT N'Next: run Database/SkillHub_Verification.sql, then open SkillHub.sln.';
GO
