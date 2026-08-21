using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SkillHub.Data
{
    /// <summary>
    /// The single, shared SQL Server entry point for every SkillHub module.
    /// Every caller receives a new connection and owns its disposal.
    /// </summary>
    public sealed class DatabaseConnection
    {
        public const string ConnectionStringName = "SkillHubConnection";

        private readonly string _connectionString;

        public DatabaseConnection()
            : this(ReadConfiguredConnectionString())
        {
        }

        public DatabaseConnection(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ConfigurationErrorsException(
                    "The SkillHub database connection string is missing.");
            }

            _connectionString = connectionString;
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public SqlConnection OpenConnection()
        {
            SqlConnection connection = CreateConnection();

            try
            {
                connection.Open();
                return connection;
            }
            catch
            {
                connection.Dispose();
                throw;
            }
        }

        public bool TryTestConnection(out string message)
        {
            try
            {
                using (SqlConnection connection = OpenConnection())
                using (SqlCommand command = new SqlCommand("SELECT DB_NAME();", connection))
                {
                    object databaseName = command.ExecuteScalar();
                    message = "Connected successfully to " + Convert.ToString(databaseName) + ".";
                    return true;
                }
            }
            catch (ConfigurationErrorsException)
            {
                message = "The database configuration is missing. Check App.config.";
                return false;
            }
            catch (SqlException)
            {
                message = "Cannot connect to SkillHubDB. Run Database/SkillHubDatabase.sql and verify App.config.";
                return false;
            }
            catch (InvalidOperationException)
            {
                message = "The database connection configuration is invalid.";
                return false;
            }
        }

        public static void AddParameter(
            SqlCommand command,
            string name,
            SqlDbType databaseType,
            object value,
            int size = 0)
        {
            SqlParameter parameter = size > 0
                ? command.Parameters.Add(name, databaseType, size)
                : command.Parameters.Add(name, databaseType);

            parameter.Value = value ?? DBNull.Value;
        }

        private static string ReadConfiguredConnectionString()
        {
            ConnectionStringSettings settings =
                ConfigurationManager.ConnectionStrings[ConnectionStringName];

            if (settings == null || string.IsNullOrWhiteSpace(settings.ConnectionString))
            {
                throw new ConfigurationErrorsException(
                    "App.config must contain a connection string named SkillHubConnection.");
            }

            return settings.ConnectionString;
        }
    }
}
