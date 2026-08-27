using SkillHub.Data;
using SkillHub.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SkillHub.Repositories
{
    public class WithdrawalRepository
    {
        // ============================================================
        // GET WITHDRAWAL HISTORY
        // ============================================================

        public List<Withdrawal> GetWithdrawals(int freelancerId)
        {
            List<Withdrawal> withdrawals =
                new List<Withdrawal>();

            DatabaseConnection databaseConnection =
                new DatabaseConnection();

            using (SqlConnection connection =
                   databaseConnection.CreateConnection())
            {
                connection.Open();

                string query = @"
                    SELECT
                        WithdrawalId,
                        FreelancerId,
                        Amount,
                        Status,
                        RequestDate,
                        ProcessedBy,
                        ProcessedAt,
                        AdminNote
                    FROM dbo.WithdrawalRequests
                    WHERE FreelancerId = @FreelancerId
                    ORDER BY RequestDate DESC,
                             WithdrawalId DESC;";

                using (SqlCommand command =
                       new SqlCommand(query, connection))
                {
                    DatabaseConnection.AddParameter(
                        command,
                        "@FreelancerId",
                        SqlDbType.Int,
                        freelancerId);

                    using (SqlDataReader reader =
                           command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            withdrawals.Add(
                                new Withdrawal
                                {
                                    WithdrawalId =
                                        Convert.ToInt32(
                                            reader["WithdrawalId"]),

                                    FreelancerId =
                                        Convert.ToInt32(
                                            reader["FreelancerId"]),

                                    Amount =
                                        Convert.ToDecimal(
                                            reader["Amount"]),

                                    Status =
                                        reader["Status"].ToString(),

                                    RequestDate =
                                        Convert.ToDateTime(
                                            reader["RequestDate"]),

                                    ProcessedBy =
                                        reader["ProcessedBy"] == DBNull.Value
                                            ? (int?)null
                                            : Convert.ToInt32(
                                                reader["ProcessedBy"]),

                                    ProcessedAt =
                                        reader["ProcessedAt"] == DBNull.Value
                                            ? (DateTime?)null
                                            : Convert.ToDateTime(
                                                reader["ProcessedAt"]),

                                    AdminNote =
                                        reader["AdminNote"] == DBNull.Value
                                            ? string.Empty
                                            : reader["AdminNote"].ToString()
                                });
                        }
                    }
                }
            }

            return withdrawals;
        }


        // ============================================================
        // CREATE WITHDRAWAL REQUEST
        // ============================================================

        public int CreateWithdrawal(
            int freelancerId,
            decimal amount)
        {
            if (amount <= 0m)
            {
                throw new ArgumentException(
                    "Withdrawal amount must be greater than zero.");
            }

            DatabaseConnection databaseConnection =
                new DatabaseConnection();

            using (SqlConnection connection =
                   databaseConnection.CreateConnection())
            {
                connection.Open();

                using (SqlTransaction transaction =
                       connection.BeginTransaction())
                {
                    try
                    {
                        // ------------------------------------------------
                        // Check available balance.
                        // ------------------------------------------------

                        string balanceQuery = @"
                            SELECT
                                AvailableBalance
                            FROM dbo.vw_FreelancerWalletBalances
                            WHERE FreelancerId = @FreelancerId;";

                        decimal availableBalance = 0m;

                        using (SqlCommand balanceCommand =
                               new SqlCommand(
                                   balanceQuery,
                                   connection,
                                   transaction))
                        {
                            DatabaseConnection.AddParameter(
                                balanceCommand,
                                "@FreelancerId",
                                SqlDbType.Int,
                                freelancerId);

                            object result =
                                balanceCommand.ExecuteScalar();

                            if (result != null &&
                                result != DBNull.Value)
                            {
                                availableBalance =
                                    Convert.ToDecimal(result);
                            }
                        }

                        if (amount > availableBalance)
                        {
                            throw new InvalidOperationException(
                                "The withdrawal amount exceeds your available balance.");
                        }


                        // ------------------------------------------------
                        // Insert withdrawal request.
                        // Status = Pending by database default.
                        // ------------------------------------------------

                        string insertQuery = @"
                            INSERT INTO dbo.WithdrawalRequests
                            (
                                FreelancerId,
                                Amount
                            )
                            OUTPUT INSERTED.WithdrawalId
                            VALUES
                            (
                                @FreelancerId,
                                @Amount
                            );";

                        int withdrawalId;

                        using (SqlCommand insertCommand =
                               new SqlCommand(
                                   insertQuery,
                                   connection,
                                   transaction))
                        {
                            DatabaseConnection.AddParameter(
                                insertCommand,
                                "@FreelancerId",
                                SqlDbType.Int,
                                freelancerId);

                            SqlParameter amountParameter =
                                insertCommand.Parameters.Add(
                                    "@Amount",
                                    SqlDbType.Decimal);

                            amountParameter.Precision = 18;
                            amountParameter.Scale = 2;
                            amountParameter.Value = amount;

                            withdrawalId =
                                Convert.ToInt32(
                                    insertCommand.ExecuteScalar());
                        }

                        transaction.Commit();

                        return withdrawalId;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}