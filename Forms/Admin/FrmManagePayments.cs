using SkillHub.Data;
using SkillHub.Forms.Common;
using SkillHub.Services;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace SkillHub.Forms.Admin
{
    /// <summary>
    /// Super-admin simulated payment management.
    /// View payments and update simulated payment status.
    /// </summary>
    public sealed class FrmManagePayments : Form
    {
        private readonly DatabaseConnection _database;

        private DataGridView _paymentsGrid;

        private TextBox _searchInput;
        private ComboBox _statusInput;

        private TextBox _paymentIdInput;
        private TextBox _orderIdInput;
        private TextBox _amountInput;
        private TextBox _methodInput;
        private TextBox _referenceInput;
        private ComboBox _editStatusInput;

        private int? _selectedPaymentId;

        public FrmManagePayments()
        {
            AuthorizationService.DemandAdmin();

            _database = new DatabaseConnection();

            Text = "SkillHub | Manage Payments";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(1300, 780);
            MinimumSize = new Size(1100, 680);

            BuildLayout();
            LoadPayments();
            UiFactory.AddBackToDashboardButton(this);
        }

        private void BuildLayout()
        {
            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };

            Label heading = new Label
            {
                AutoSize = true,
                Text = "Payment Management",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Location = new Point(20, 20)
            };

            Label caption = new Label
            {
                AutoSize = true,
                Text = "View and control simulated marketplace payment records.",
                ForeColor = Color.Gray,
                Location = new Point(23, 58)
            };

            _searchInput = new TextBox
            {
                Width = 280,
                Location = new Point(20, 100)
            };

            _searchInput.TextChanged += delegate
            {
                LoadPayments();
            };

            _statusInput = new ComboBox
            {
                Width = 170,
                Location = new Point(315, 100),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            _statusInput.Items.Add("All Status");
            _statusInput.Items.Add("Pending");
            _statusInput.Items.Add("Paid");
            _statusInput.Items.Add("Failed");
            _statusInput.Items.Add("Refunded");
            _statusInput.SelectedIndex = 0;

            _statusInput.SelectedIndexChanged += delegate
            {
                LoadPayments();
            };

            Button refreshButton = new Button
            {
                Text = "Refresh",
                Width = 100,
                Height = 32,
                Location = new Point(500, 98)
            };

            refreshButton.Click += delegate
            {
                LoadPayments();
            };

            Button clearFilterButton = new Button
            {
                Text = "Clear Filters",
                Width = 120,
                Height = 32,
                Location = new Point(610, 98)
            };

            clearFilterButton.Click += delegate
            {
                _searchInput.Clear();
                _statusInput.SelectedIndex = 0;
            };

            _paymentsGrid = new DataGridView
            {
                Location = new Point(20, 145),
                Anchor = AnchorStyles.Top
                    | AnchorStyles.Bottom
                    | AnchorStyles.Left
                    | AnchorStyles.Right,
                Width = 1250,
                Height = 390,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode =
                    DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };

            _paymentsGrid.SelectionChanged += delegate
            {
                LoadSelectedPayment();
            };

            GroupBox editor = new GroupBox
            {
                Text = "Selected Payment",
                Location = new Point(20, 550),
                Anchor = AnchorStyles.Left
                    | AnchorStyles.Right
                    | AnchorStyles.Bottom,
                Width = 1250,
                Height = 140
            };

            Label paymentIdLabel = CreateLabel("Payment ID", 15, 25);
            _paymentIdInput = CreateTextBox(15, 47, 120);
            _paymentIdInput.ReadOnly = true;

            Label orderIdLabel = CreateLabel("Order ID", 150, 25);
            _orderIdInput = CreateTextBox(150, 47, 120);
            _orderIdInput.ReadOnly = true;

            Label amountLabel = CreateLabel("Amount", 285, 25);
            _amountInput = CreateTextBox(285, 47, 140);
            _amountInput.ReadOnly = true;

            Label methodLabel = CreateLabel("Payment Method", 440, 25);
            _methodInput = CreateTextBox(440, 47, 180);
            _methodInput.ReadOnly = true;

            Label referenceLabel = CreateLabel("Transaction Reference", 635, 25);
            _referenceInput = CreateTextBox(635, 47, 220);
            _referenceInput.ReadOnly = true;

            Label statusLabel = CreateLabel("New Status", 870, 25);

            _editStatusInput = new ComboBox
            {
                Width = 150,
                Location = new Point(870, 47),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            _editStatusInput.Items.Add("Pending");
            _editStatusInput.Items.Add("Paid");
            _editStatusInput.Items.Add("Failed");
            _editStatusInput.Items.Add("Refunded");
            _editStatusInput.SelectedIndex = 0;

            Button updateButton = new Button
            {
                Text = "Update Status",
                Width = 120,
                Height = 32,
                Location = new Point(1035, 45)
            };

            updateButton.Click += delegate
            {
                UpdatePaymentStatus();
            };

            Button clearButton = new Button
            {
                Text = "Clear",
                Width = 80,
                Height = 32,
                Location = new Point(1165, 45)
            };

            clearButton.Click += delegate
            {
                ClearEditor();
            };

            editor.Controls.Add(paymentIdLabel);
            editor.Controls.Add(_paymentIdInput);

            editor.Controls.Add(orderIdLabel);
            editor.Controls.Add(_orderIdInput);

            editor.Controls.Add(amountLabel);
            editor.Controls.Add(_amountInput);

            editor.Controls.Add(methodLabel);
            editor.Controls.Add(_methodInput);

            editor.Controls.Add(referenceLabel);
            editor.Controls.Add(_referenceInput);

            editor.Controls.Add(statusLabel);
            editor.Controls.Add(_editStatusInput);

            editor.Controls.Add(updateButton);
            editor.Controls.Add(clearButton);

            mainPanel.Controls.Add(heading);
            mainPanel.Controls.Add(caption);
            mainPanel.Controls.Add(_searchInput);
            mainPanel.Controls.Add(_statusInput);
            mainPanel.Controls.Add(refreshButton);
            mainPanel.Controls.Add(clearFilterButton);
            mainPanel.Controls.Add(_paymentsGrid);
            mainPanel.Controls.Add(editor);

            Controls.Add(mainPanel);
        }

        private Label CreateLabel(
            string text,
            int x,
            int y)
        {
            return new Label
            {
                AutoSize = true,
                Text = text,
                Location = new Point(x, y)
            };
        }

        private TextBox CreateTextBox(
            int x,
            int y,
            int width)
        {
            return new TextBox
            {
                Width = width,
                Location = new Point(x, y)
            };
        }

        private void LoadPayments()
        {
            try
            {
                string search = _searchInput.Text.Trim();

                string selectedStatus =
                    _statusInput.SelectedItem == null
                        ? "All Status"
                        : _statusInput.SelectedItem.ToString();

                const string query = @"
SELECT
    p.PaymentId,
    p.OrderId,
    p.Amount,
    p.PaymentMethod,
    p.PaymentStatus,
    p.TransactionReference,
    p.PaidAt,
    p.CreatedAt,
    o.OrderStatus
FROM dbo.Payments AS p
INNER JOIN dbo.Orders AS o
    ON o.OrderId = p.OrderId
WHERE
    (
        @Search = N''
        OR CONVERT(NVARCHAR(50), p.PaymentId) LIKE @Pattern
        OR CONVERT(NVARCHAR(50), p.OrderId) LIKE @Pattern
        OR ISNULL(p.TransactionReference, N'') LIKE @Pattern
    )
    AND
    (
        @Status = N'All Status'
        OR p.PaymentStatus = @Status
    )
ORDER BY p.PaymentId DESC;
";

                DataTable table = new DataTable();

                using (SqlConnection connection =
                    _database.OpenConnection())
                using (SqlCommand command =
                    new SqlCommand(query, connection))
                {
                    DatabaseConnection.AddParameter(
                        command,
                        "@Search",
                        SqlDbType.NVarChar,
                        search,
                        100);

                    DatabaseConnection.AddParameter(
                        command,
                        "@Pattern",
                        SqlDbType.NVarChar,
                        "%" + search + "%",
                        120);

                    DatabaseConnection.AddParameter(
                        command,
                        "@Status",
                        SqlDbType.NVarChar,
                        selectedStatus,
                        20);

                    using (SqlDataAdapter adapter =
                        new SqlDataAdapter(command))
                    {
                        adapter.Fill(table);
                    }
                }

                _paymentsGrid.DataSource = table;
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "Unable to load payments.\n\n"
                    + exception.Message,
                    "Payment Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LoadSelectedPayment()
        {
            if (_paymentsGrid.CurrentRow == null)
            {
                return;
            }

            DataGridViewRow row =
                _paymentsGrid.CurrentRow;

            if (row.Cells["PaymentId"].Value == null)
            {
                return;
            }

            _selectedPaymentId =
                Convert.ToInt32(
                    row.Cells["PaymentId"].Value);

            _paymentIdInput.Text =
                row.Cells["PaymentId"].Value.ToString();

            _orderIdInput.Text =
                row.Cells["OrderId"].Value.ToString();

            _amountInput.Text =
                row.Cells["Amount"].Value.ToString();

            _methodInput.Text =
                row.Cells["PaymentMethod"].Value.ToString();

            _referenceInput.Text =
                row.Cells["TransactionReference"].Value == DBNull.Value
                    ? ""
                    : row.Cells["TransactionReference"].Value.ToString();

            string status =
                row.Cells["PaymentStatus"].Value.ToString();

            int index =
                _editStatusInput.Items.IndexOf(status);

            if (index >= 0)
            {
                _editStatusInput.SelectedIndex = index;
            }
        }

        private void UpdatePaymentStatus()
        {
            if (!_selectedPaymentId.HasValue)
            {
                MessageBox.Show(
                    "Please select a payment first.",
                    "Payment Management",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            string newStatus =
                _editStatusInput.SelectedItem == null
                    ? ""
                    : _editStatusInput.SelectedItem.ToString();

            DialogResult confirmation =
                MessageBox.Show(
                    "Update this payment status to "
                    + newStatus
                    + "?",
                    "Confirm Status Update",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

            if (confirmation != DialogResult.Yes)
            {
                return;
            }

            const string query = @"
UPDATE dbo.Payments
SET
    PaymentStatus = @PaymentStatus,
    PaidAt =
        CASE
            WHEN @PaymentStatus = N'Paid'
                THEN COALESCE(PaidAt, SYSDATETIME())
            ELSE PaidAt
        END
WHERE PaymentId = @PaymentId;
";

            try
            {
                using (SqlConnection connection =
                    _database.OpenConnection())
                using (SqlCommand command =
                    new SqlCommand(query, connection))
                {
                    DatabaseConnection.AddParameter(
                        command,
                        "@PaymentStatus",
                        SqlDbType.NVarChar,
                        newStatus,
                        20);

                    DatabaseConnection.AddParameter(
                        command,
                        "@PaymentId",
                        SqlDbType.Int,
                        _selectedPaymentId.Value);

                    command.ExecuteNonQuery();
                }

                MessageBox.Show(
                    "Payment status updated successfully.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadPayments();
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "Unable to update payment status.\n\n"
                    + exception.Message,
                    "Payment Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ClearEditor()
        {
            _selectedPaymentId = null;

            _paymentIdInput.Clear();
            _orderIdInput.Clear();
            _amountInput.Clear();
            _methodInput.Clear();
            _referenceInput.Clear();

            _editStatusInput.SelectedIndex = 0;

            _paymentsGrid.ClearSelection();
        }
    }
}