using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SkillHub.Data;
using SkillHub.Models;
using SkillHub.Utilities;

namespace SkillHub.Repositories
{
    public sealed class OrderRepository
    {
        private readonly DatabaseConnection _database;

        public OrderRepository()
        {
            _database = new DatabaseConnection();
        }

        public List<OrderModel> GetClientOrders()
        {
            int clientId = UserSession.UserId;

            List<OrderModel> orders = new List<OrderModel>();

            const string sql = @"
                SELECT
                    o.OrderId,
                    o.ClientId,
                    o.FreelancerId,
                    o.ServiceId,
                    s.Title AS ServiceTitle,
                    u.FullName AS FreelancerName,
                    o.Quantity,
                    o.UnitPrice,
                    o.DiscountAmount,
                    o.GrossAmount,
                    o.CommissionRate,
                    o.CommissionAmount,
                    o.FreelancerEarning,
                    o.OrderStatus
                FROM dbo.Orders AS o
                LEFT JOIN dbo.Services AS s
                    ON s.ServiceId = o.ServiceId
                LEFT JOIN dbo.Users AS u
                    ON u.UserId = o.FreelancerId
                WHERE o.ClientId = @ClientId
                ORDER BY o.OrderId DESC;";

            using (SqlConnection connection = _database.OpenConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                DatabaseConnection.AddParameter(
                    command,
                    "@ClientId",
                    SqlDbType.Int,
                    clientId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        orders.Add(MapOrder(reader));
                    }
                }
            }

            return orders;
        }

        public List<OrderModel> GetClientOrdersByStatus(string status)
        {
            int clientId = UserSession.UserId;

            List<OrderModel> orders = new List<OrderModel>();

            const string sql = @"
                SELECT
                    o.OrderId,
                    o.ClientId,
                    o.FreelancerId,
                    o.ServiceId,
                    s.Title AS ServiceTitle,
                    u.FullName AS FreelancerName,
                    o.Quantity,
                    o.UnitPrice,
                    o.DiscountAmount,
                    o.GrossAmount,
                    o.CommissionRate,
                    o.CommissionAmount,
                    o.FreelancerEarning,
                    o.OrderStatus
                FROM dbo.Orders AS o
                LEFT JOIN dbo.Services AS s
                    ON s.ServiceId = o.ServiceId
                LEFT JOIN dbo.Users AS u
                    ON u.UserId = o.FreelancerId
                WHERE o.ClientId = @ClientId
                  AND o.OrderStatus = @OrderStatus
                ORDER BY o.OrderId DESC;";

            using (SqlConnection connection = _database.OpenConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                DatabaseConnection.AddParameter(
                    command,
                    "@ClientId",
                    SqlDbType.Int,
                    clientId);

                DatabaseConnection.AddParameter(
                    command,
                    "@OrderStatus",
                    SqlDbType.NVarChar,
                    status ?? string.Empty,
                    50);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        orders.Add(MapOrder(reader));
                    }
                }
            }

            return orders;
        }

        public OrderModel GetClientOrderById(int orderId)
        {
            if (orderId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(orderId));
            }

            int clientId = UserSession.UserId;

            const string sql = @"
                SELECT
                    o.OrderId,
                    o.ClientId,
                    o.FreelancerId,
                    o.ServiceId,
                    s.Title AS ServiceTitle,
                    u.FullName AS FreelancerName,
                    o.Quantity,
                    o.UnitPrice,
                    o.DiscountAmount,
                    o.GrossAmount,
                    o.CommissionRate,
                    o.CommissionAmount,
                    o.FreelancerEarning,
                    o.OrderStatus
                FROM dbo.Orders AS o
                LEFT JOIN dbo.Services AS s
                    ON s.ServiceId = o.ServiceId
                LEFT JOIN dbo.Users AS u
                    ON u.UserId = o.FreelancerId
                WHERE o.OrderId = @OrderId
                  AND o.ClientId = @ClientId;";

            using (SqlConnection connection = _database.OpenConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                DatabaseConnection.AddParameter(
                    command,
                    "@OrderId",
                    SqlDbType.Int,
                    orderId);

                DatabaseConnection.AddParameter(
                    command,
                    "@ClientId",
                    SqlDbType.Int,
                    clientId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapOrder(reader);
                    }
                }
            }

            return null;
        }

        public void ApproveCompletion(int orderId)
        {
            if (orderId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(orderId));
            }

            int clientId = UserSession.UserId;

            const string sql = @"
                UPDATE dbo.Orders
                SET OrderStatus = N'Completed'
                WHERE OrderId = @OrderId
                  AND ClientId = @ClientId
                  AND OrderStatus = N'Delivered';

                IF @@ROWCOUNT = 0
                BEGIN
                    THROW 51006, 'The order cannot be approved for completion.', 1;
                END;";

            using (SqlConnection connection = _database.OpenConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                DatabaseConnection.AddParameter(
                    command,
                    "@OrderId",
                    SqlDbType.Int,
                    orderId);

                DatabaseConnection.AddParameter(
                    command,
                    "@ClientId",
                    SqlDbType.Int,
                    clientId);

                command.ExecuteNonQuery();
            }
        }

        private static OrderModel MapOrder(SqlDataReader reader)
        {
            OrderModel order = new OrderModel();

            order.OrderId = Convert.ToInt32(reader["OrderId"]);
            order.ClientId = Convert.ToInt32(reader["ClientId"]);
            order.FreelancerId = Convert.ToInt32(reader["FreelancerId"]);
            order.ServiceId = Convert.ToInt32(reader["ServiceId"]);
            order.ServiceTitle = Convert.ToString(reader["ServiceTitle"]);
            order.FreelancerName = Convert.ToString(reader["FreelancerName"]);
            order.Quantity = Convert.ToInt32(reader["Quantity"]);
            order.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
            order.DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"]);
            order.GrossAmount = Convert.ToDecimal(reader["GrossAmount"]);
            order.CommissionRate = Convert.ToDecimal(reader["CommissionRate"]);
            order.CommissionAmount = Convert.ToDecimal(reader["CommissionAmount"]);
            order.FreelancerEarning = Convert.ToDecimal(reader["FreelancerEarning"]);
            order.OrderStatus = Convert.ToString(reader["OrderStatus"]);

            return order;
        }
    }
}
