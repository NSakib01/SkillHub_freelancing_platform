using SkillHub.Data;
using SkillHub.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SkillHub.Repositories
{
    public class FreelancerOrderRepository
    {
        // ============================================================
        // GET FREELANCER ORDERS
        // ============================================================

        public List<Order> GetByFreelancer(int freelancerId)
        {
            List<Order> orders = new List<Order>();

            DatabaseConnection databaseConnection =
                new DatabaseConnection();

            using (SqlConnection connection =
                   databaseConnection.CreateConnection())
            {
                connection.Open();

                string query = @"
                    SELECT
                        o.OrderId,
                        o.ClientId,
                        client.FullName AS ClientName,
                        o.FreelancerId,
                        o.ServiceId,
                        s.Title AS ServiceTitle,
                        o.Quantity,
                        o.UnitPrice,
                        o.DiscountAmount,
                        o.GrossAmount,
                        o.CommissionRate,
                        o.CommissionAmount,
                        o.FreelancerEarning,
                        o.OrderStatus,
                        o.DeliveryNote,
                        o.CreatedAt,
                        o.AcceptedAt,
                        o.DeliveredAt,
                        o.CompletedAt
                    FROM dbo.Orders o
                    INNER JOIN dbo.Users client
                        ON client.UserId = o.ClientId
                    INNER JOIN dbo.Services s
                        ON s.ServiceId = o.ServiceId
                    WHERE o.FreelancerId = @FreelancerId
                    ORDER BY o.CreatedAt DESC;";

                using (SqlCommand command =
                       new SqlCommand(query, connection))
                {
                    command.Parameters.Add(
                        "@FreelancerId",
                        SqlDbType.Int).Value = freelancerId;

                    using (SqlDataReader reader =
                           command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(new Order
                            {
                                OrderId =
                                    Convert.ToInt32(
                                        reader["OrderId"]),

                                ClientId =
                                    Convert.ToInt32(
                                        reader["ClientId"]),

                                ClientName =
                                    reader["ClientName"].ToString(),

                                FreelancerId =
                                    Convert.ToInt32(
                                        reader["FreelancerId"]),

                                ServiceId =
                                    Convert.ToInt32(
                                        reader["ServiceId"]),

                                ServiceTitle =
                                    reader["ServiceTitle"].ToString(),

                                Quantity =
                                    Convert.ToInt32(
                                        reader["Quantity"]),

                                UnitPrice =
                                    Convert.ToDecimal(
                                        reader["UnitPrice"]),

                                DiscountAmount =
                                    Convert.ToDecimal(
                                        reader["DiscountAmount"]),

                                GrossAmount =
                                    Convert.ToDecimal(
                                        reader["GrossAmount"]),

                                CommissionRate =
                                    Convert.ToDecimal(
                                        reader["CommissionRate"]),

                                CommissionAmount =
                                    Convert.ToDecimal(
                                        reader["CommissionAmount"]),

                                FreelancerEarning =
                                    Convert.ToDecimal(
                                        reader["FreelancerEarning"]),

                                OrderStatus =
                                    reader["OrderStatus"].ToString(),

                                DeliveryNote =
                                    reader["DeliveryNote"] == DBNull.Value
                                        ? null
                                        : reader["DeliveryNote"].ToString(),

                                CreatedAt =
                                    Convert.ToDateTime(
                                        reader["CreatedAt"]),

                                AcceptedAt =
                                    reader["AcceptedAt"] == DBNull.Value
                                        ? (DateTime?)null
                                        : Convert.ToDateTime(
                                            reader["AcceptedAt"]),

                                DeliveredAt =
                                    reader["DeliveredAt"] == DBNull.Value
                                        ? (DateTime?)null
                                        : Convert.ToDateTime(
                                            reader["DeliveredAt"]),

                                CompletedAt =
                                    reader["CompletedAt"] == DBNull.Value
                                        ? (DateTime?)null
                                        : Convert.ToDateTime(
                                            reader["CompletedAt"])
                            });
                        }
                    }
                }
            }

            return orders;
        }


        // ============================================================
        // ACCEPT ORDER
        // Placed -> In Progress
        // ============================================================

        public bool AcceptOrder(
            int orderId,
            int freelancerId)
        {
            DatabaseConnection databaseConnection =
                new DatabaseConnection();

            using (SqlConnection connection =
                   databaseConnection.CreateConnection())
            {
                connection.Open();

                string query = @"
                    UPDATE dbo.Orders
                    SET
                        OrderStatus = N'In Progress',
                        AcceptedAt = SYSDATETIME()
                    WHERE OrderId = @OrderId
                      AND FreelancerId = @FreelancerId
                      AND OrderStatus = N'Placed';";

                using (SqlCommand command =
                       new SqlCommand(query, connection))
                {
                    command.Parameters.Add(
                        "@OrderId",
                        SqlDbType.Int).Value = orderId;

                    command.Parameters.Add(
                        "@FreelancerId",
                        SqlDbType.Int).Value = freelancerId;

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }


        // ============================================================
        // DELIVER ORDER
        // In Progress -> Delivered
        // ============================================================

        public bool DeliverOrder(
            int orderId,
            int freelancerId,
            string deliveryNote)
        {
            DatabaseConnection databaseConnection =
                new DatabaseConnection();

            using (SqlConnection connection =
                   databaseConnection.CreateConnection())
            {
                connection.Open();

                string query = @"
                    UPDATE dbo.Orders
                    SET
                        OrderStatus = N'Delivered',
                        DeliveryNote = @DeliveryNote,
                        DeliveredAt = SYSDATETIME()
                    WHERE OrderId = @OrderId
                      AND FreelancerId = @FreelancerId
                      AND OrderStatus = N'In Progress';";

                using (SqlCommand command =
                       new SqlCommand(query, connection))
                {
                    command.Parameters.Add(
                        "@OrderId",
                        SqlDbType.Int).Value = orderId;

                    command.Parameters.Add(
                        "@FreelancerId",
                        SqlDbType.Int).Value = freelancerId;

                    command.Parameters.Add(
                        "@DeliveryNote",
                        SqlDbType.NVarChar,
                        1000).Value =
                        string.IsNullOrWhiteSpace(deliveryNote)
                            ? (object)DBNull.Value
                            : deliveryNote.Trim();

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }


        // ============================================================
        // GET SINGLE ORDER
        // ============================================================

        public Order GetById(
            int orderId,
            int freelancerId)
        {
            DatabaseConnection databaseConnection =
                new DatabaseConnection();

            using (SqlConnection connection =
                   databaseConnection.CreateConnection())
            {
                connection.Open();

                string query = @"
                    SELECT
                        o.OrderId,
                        o.ClientId,
                        client.FullName AS ClientName,
                        o.FreelancerId,
                        o.ServiceId,
                        s.Title AS ServiceTitle,
                        o.Quantity,
                        o.UnitPrice,
                        o.DiscountAmount,
                        o.GrossAmount,
                        o.CommissionRate,
                        o.CommissionAmount,
                        o.FreelancerEarning,
                        o.OrderStatus,
                        o.DeliveryNote,
                        o.CreatedAt,
                        o.AcceptedAt,
                        o.DeliveredAt,
                        o.CompletedAt
                    FROM dbo.Orders o
                    INNER JOIN dbo.Users client
                        ON client.UserId = o.ClientId
                    INNER JOIN dbo.Services s
                        ON s.ServiceId = o.ServiceId
                    WHERE o.OrderId = @OrderId
                      AND o.FreelancerId = @FreelancerId;";

                using (SqlCommand command =
                       new SqlCommand(query, connection))
                {
                    command.Parameters.Add(
                        "@OrderId",
                        SqlDbType.Int).Value = orderId;

                    command.Parameters.Add(
                        "@FreelancerId",
                        SqlDbType.Int).Value = freelancerId;

                    using (SqlDataReader reader =
                           command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Order
                            {
                                OrderId =
                                    Convert.ToInt32(
                                        reader["OrderId"]),

                                ClientId =
                                    Convert.ToInt32(
                                        reader["ClientId"]),

                                ClientName =
                                    reader["ClientName"].ToString(),

                                FreelancerId =
                                    Convert.ToInt32(
                                        reader["FreelancerId"]),

                                ServiceId =
                                    Convert.ToInt32(
                                        reader["ServiceId"]),

                                ServiceTitle =
                                    reader["ServiceTitle"].ToString(),

                                Quantity =
                                    Convert.ToInt32(
                                        reader["Quantity"]),

                                UnitPrice =
                                    Convert.ToDecimal(
                                        reader["UnitPrice"]),

                                DiscountAmount =
                                    Convert.ToDecimal(
                                        reader["DiscountAmount"]),

                                GrossAmount =
                                    Convert.ToDecimal(
                                        reader["GrossAmount"]),

                                CommissionRate =
                                    Convert.ToDecimal(
                                        reader["CommissionRate"]),

                                CommissionAmount =
                                    Convert.ToDecimal(
                                        reader["CommissionAmount"]),

                                FreelancerEarning =
                                    Convert.ToDecimal(
                                        reader["FreelancerEarning"]),

                                OrderStatus =
                                    reader["OrderStatus"].ToString(),

                                DeliveryNote =
                                    reader["DeliveryNote"] == DBNull.Value
                                        ? null
                                        : reader["DeliveryNote"].ToString(),

                                CreatedAt =
                                    Convert.ToDateTime(
                                        reader["CreatedAt"]),

                                AcceptedAt =
                                    reader["AcceptedAt"] == DBNull.Value
                                        ? (DateTime?)null
                                        : Convert.ToDateTime(
                                            reader["AcceptedAt"]),

                                DeliveredAt =
                                    reader["DeliveredAt"] == DBNull.Value
                                        ? (DateTime?)null
                                        : Convert.ToDateTime(
                                            reader["DeliveredAt"]),

                                CompletedAt =
                                    reader["CompletedAt"] == DBNull.Value
                                        ? (DateTime?)null
                                        : Convert.ToDateTime(
                                            reader["CompletedAt"])
                            };
                        }
                    }
                }
            }

            return null;
        }
    }
}