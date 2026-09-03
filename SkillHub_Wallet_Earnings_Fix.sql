/*
    SkillHub completed-order wallet repair.

    Run this once against an existing SkillHubDB created by an older build.
    It repairs already completed paid orders and installs automatic settlement
    for future client approvals. The script is safe to run more than once.
*/

USE [SkillHubDB];
GO

SET XACT_ABORT ON;
SET NOCOUNT ON;
GO

IF OBJECT_ID(N'dbo.Orders', N'U') IS NULL
   OR OBJECT_ID(N'dbo.Payments', N'U') IS NULL
   OR OBJECT_ID(N'dbo.WalletTransactions', N'U') IS NULL
BEGIN
    THROW 52100, 'Run SkillHubDatabase.sql before applying the wallet repair.', 1;
END;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.WalletTransactions')
      AND name = N'UX_WalletTransactions_OneOrderCredit'
)
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX UX_WalletTransactions_OneOrderCredit
        ON dbo.WalletTransactions (OrderId)
        WHERE TransactionType = N'Credit' AND OrderId IS NOT NULL;
END;
GO

CREATE OR ALTER TRIGGER dbo.tr_Orders_CreateWalletCreditOnCompletion
ON dbo.Orders
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS
    (
        SELECT 1
        FROM inserted AS currentRows
        INNER JOIN deleted AS previousRows
            ON previousRows.OrderId = currentRows.OrderId
        WHERE currentRows.OrderStatus = N'Completed'
          AND previousRows.OrderStatus <> N'Completed'
          AND currentRows.FreelancerEarning > 0.00
          AND NOT EXISTS
          (
              SELECT 1
              FROM dbo.Payments AS payments
              WHERE payments.OrderId = currentRows.OrderId
                AND payments.PaymentStatus = N'Paid'
          )
    )
    BEGIN
        THROW 51011, 'A paid payment is required before an order can be completed.', 1;
    END;

    UPDATE orders
    SET CompletedAt = COALESCE(orders.CompletedAt, SYSUTCDATETIME())
    FROM dbo.Orders AS orders
    INNER JOIN inserted AS currentRows
        ON currentRows.OrderId = orders.OrderId
    INNER JOIN deleted AS previousRows
        ON previousRows.OrderId = currentRows.OrderId
    WHERE currentRows.OrderStatus = N'Completed'
      AND previousRows.OrderStatus <> N'Completed'
      AND orders.CompletedAt IS NULL;

    INSERT INTO dbo.WalletTransactions
    (
        FreelancerId,
        OrderId,
        TransactionType,
        Amount,
        Description,
        TransactionDate
    )
    SELECT
        currentRows.FreelancerId,
        currentRows.OrderId,
        N'Credit',
        currentRows.FreelancerEarning,
        N'Earning released for completed order #'
            + CONVERT(NVARCHAR(20), currentRows.OrderId),
        SYSUTCDATETIME()
    FROM inserted AS currentRows
    INNER JOIN deleted AS previousRows
        ON previousRows.OrderId = currentRows.OrderId
    WHERE currentRows.OrderStatus = N'Completed'
      AND previousRows.OrderStatus <> N'Completed'
      AND currentRows.FreelancerEarning > 0.00
      AND NOT EXISTS
      (
          SELECT 1
          FROM dbo.WalletTransactions AS existing
          WHERE existing.OrderId = currentRows.OrderId
            AND existing.TransactionType = N'Credit'
      );
END;
GO

BEGIN TRY
    BEGIN TRANSACTION;

    UPDATE dbo.Orders
    SET CompletedAt = COALESCE(CompletedAt, SYSUTCDATETIME())
    WHERE OrderStatus = N'Completed';

    INSERT INTO dbo.WalletTransactions
    (
        FreelancerId,
        OrderId,
        TransactionType,
        Amount,
        Description,
        TransactionDate
    )
    SELECT
        orders.FreelancerId,
        orders.OrderId,
        N'Credit',
        orders.FreelancerEarning,
        N'Earning repaired for completed order #'
            + CONVERT(NVARCHAR(20), orders.OrderId),
        COALESCE(orders.CompletedAt, SYSUTCDATETIME())
    FROM dbo.Orders AS orders
    INNER JOIN dbo.Payments AS payments
        ON payments.OrderId = orders.OrderId
       AND payments.PaymentStatus = N'Paid'
    WHERE orders.OrderStatus = N'Completed'
      AND orders.FreelancerEarning > 0.00
      AND NOT EXISTS
      (
          SELECT 1
          FROM dbo.WalletTransactions AS existing
              WITH (UPDLOCK, HOLDLOCK)
          WHERE existing.OrderId = orders.OrderId
            AND existing.TransactionType = N'Credit'
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

IF EXISTS
(
    SELECT 1
    FROM dbo.Orders AS orders
    INNER JOIN dbo.Payments AS payments
        ON payments.OrderId = orders.OrderId
       AND payments.PaymentStatus = N'Paid'
    WHERE orders.OrderStatus = N'Completed'
      AND orders.FreelancerEarning > 0.00
      AND NOT EXISTS
      (
          SELECT 1
          FROM dbo.WalletTransactions AS transactions
          WHERE transactions.OrderId = orders.OrderId
            AND transactions.TransactionType = N'Credit'
      )
)
BEGIN
    THROW 52101, 'One or more completed orders could not be credited.', 1;
END;

IF EXISTS
(
    SELECT 1
    FROM dbo.Orders
    WHERE OrderStatus = N'Completed'
      AND CompletedAt IS NULL
)
BEGIN
    THROW 52102, 'One or more completed orders still lack a completion timestamp.', 1;
END;

PRINT N'PASS: historical completed-order earnings were repaired.';
PRINT N'PASS: future completed orders will be credited automatically.';
PRINT N'PASS: completed-order timestamps are populated.';

SELECT
    orders.OrderId,
    freelancer.FullName AS FreelancerName,
    orders.FreelancerEarning,
    orders.CompletedAt,
    transactions.WalletTxnId,
    transactions.TransactionDate
FROM dbo.Orders AS orders
INNER JOIN dbo.Users AS freelancer
    ON freelancer.UserId = orders.FreelancerId
LEFT JOIN dbo.WalletTransactions AS transactions
    ON transactions.OrderId = orders.OrderId
   AND transactions.TransactionType = N'Credit'
WHERE orders.OrderStatus = N'Completed'
ORDER BY orders.OrderId DESC;
GO
