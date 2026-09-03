using System;
using System.Data;
using System.Data.SqlClient;
using SkillHub.Data;
using SkillHub.Models;
using SkillHub.Utilities;

namespace SkillHub.Repositories
{
    public sealed class ReviewRepository
    {
        private readonly DatabaseConnection _database;

        public ReviewRepository()
        {
            _database = new DatabaseConnection();
        }

        public bool HasReview(int orderId)
        {
            if (orderId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(orderId));
            }

            int clientId = UserSession.UserId;

            const string sql = @"
                SELECT COUNT(*)
                FROM dbo.Reviews
                WHERE OrderId = @OrderId
                  AND ClientId = @ClientId;";

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

                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        public void AddReview(ReviewModel review)
        {
            if (review == null)
            {
                throw new ArgumentNullException(nameof(review));
            }

            if (review.OrderId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(review.OrderId));
            }

            if (review.Rating < 1 || review.Rating > 5)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(review.Rating),
                    "Rating must be between 1 and 5.");
            }

            if (review.Comment == null)
            {
                review.Comment = string.Empty;
            }

            if (review.Comment.Length > 2000)
            {
                throw new ArgumentException(
                    "Review comment cannot exceed 2000 characters.",
                    nameof(review.Comment));
            }

            int clientId = UserSession.UserId;

            const string sql = @"
                INSERT INTO dbo.Reviews
                (
                    OrderId,
                    ClientId,
                    FreelancerId,
                    Rating,
                    Comment
                )
                SELECT
                    o.OrderId,
                    o.ClientId,
                    o.FreelancerId,
                    @Rating,
                    @Comment
                FROM dbo.Orders AS o
                WHERE o.OrderId = @OrderId
                  AND o.ClientId = @ClientId
                  AND o.OrderStatus = N'Completed'
                  AND NOT EXISTS
                  (
                      SELECT 1
                      FROM dbo.Reviews AS existingReview
                      WHERE existingReview.OrderId = o.OrderId
                  );

                IF @@ROWCOUNT = 0
                BEGIN
                    THROW 51007, 'Review cannot be added. The order may not be completed or may already have a review.', 1;
                END;";

            using (SqlConnection connection = _database.OpenConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                DatabaseConnection.AddParameter(
                    command,
                    "@OrderId",
                    SqlDbType.Int,
                    review.OrderId);

                DatabaseConnection.AddParameter(
                    command,
                    "@ClientId",
                    SqlDbType.Int,
                    clientId);

                DatabaseConnection.AddParameter(
                    command,
                    "@Rating",
                    SqlDbType.TinyInt,
                    review.Rating);

                DatabaseConnection.AddParameter(
                    command,
                    "@Comment",
                    SqlDbType.NVarChar,
                    review.Comment,
                    2000);

                command.ExecuteNonQuery();
            }
        }
    }
}
