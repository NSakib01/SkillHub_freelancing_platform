using SkillHub.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SkillHub.Repositories
{
    public class FreelancerWalletRepository
    {
        // ============================================================
        // WALLET BALANCE
        // ============================================================

        public WalletBalance GetBalance(int freelancerId)
        {
            DatabaseConnection databaseConnection =
                new DatabaseConnection();

            using (SqlConnection connection =
                   databaseConnection.CreateConnection())
            {
                connection.Open();

                string query = @"
                    SELECT
                        FreelancerId,
                        FullName,
                        LedgerBalance,
                        PendingWithdrawalAmount,
                        AvailableBalance
                    FROM dbo.vw_FreelancerWalletBalances
                    WHERE FreelancerId = @FreelancerId;";

                using (SqlCommand command =
                       new SqlCommand(query, connection))
                {
                    command.Parameters.Add(
                        "@FreelancerId",
                        SqlDbType.Int).Value = freelancerId;

                    using (SqlDataReader reader =
                           command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new WalletBalance
                            {
                                FreelancerId =
                                    Convert.ToInt32(
                                        reader["FreelancerId"]),

                                FullName =
                                    reader["FullName"].ToString(),

                                LedgerBalance =
                                    Convert.ToDecimal(
                                        reader["LedgerBalance"]),

                                PendingWithdrawalAmount =
                                    Convert.ToDecimal(
                                        reader["PendingWithdrawalAmount"]),

                                AvailableBalance =
                                    Convert.ToDecimal(
                                        reader["AvailableBalance"])
                            };
                        }
                    }
                }
            }

            return new WalletBalance
            {
                FreelancerId = freelancerId,
                LedgerBalance = 0m,
                PendingWithdrawalAmount = 0m,
                AvailableBalance = 0m
            };
        }


        // ============================================================
        // TRANSACTION HISTORY
        // ============================================================

        public List<WalletTransaction> GetTransactions(
            int freelancerId)
        {
            List<WalletTransaction> transactions =
                new List<WalletTransaction>();

            DatabaseConnection databaseConnection =
                new DatabaseConnection();

            using (SqlConnection connection =
                   databaseConnection.CreateConnection())
            {
                connection.Open();

                string query = @"
                    SELECT
                        wt.WalletTxnId,
                        wt.FreelancerId,
                        wt.OrderId,
                        wt.WithdrawalId,
                        wt.TransactionType,
                        wt.Amount,
                        wt.Description,
                        wt.TransactionDate
                    FROM dbo.WalletTransactions wt
                    WHERE wt.FreelancerId = @FreelancerId
                    ORDER BY wt.TransactionDate DESC,
                             wt.WalletTxnId DESC;";

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
                            transactions.Add(
                                new WalletTransaction
                                {
                                    WalletTxnId =
                                        Convert.ToInt32(
                                            reader["WalletTxnId"]),

                                    FreelancerId =
                                        Convert.ToInt32(
                                            reader["FreelancerId"]),

                                    OrderId =
                                        reader["OrderId"] == DBNull.Value
                                            ? (int?)null
                                            : Convert.ToInt32(
                                                reader["OrderId"]),

                                    WithdrawalId =
                                        reader["WithdrawalId"] == DBNull.Value
                                            ? (int?)null
                                            : Convert.ToInt32(
                                                reader["WithdrawalId"]),

                                    TransactionType =
                                        reader["TransactionType"]
                                            .ToString(),

                                    Amount =
                                        Convert.ToDecimal(
                                            reader["Amount"]),

                                    Description =
                                        reader["Description"] == DBNull.Value
                                            ? string.Empty
                                            : reader["Description"]
                                                .ToString(),

                                    TransactionDate =
                                        Convert.ToDateTime(
                                            reader["TransactionDate"])
                                });
                        }
                    }
                }
            }

            return transactions;
        }


        // ============================================================
        // TOTAL EARNINGS
        // Completed-order credits only
        // ============================================================

        public decimal GetTotalEarned(int freelancerId)
        {
            DatabaseConnection databaseConnection =
                new DatabaseConnection();

            using (SqlConnection connection =
                   databaseConnection.CreateConnection())
            {
                connection.Open();

                string query = @"
                    SELECT
                        COALESCE(SUM(Amount), 0.00)
                    FROM dbo.WalletTransactions
                    WHERE FreelancerId = @FreelancerId
                      AND TransactionType = N'Credit';";

                using (SqlCommand command =
                       new SqlCommand(query, connection))
                {
                    command.Parameters.Add(
                        "@FreelancerId",
                        SqlDbType.Int).Value = freelancerId;

                    return Convert.ToDecimal(
                        command.ExecuteScalar());
                }
            }
        }
    }


    // ================================================================
    // WALLET BALANCE MODEL
    // ================================================================

    public class WalletBalance
    {
        public int FreelancerId { get; set; }

        public string FullName { get; set; }

        public decimal LedgerBalance { get; set; }

        public decimal PendingWithdrawalAmount { get; set; }

        public decimal AvailableBalance { get; set; }
    }


    // ================================================================
    // WALLET TRANSACTION MODEL
    // ================================================================

    public class WalletTransaction
    {
        public int WalletTxnId { get; set; }

        public int FreelancerId { get; set; }

        public int? OrderId { get; set; }

        public int? WithdrawalId { get; set; }

        public string TransactionType { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }

        public DateTime TransactionDate { get; set; }
    }
}