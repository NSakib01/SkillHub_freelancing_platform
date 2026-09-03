using System;
using System.Data;
using System.Data.SqlClient;
using SkillHub.Data;
using SkillHub.Models;
using SkillHub.Utilities;

namespace SkillHub.Repositories
{
    public sealed class DisputeRepository
    {
        private readonly DatabaseConnection _database;

        public DisputeRepository()
        {
            _database = new DatabaseConnection();
        }

        public void AddDispute(DisputeModel dispute)
        {
            if (dispute == null)
            {
                throw new ArgumentNullException(nameof(dispute));
            }

            if (dispute.OrderId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(dispute.OrderId));
            }

            if (string.IsNullOrWhiteSpace(dispute.Reason))
            {
                throw new ArgumentException(
                    "A dispute reason is required.",
                    nameof(dispute.Reason));
            }

            if (dispute.Reason.Length > 2000)
            {
                throw new ArgumentException(
                    "Dispute reason cannot exceed 2000 characters.",
                    nameof(dispute.Reason));
            }

            int clientId = UserSession.UserId;

            const string sql = @"
                INSERT INTO dbo.Disputes
                (
                    OrderId,
                    OpenedBy,
                    Reason,
                    Status
                )
                SELECT
                    o.OrderId,
                    @OpenedBy,
                    @Reason,
                    N'Open'
                FROM dbo.Orders AS o
                WHERE o.OrderId = @OrderId
                  AND o.ClientId = @ClientId
                  AND o.OrderStatus IN
                  (
                      N'Placed',
                      N'In Progress',
                      N'Delivered'
                  )
                  AND NOT EXISTS
                  (
                      SELECT 1
                      FROM dbo.Disputes AS existingDispute
                      WHERE existingDispute.OrderId = o.OrderId
                        AND existingDispute.OpenedBy = @OpenedBy
                        AND existingDispute.Status IN
                        (
                            N'Open',
                            N'Under Review'
                        )
                  );

                IF @@ROWCOUNT = 0
                BEGIN
                    THROW 51008, 'A dispute cannot be filed for this order.', 1;
                END;";

            using (SqlConnection connection = _database.OpenConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                DatabaseConnection.AddParameter(
                    command,
                    "@OrderId",
                    SqlDbType.Int,
                    dispute.OrderId);

                DatabaseConnection.AddParameter(
                    command,
                    "@ClientId",
                    SqlDbType.Int,
                    clientId);

                DatabaseConnection.AddParameter(
                    command,
                    "@OpenedBy",
                    SqlDbType.Int,
                    clientId);

                DatabaseConnection.AddParameter(
                    command,
                    "@Reason",
                    SqlDbType.NVarChar,
                    dispute.Reason.Trim(),
                    2000);

                command.ExecuteNonQuery();
            }
        }

        public bool HasOpenDispute(int orderId)
        {
            if (orderId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(orderId));
            }

            int clientId = UserSession.UserId;

            const string sql = @"
                SELECT COUNT(*)
                FROM dbo.Disputes AS d
                INNER JOIN dbo.Orders AS o
                    ON o.OrderId = d.OrderId
                WHERE d.OrderId = @OrderId
                  AND d.OpenedBy = @OpenedBy
                  AND o.ClientId = @ClientId
                  AND d.Status IN
                  (
                      N'Open',
                      N'Under Review'
                  );";

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
                    "@OpenedBy",
                    SqlDbType.Int,
                    clientId);

                DatabaseConnection.AddParameter(
                    command,
                    "@ClientId",
                    SqlDbType.Int,
                    clientId);

                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }
    }
}
