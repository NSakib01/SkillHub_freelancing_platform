using System;
using System.Data;
using System.Data.SqlClient;
using SkillHub.Data;
using SkillHub.Models;

namespace SkillHub.Repositories
{
    /// <summary>
    /// Parameterized account CRUD used by registration, login, profile editing
    /// and Sakib's individual DataGridView demonstration.
    /// </summary>
    public sealed class UserRepository
    {
        private const string AccountProjection =
            "SELECT u.UserId, u.RoleId, r.RoleName, u.FullName, u.Email, "
            + "u.PasswordHash, u.Phone, u.Address, u.ProfileImagePath, u.Status, u.CreatedAt "
            + "FROM dbo.Users AS u "
            + "INNER JOIN dbo.Roles AS r ON r.RoleId = u.RoleId ";

        private readonly DatabaseConnection _database;

        public UserRepository()
            : this(new DatabaseConnection())
        {
        }

        public UserRepository(DatabaseConnection database)
        {
            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
            }

            _database = database;
        }

        public User GetByEmail(string email)
        {
            using (SqlConnection connection = _database.OpenConnection())
            using (SqlCommand command = new SqlCommand(
                AccountProjection + "WHERE u.Email = @Email;",
                connection))
            {
                DatabaseConnection.AddParameter(
                    command, "@Email", SqlDbType.NVarChar, email, 150);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    return reader.Read() ? MapUser(reader) : null;
                }
            }
        }

        public User GetById(int userId)
        {
            using (SqlConnection connection = _database.OpenConnection())
            using (SqlCommand command = new SqlCommand(
                AccountProjection + "WHERE u.UserId = @UserId;",
                connection))
            {
                DatabaseConnection.AddParameter(
                    command, "@UserId", SqlDbType.Int, userId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    return reader.Read() ? MapUser(reader) : null;
                }
            }
        }

        public int Create(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            using (SqlConnection connection = _database.OpenConnection())
            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    int roleId = ResolveRoleId(connection, transaction, user.RoleName);
                    int userId;

                    using (SqlCommand command = new SqlCommand(
                        "INSERT INTO dbo.Users "
                        + "(RoleId, FullName, Email, PasswordHash, Phone, Address, Status) "
                        + "VALUES "
                        + "(@RoleId, @FullName, @Email, @PasswordHash, @Phone, @Address, @Status); "
                        + "SELECT CAST(SCOPE_IDENTITY() AS INT);",
                        connection,
                        transaction))
                    {
                        DatabaseConnection.AddParameter(
                            command, "@RoleId", SqlDbType.Int, roleId);
                        DatabaseConnection.AddParameter(
                            command, "@FullName", SqlDbType.NVarChar, user.FullName, 120);
                        DatabaseConnection.AddParameter(
                            command, "@Email", SqlDbType.NVarChar, user.Email, 150);
                        DatabaseConnection.AddParameter(
                            command, "@PasswordHash", SqlDbType.NVarChar, user.PasswordHash, 300);
                        DatabaseConnection.AddParameter(
                            command, "@Phone", SqlDbType.NVarChar, user.Phone, 20);
                        DatabaseConnection.AddParameter(
                            command, "@Address", SqlDbType.NVarChar, user.Address, 250);
                        DatabaseConnection.AddParameter(
                            command, "@Status", SqlDbType.NVarChar, AccountStatuses.Active, 20);

                        userId = Convert.ToInt32(command.ExecuteScalar());
                    }

                    CreateRoleSpecificRecords(connection, transaction, user.RoleName, userId);
                    transaction.Commit();

                    user.UserId = userId;
                    user.RoleId = roleId;
                    user.Status = AccountStatuses.Active;

                    return userId;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public DataTable Search(string searchTerm)
        {
            string normalizedSearch = (searchTerm ?? string.Empty).Trim();

            using (SqlConnection connection = _database.OpenConnection())
            using (SqlCommand command = new SqlCommand(
                "SELECT UserId, RoleName, UserType, FullName, Email, Phone, Address, "
                + "Status, CreatedAt "
                + "FROM dbo.vw_UserAccounts "
                + "WHERE @Search = N'' "
                + "OR FullName LIKE @Pattern "
                + "OR Email LIKE @Pattern "
                + "OR RoleName LIKE @Pattern "
                + "OR Status LIKE @Pattern "
                + "ORDER BY UserId DESC;",
                connection))
            {
                DatabaseConnection.AddParameter(
                    command, "@Search", SqlDbType.NVarChar, normalizedSearch, 150);
                DatabaseConnection.AddParameter(
                    command,
                    "@Pattern",
                    SqlDbType.NVarChar,
                    "%" + normalizedSearch + "%",
                    152);

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataTable accounts = new DataTable("UserAccounts");
                    adapter.Fill(accounts);
                    return accounts;
                }
            }
        }

        public void UpdateProfile(
            int userId,
            string fullName,
            string email,
            string phone,
            string address)
        {
            using (SqlConnection connection = _database.OpenConnection())
            using (SqlCommand command = new SqlCommand(
                "UPDATE dbo.Users "
                + "SET FullName = @FullName, Email = @Email, Phone = @Phone, "
                + "Address = @Address, UpdatedAt = SYSDATETIME() "
                + "WHERE UserId = @UserId AND Status <> @Deactivated;",
                connection))
            {
                DatabaseConnection.AddParameter(
                    command, "@UserId", SqlDbType.Int, userId);
                DatabaseConnection.AddParameter(
                    command, "@FullName", SqlDbType.NVarChar, fullName, 120);
                DatabaseConnection.AddParameter(
                    command, "@Email", SqlDbType.NVarChar, email, 150);
                DatabaseConnection.AddParameter(
                    command, "@Phone", SqlDbType.NVarChar, phone, 20);
                DatabaseConnection.AddParameter(
                    command, "@Address", SqlDbType.NVarChar, address, 250);
                DatabaseConnection.AddParameter(
                    command, "@Deactivated", SqlDbType.NVarChar, AccountStatuses.Deactivated, 20);

                if (command.ExecuteNonQuery() != 1)
                {
                    throw new InvalidOperationException(
                        "The selected active account could not be updated.");
                }
            }
        }

        public void UpdatePasswordHash(int userId, string passwordHash)
        {
            using (SqlConnection connection = _database.OpenConnection())
            using (SqlCommand command = new SqlCommand(
                "UPDATE dbo.Users "
                + "SET PasswordHash = @PasswordHash, UpdatedAt = SYSDATETIME() "
                + "WHERE UserId = @UserId AND Status = @Active;",
                connection))
            {
                DatabaseConnection.AddParameter(
                    command, "@PasswordHash", SqlDbType.NVarChar, passwordHash, 300);
                DatabaseConnection.AddParameter(
                    command, "@UserId", SqlDbType.Int, userId);
                DatabaseConnection.AddParameter(
                    command, "@Active", SqlDbType.NVarChar, AccountStatuses.Active, 20);

                if (command.ExecuteNonQuery() != 1)
                {
                    throw new InvalidOperationException("The account password could not be changed.");
                }
            }
        }

        public void Deactivate(int userId)
        {
            using (SqlConnection connection = _database.OpenConnection())
            using (SqlCommand command = new SqlCommand(
                "UPDATE dbo.Users "
                + "SET Status = @Deactivated, UpdatedAt = SYSDATETIME() "
                + "WHERE UserId = @UserId AND Status <> @Deactivated;",
                connection))
            {
                DatabaseConnection.AddParameter(
                    command, "@Deactivated", SqlDbType.NVarChar, AccountStatuses.Deactivated, 20);
                DatabaseConnection.AddParameter(
                    command, "@UserId", SqlDbType.Int, userId);

                if (command.ExecuteNonQuery() != 1)
                {
                    throw new InvalidOperationException(
                        "The account does not exist or has already been deactivated.");
                }
            }
        }

        public void RecordSuccessfulLogin(int userId)
        {
            using (SqlConnection connection = _database.OpenConnection())
            using (SqlCommand command = new SqlCommand(
                "UPDATE dbo.Users "
                + "SET LastLoginAt = SYSDATETIME() "
                + "WHERE UserId = @UserId;",
                connection))
            {
                DatabaseConnection.AddParameter(
                    command, "@UserId", SqlDbType.Int, userId);

                command.ExecuteNonQuery();
            }
        }

        private static int ResolveRoleId(
            SqlConnection connection,
            SqlTransaction transaction,
            string roleName)
        {
            using (SqlCommand command = new SqlCommand(
                "SELECT RoleId FROM dbo.Roles WHERE RoleName = @RoleName;",
                connection,
                transaction))
            {
                DatabaseConnection.AddParameter(
                    command, "@RoleName", SqlDbType.NVarChar, roleName, 30);

                object result = command.ExecuteScalar();

                if (result == null || result == DBNull.Value)
                {
                    throw new InvalidOperationException(
                        "The requested account role was not found in the database.");
                }

                return Convert.ToInt32(result);
            }
        }

        private static void CreateRoleSpecificRecords(
            SqlConnection connection,
            SqlTransaction transaction,
            string roleName,
            int userId)
        {
            if (string.Equals(roleName, UserRoles.Client, StringComparison.Ordinal))
            {
                ExecuteUserIdInsert(
                    connection,
                    transaction,
                    "INSERT INTO dbo.ClientProfiles (UserId) VALUES (@UserId);",
                    userId);

                ExecuteUserIdInsert(
                    connection,
                    transaction,
                    "INSERT INTO dbo.Carts (ClientId) VALUES (@UserId);",
                    userId);
            }
            else if (string.Equals(roleName, UserRoles.Freelancer, StringComparison.Ordinal))
            {
                ExecuteUserIdInsert(
                    connection,
                    transaction,
                    "INSERT INTO dbo.FreelancerProfiles (UserId) VALUES (@UserId);",
                    userId);
            }
        }

        private static void ExecuteUserIdInsert(
            SqlConnection connection,
            SqlTransaction transaction,
            string statement,
            int userId)
        {
            using (SqlCommand command = new SqlCommand(statement, connection, transaction))
            {
                DatabaseConnection.AddParameter(
                    command, "@UserId", SqlDbType.Int, userId);

                command.ExecuteNonQuery();
            }
        }

        private static User MapUser(SqlDataReader reader)
        {
            string roleName = Convert.ToString(reader["RoleName"]);
            User user;

            if (string.Equals(roleName, UserRoles.Admin, StringComparison.OrdinalIgnoreCase))
            {
                user = new Admin();
            }
            else if (string.Equals(roleName, UserRoles.Freelancer, StringComparison.OrdinalIgnoreCase))
            {
                user = new Freelancer();
            }
            else if (string.Equals(roleName, UserRoles.Client, StringComparison.OrdinalIgnoreCase))
            {
                user = new Client();
            }
            else
            {
                throw new InvalidOperationException("An unsupported account role was found.");
            }

            user.UserId = Convert.ToInt32(reader["UserId"]);
            user.RoleId = Convert.ToInt32(reader["RoleId"]);
            user.FullName = Convert.ToString(reader["FullName"]);
            user.Email = Convert.ToString(reader["Email"]);
            user.PasswordHash = Convert.ToString(reader["PasswordHash"]);
            user.Phone = reader["Phone"] == DBNull.Value
                ? null
                : Convert.ToString(reader["Phone"]);
            user.Address = reader["Address"] == DBNull.Value
                ? null
                : Convert.ToString(reader["Address"]);
            user.ProfileImagePath = reader["ProfileImagePath"] == DBNull.Value
                ? null
                : Convert.ToString(reader["ProfileImagePath"]);
            user.Status = Convert.ToString(reader["Status"]);
            user.CreatedAt = Convert.ToDateTime(reader["CreatedAt"]);

            return user;
        }
    }
}
