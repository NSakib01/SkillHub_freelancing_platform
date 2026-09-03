using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SkillHub.Data;
using SkillHub.Utilities;

namespace SkillHub.Services
{
    public sealed class CheckoutService
    {
        private readonly DatabaseConnection _database;

    public CheckoutService()
        {
            _database = new DatabaseConnection();
        }

        public CheckoutResult Checkout()
        {
            int clientId = UserSession.UserId;

            using (SqlConnection connection = _database.OpenConnection())
            using (SqlTransaction transaction =
                   connection.BeginTransaction(IsolationLevel.Serializable))
            {
                try
                {
                    List<CheckoutCartItem> cartItems =
                        LoadCartItems(
                            connection,
                            transaction,
                            clientId);

                    if (cartItems.Count == 0)
                    {
                        throw new InvalidOperationException(
                            "Your cart is empty. Add at least one service before checkout.");
                    }

                    decimal commissionRate =
                        LoadCommissionRate(
                            connection,
                            transaction);

                    ValidateCartItems(
                        connection,
                        transaction,
                        cartItems);

                    List<int> createdOrderIds =
                        new List<int>();

                    foreach (CheckoutCartItem item in cartItems)
                    {
                        decimal grossAmount =
                            Math.Round(
                                item.UnitPrice * item.Quantity,
                                2,
                                MidpointRounding.AwayFromZero);

                        decimal discountAmount = 0m;

                        decimal commissionAmount =
                            Math.Round(
                                grossAmount * commissionRate / 100m,
                                2,
                                MidpointRounding.AwayFromZero);

                        decimal freelancerEarning =
                            grossAmount
                            - discountAmount
                            - commissionAmount;

                        int orderId =
                            InsertOrder(
                                connection,
                                transaction,
                                clientId,
                                item,
                                discountAmount,
                                grossAmount,
                                commissionRate,
                                commissionAmount,
                                freelancerEarning);

                        InsertPayment(
                            connection,
                            transaction,
                            orderId,
                            grossAmount - discountAmount);

                        createdOrderIds.Add(orderId);
                    }

                    ClearCart(
                        connection,
                        transaction,
                        clientId);

                    transaction.Commit();

                    decimal totalAmount = 0m;

                    foreach (CheckoutCartItem item in cartItems)
                    {
                        totalAmount +=
                            Math.Round(
                                item.UnitPrice * item.Quantity,
                                2,
                                MidpointRounding.AwayFromZero);
                    }

                    return new CheckoutResult
                    {
                        Success = true,
                        OrderIds = createdOrderIds,
                        TotalAmount = totalAmount,
                        Message =
                            createdOrderIds.Count +
                            " order(s) placed successfully."
                    };
                }
                catch
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch
                    {
                    }

                    throw;
                }
            }
        }

        private List<CheckoutCartItem> LoadCartItems(
            SqlConnection connection,
            SqlTransaction transaction,
            int clientId)
        {
            List<CheckoutCartItem> items =
                new List<CheckoutCartItem>();

            const string sql = @"
            SELECT
                ci.CartItemId,
                ci.ServiceId,
                ci.Quantity,
                ci.UnitPrice,
                svc.FreelancerId,
                svc.Title,
                svc.AvailableSlots,
                svc.IsActive,
                svc.Price AS CurrentPrice
            FROM dbo.CartItems AS ci WITH (UPDLOCK, HOLDLOCK)
            INNER JOIN dbo.Carts AS c WITH (UPDLOCK, HOLDLOCK)
                ON c.CartId = ci.CartId
            INNER JOIN dbo.vw_ServiceCatalog AS svc
                ON svc.ServiceId = ci.ServiceId
            WHERE c.ClientId = @ClientId
            ORDER BY ci.CartItemId;";

            using (SqlCommand command =
                   new SqlCommand(
                       sql,
                       connection,
                       transaction))
            {
                DatabaseConnection.AddParameter(
                    command,
                    "@ClientId",
                    SqlDbType.Int,
                    clientId);

                using (SqlDataReader reader =
                       command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CheckoutCartItem item =
                            new CheckoutCartItem();

                        item.CartItemId =
                            Convert.ToInt32(
                                reader["CartItemId"]);

                        item.ServiceId =
                            Convert.ToInt32(
                                reader["ServiceId"]);

                        item.Quantity =
                            Convert.ToInt32(
                                reader["Quantity"]);

                        item.UnitPrice =
                            Convert.ToDecimal(
                                reader["UnitPrice"]);

                        item.FreelancerId =
                            Convert.ToInt32(
                                reader["FreelancerId"]);

                        item.Title =
                            Convert.ToString(
                                reader["Title"]);

                        item.AvailableSlots =
                            Convert.ToInt32(
                                reader["AvailableSlots"]);

                        item.IsActive =
                            Convert.ToBoolean(
                                reader["IsActive"]);

                        item.CurrentPrice =
                            Convert.ToDecimal(
                                reader["CurrentPrice"]);

                        items.Add(item);
                    }
                }
            }

            return items;
        }

        private decimal LoadCommissionRate(
            SqlConnection connection,
            SqlTransaction transaction)
        {
            const string sql = @"
            SELECT TOP (1)
                TRY_CONVERT(
                    DECIMAL(5, 2),
                    SettingValue)
            FROM dbo.PlatformSettings
            WHERE SettingKey = N'CommissionPercent';";

            using (SqlCommand command =
                   new SqlCommand(
                       sql,
                       connection,
                       transaction))
            {
                object result =
                    command.ExecuteScalar();

                if (result == null ||
                    result == DBNull.Value)
                {
                    throw new InvalidOperationException(
                        "The platform commission percentage is not configured.");
                }

                decimal commissionRate =
                    Convert.ToDecimal(result);

                if (commissionRate < 0m ||
                    commissionRate > 100m)
                {
                    throw new InvalidOperationException(
                        "The platform commission percentage is invalid.");
                }

                return commissionRate;
            }
        }

        private void ValidateCartItems(
            SqlConnection connection,
            SqlTransaction transaction,
            List<CheckoutCartItem> items)
        {
            foreach (CheckoutCartItem item in items)
            {
                if (!item.IsActive)
                {
                    throw new InvalidOperationException(
                        "The service '" +
                        item.Title +
                        "' is no longer active.");
                }

                if (item.AvailableSlots <= 0)
                {
                    throw new InvalidOperationException(
                        "The service '" +
                        item.Title +
                        "' has no available slots.");
                }

                if (item.Quantity <= 0)
                {
                    throw new InvalidOperationException(
                        "Invalid quantity for service '" +
                        item.Title +
                        "'.");
                }

                if (item.Quantity > item.AvailableSlots)
                {
                    throw new InvalidOperationException(
                        "The requested quantity for '" +
                        item.Title +
                        "' exceeds its available slots.");
                }

                if (item.CurrentPrice < 0m)
                {
                    throw new InvalidOperationException(
                        "The service '" +
                        item.Title +
                        "' has an invalid price.");
                }

                item.UnitPrice =
                    item.CurrentPrice;
            }
        }

        private int InsertOrder(
            SqlConnection connection,
            SqlTransaction transaction,
            int clientId,
            CheckoutCartItem item,
            decimal discountAmount,
            decimal grossAmount,
            decimal commissionRate,
            decimal commissionAmount,
            decimal freelancerEarning)
        {
            const string sql = @"
            INSERT INTO dbo.Orders
            (
                ClientId,
                FreelancerId,
                ServiceId,
                Quantity,
                UnitPrice,
                DiscountAmount,
                GrossAmount,
                CommissionRate,
                CommissionAmount,
                FreelancerEarning,
                OrderStatus
            )
            VALUES
            (
                @ClientId,
                @FreelancerId,
                @ServiceId,
                @Quantity,
                @UnitPrice,
                @DiscountAmount,
                @GrossAmount,
                @CommissionRate,
                @CommissionAmount,
                @FreelancerEarning,
                N'Placed'
            );

            SELECT CAST(
                SCOPE_IDENTITY() AS INT);";

            using (SqlCommand command =
                   new SqlCommand(
                       sql,
                       connection,
                       transaction))
            {
                DatabaseConnection.AddParameter(
                    command,
                    "@ClientId",
                    SqlDbType.Int,
                    clientId);

                DatabaseConnection.AddParameter(
                    command,
                    "@FreelancerId",
                    SqlDbType.Int,
                    item.FreelancerId);

                DatabaseConnection.AddParameter(
                    command,
                    "@ServiceId",
                    SqlDbType.Int,
                    item.ServiceId);

                DatabaseConnection.AddParameter(
                    command,
                    "@Quantity",
                    SqlDbType.Int,
                    item.Quantity);

                DatabaseConnection.AddParameter(
                    command,
                    "@UnitPrice",
                    SqlDbType.Decimal,
                    item.UnitPrice);

                DatabaseConnection.AddParameter(
                    command,
                    "@DiscountAmount",
                    SqlDbType.Decimal,
                    discountAmount);

                DatabaseConnection.AddParameter(
                    command,
                    "@GrossAmount",
                    SqlDbType.Decimal,
                    grossAmount);

                DatabaseConnection.AddParameter(
                    command,
                    "@CommissionRate",
                    SqlDbType.Decimal,
                    commissionRate);

                DatabaseConnection.AddParameter(
                    command,
                    "@CommissionAmount",
                    SqlDbType.Decimal,
                    commissionAmount);

                DatabaseConnection.AddParameter(
                    command,
                    "@FreelancerEarning",
                    SqlDbType.Decimal,
                    freelancerEarning);

                object result =
                    command.ExecuteScalar();

                if (result == null ||
                    result == DBNull.Value)
                {
                    throw new InvalidOperationException(
                        "The order could not be created.");
                }

                return Convert.ToInt32(result);
            }
        }

        private void InsertPayment(
            SqlConnection connection,
            SqlTransaction transaction,
            int orderId,
            decimal amount)
        {
            string transactionReference =
                "SH-" +
                Guid.NewGuid()
                    .ToString("N")
                    .ToUpperInvariant();

            const string sql = @"
            INSERT INTO dbo.Payments
            (
                OrderId,
                Amount,
                PaymentMethod,
                PaymentStatus,
                TransactionReference,
                PaidAt,
                CreatedAt
            )
            VALUES
            (
                @OrderId,
                @Amount,
                N'Demo Payment',
                N'Paid',
                @TransactionReference,
                SYSUTCDATETIME(),
                SYSUTCDATETIME()
            );";

            using (SqlCommand command =
                   new SqlCommand(
                       sql,
                       connection,
                       transaction))
            {
                DatabaseConnection.AddParameter(
                    command,
                    "@OrderId",
                    SqlDbType.Int,
                    orderId);

                DatabaseConnection.AddParameter(
                    command,
                    "@Amount",
                    SqlDbType.Decimal,
                    amount);

                DatabaseConnection.AddParameter(
                    command,
                    "@TransactionReference",
                    SqlDbType.NVarChar,
                    transactionReference,
                    80);

                command.ExecuteNonQuery();
            }
        }

        private void ClearCart(
            SqlConnection connection,
            SqlTransaction transaction,
            int clientId)
        {
            const string sql = @"
            DELETE ci
            FROM dbo.CartItems AS ci
            INNER JOIN dbo.Carts AS c
                ON c.CartId = ci.CartId
            WHERE c.ClientId = @ClientId;";

            using (SqlCommand command =
                   new SqlCommand(
                       sql,
                       connection,
                       transaction))
            {
                DatabaseConnection.AddParameter(
                    command,
                    "@ClientId",
                    SqlDbType.Int,
                    clientId);

                command.ExecuteNonQuery();
            }
        }

        private sealed class CheckoutCartItem
        {
            public int CartItemId { get; set; }

            public int ServiceId { get; set; }

            public int Quantity { get; set; }

            public decimal UnitPrice { get; set; }

            public int FreelancerId { get; set; }

            public string Title { get; set; }

            public int AvailableSlots { get; set; }

            public bool IsActive { get; set; }

            public decimal CurrentPrice { get; set; }
        }
    }

    public sealed class CheckoutResult
    {
        public bool Success { get; set; }

        public List<int> OrderIds { get; set; }

        public decimal TotalAmount { get; set; }

        public string Message { get; set; }
    }


}
