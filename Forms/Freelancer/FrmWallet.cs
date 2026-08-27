using SkillHub.Repositories;
using SkillHub.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SkillHub.Forms.Freelancer
{
    [System.ComponentModel.DesignerCategory("Code")]
    public partial class FrmWallet : Form
    {
        // ============================================================
        // REPOSITORY
        // ============================================================

        private readonly FreelancerWalletRepository _repository;

        private readonly int _freelancerId;

        private WalletBalance _walletBalance;

        // ============================================================
        // CONSTRUCTOR
        // ============================================================

        public FrmWallet(int freelancerId)
        {
            InitializeComponent();

            _freelancerId = freelancerId;

            _repository =
                new FreelancerWalletRepository();

            ConfigureForm();

            LoadWallet();
        }


        // ============================================================
        // FORM CONFIGURATION
        // ============================================================

        private void ConfigureForm()
        {
            Text = "My Wallet | SkillHub";

            StartPosition =
                FormStartPosition.CenterParent;

            MinimumSize =
                new Size(1000, 650);

            AutoScaleMode =
                AutoScaleMode.Font;

            dgvTransactions.AutoGenerateColumns = false;

            dgvTransactions.AllowUserToAddRows = false;

            dgvTransactions.AllowUserToDeleteRows = false;

            dgvTransactions.ReadOnly = true;

            dgvTransactions.MultiSelect = false;

            dgvTransactions.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvTransactions.RowHeadersVisible = false;

            dgvTransactions.AutoSizeRowsMode =
                DataGridViewAutoSizeRowsMode.None;
        }


        // ============================================================
        // LOAD WALLET
        // ============================================================

        private void LoadWallet()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                _walletBalance =
                    _repository.GetBalance(
                        _freelancerId);

                decimal totalEarned =
                    _repository.GetTotalEarned(
                        _freelancerId);

                List<WalletTransaction> transactions =
                    _repository.GetTransactions(
                        _freelancerId);

                UpdateSummary(
                    totalEarned);

                DisplayTransactions(
                    transactions);

                UpdateLastRefresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load wallet information.\n\n" +
                    ex.Message,
                    "Wallet Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }


        // ============================================================
        // SUMMARY CARDS
        // ============================================================

        private void UpdateSummary(
            decimal totalEarned)
        {
            lblAvailableBalance.Text =
                FormatMoney(
                    _walletBalance.AvailableBalance);

            lblLedgerBalance.Text =
                FormatMoney(
                    _walletBalance.LedgerBalance);

            lblPendingWithdrawal.Text =
                FormatMoney(
                    _walletBalance.PendingWithdrawalAmount);

            lblTotalEarned.Text =
                FormatMoney(
                    totalEarned);

            if (_walletBalance.AvailableBalance > 0)
            {
                lblBalanceStatus.Text =
                    "Available for withdrawal";
            }
            else if (_walletBalance.PendingWithdrawalAmount > 0)
            {
                lblBalanceStatus.Text =
                    "Funds are currently reserved";
            }
            else
            {
                lblBalanceStatus.Text =
                    "No withdrawable funds yet";
            }
        }


        // ============================================================
        // TRANSACTION TABLE
        // ============================================================

        private void DisplayTransactions(
            List<WalletTransaction> transactions)
        {
            dgvTransactions.Rows.Clear();

            foreach (WalletTransaction transaction
                     in transactions)
            {
                int rowIndex =
                    dgvTransactions.Rows.Add();

                DataGridViewRow row =
                    dgvTransactions.Rows[rowIndex];

                row.Cells["colDate"].Value =
                    transaction.TransactionDate
                        .ToString("dd MMM yyyy, hh:mm tt");

                row.Cells["colType"].Value =
                    transaction.TransactionType;

                row.Cells["colReference"].Value =
                    GetReference(transaction);

                row.Cells["colDescription"].Value =
                    string.IsNullOrWhiteSpace(
                        transaction.Description)
                        ? "-"
                        : transaction.Description;

                row.Cells["colAmount"].Value =
                    FormatTransactionAmount(
                        transaction);
            }

            lblTransactionCount.Text =
                transactions.Count +
                (transactions.Count == 1
                    ? " transaction"
                    : " transactions");

            lblEmptyTransactions.Visible =
                transactions.Count == 0;

            dgvTransactions.Visible =
                transactions.Count > 0;
        }


        // ============================================================
        // TRANSACTION REFERENCE
        // ============================================================

        private string GetReference(
            WalletTransaction transaction)
        {
            if (transaction.OrderId.HasValue)
            {
                return "Order #" +
                       transaction.OrderId.Value;
            }

            if (transaction.WithdrawalId.HasValue)
            {
                return "Withdrawal #" +
                       transaction.WithdrawalId.Value;
            }

            return "-";
        }


        // ============================================================
        // TRANSACTION AMOUNT
        // ============================================================

        private string FormatTransactionAmount(
            WalletTransaction transaction)
        {
            decimal amount =
                transaction.Amount;

            if (transaction.TransactionType ==
                    "Credit" ||
                transaction.TransactionType ==
                    "Adjustment")
            {
                return "+ " +
                       FormatMoney(amount);
            }

            return "- " +
                   FormatMoney(amount);
        }


        // ============================================================
        // MONEY FORMAT
        // ============================================================

        private string FormatMoney(
            decimal amount)
        {
            return "৳ " +
                   amount.ToString("N2");
        }


        // ============================================================
        // REFRESH
        // ============================================================

        private void btnRefresh_Click(
            object sender,
            EventArgs e)
        {
            LoadWallet();
        }


        // ============================================================
        // WITHDRAW
        // ============================================================

        private void btnWithdraw_Click(
            object sender,
            EventArgs e)
        {
            if (_walletBalance == null)
            {
                return;
            }

            if (_walletBalance.AvailableBalance <= 0)
            {
                MessageBox.Show(
                    "You do not have any available balance " +
                    "to withdraw.",
                    "No Available Balance",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            using (FrmWithdrawal withdrawal =
                   new FrmWithdrawal(
                       _freelancerId))
            {
                withdrawal.ShowDialog(this);
            }

            LoadWallet();
        }


        // ============================================================
        // CLOSE
        // ============================================================

        private void btnClose_Click(
            object sender,
            EventArgs e)
        {
            Close();
        }


        // ============================================================
        // LAST REFRESH
        // ============================================================

        private void UpdateLastRefresh()
        {
            lblLastRefresh.Text =
                "Last updated: " +
                DateTime.Now.ToString(
                    "dd MMM yyyy, hh:mm tt");
        }
    }
}