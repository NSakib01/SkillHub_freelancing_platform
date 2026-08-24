using System;
using System.Data;
using System.Data.SqlClient;
using SkillHub.Data;
using SkillHub.Models;

namespace SkillHub.Repositories
{
    public sealed class FreelancerProfileRepository
    {
        private readonly DatabaseConnection _database;

        public FreelancerProfileRepository()
        {
            _database = new DatabaseConnection();
        }

        public FreelancerProfile GetByUserId(int userId)
        {
            const string sql = @"
                SELECT
                    UserId,
                    ProfessionalTitle,
                    Biography,
                    Skills,
                    IsVerified,
                    AverageRating,
                    CreatedAt,
                    UpdatedAt
                FROM dbo.FreelancerProfiles
                WHERE UserId = @UserId;";

            using (SqlConnection connection = _database.OpenConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                DatabaseConnection.AddParameter(
                    command,
                    "@UserId",
                    SqlDbType.Int,
                    userId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return null;
                    }

                    return new FreelancerProfile
                    {
                        UserId = Convert.ToInt32(reader["UserId"]),
                        ProfessionalTitle = reader["ProfessionalTitle"] == DBNull.Value
                            ? string.Empty
                            : Convert.ToString(reader["ProfessionalTitle"]),

                        Biography = reader["Biography"] == DBNull.Value
                            ? string.Empty
                            : Convert.ToString(reader["Biography"]),

                        Skills = reader["Skills"] == DBNull.Value
                            ? string.Empty
                            : Convert.ToString(reader["Skills"]),

                        IsVerified = Convert.ToBoolean(reader["IsVerified"]),
                        AverageRating = Convert.ToDecimal(reader["AverageRating"]),
                        CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),

                        UpdatedAt = reader["UpdatedAt"] == DBNull.Value
                            ? (DateTime?)null
                            : Convert.ToDateTime(reader["UpdatedAt"])
                    };
                }
            }
        }

        public void Update(FreelancerProfile profile)
        {
            const string sql = @"
                UPDATE dbo.FreelancerProfiles
                SET
                    ProfessionalTitle = @ProfessionalTitle,
                    Biography = @Biography,
                    Skills = @Skills,
                    UpdatedAt = SYSDATETIME()
                WHERE UserId = @UserId;";

            using (SqlConnection connection = _database.OpenConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                DatabaseConnection.AddParameter(
                    command,
                    "@UserId",
                    SqlDbType.Int,
                    profile.UserId);

                DatabaseConnection.AddParameter(
                    command,
                    "@ProfessionalTitle",
                    SqlDbType.NVarChar,
                    string.IsNullOrWhiteSpace(profile.ProfessionalTitle)
                        ? (object)DBNull.Value
                        : profile.ProfessionalTitle,
                    120);

                DatabaseConnection.AddParameter(
                    command,
                    "@Biography",
                    SqlDbType.NVarChar,
                    string.IsNullOrWhiteSpace(profile.Biography)
                        ? (object)DBNull.Value
                        : profile.Biography,
                    1000);

                DatabaseConnection.AddParameter(
                    command,
                    "@Skills",
                    SqlDbType.NVarChar,
                    string.IsNullOrWhiteSpace(profile.Skills)
                        ? (object)DBNull.Value
                        : profile.Skills,
                    500);

                command.ExecuteNonQuery();
            }
        }
    }
}