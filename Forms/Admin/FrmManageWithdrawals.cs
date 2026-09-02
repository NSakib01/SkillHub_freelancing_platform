using SkillHub.Data;
using SkillHub.Forms.Common;
using SkillHub.Services;
using SkillHub.Utilities;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace SkillHub.Forms.Admin
{
    /// <summary>
    /// Super-admin withdrawal management.
    /// View, search, approve and reject freelancer withdrawal requests.
    /// </summary>
    public sealed class FrmManageWithdrawals : Form
    {
        private readonly DatabaseConnection _database;

        private DataGridView _withdrawalsGrid;

        private TextBox _searchInput;
        private ComboBox _statusInput;

        private TextBox _withdrawalIdInput;
        private TextBox _freelancerIdInput;
        private TextBox _freelancerNameInput;
        private TextBox _amountInput;
        private TextBox _requestDateInput;
        private TextBox _adminNoteInput;

        private ComboBox _editStatusInput;

        private int? _selectedWithdrawalId;

        public FrmManageWithdrawals()
        {
            AuthorizationService.DemandAdmin();

            _database = new DatabaseConnection();

            Text = "SkillHub | Manage Withdrawals";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(1300, 800);
            MinimumSize = new Size(1100, 700);

            BuildLayout();
            LoadWithdrawals();

            UiFactory.AddBackToDashboardButton(this);
        }

        // ================================================================
        // MAIN LAYOUT
        // ================================================================

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
                Text = "Withdrawal Management",
                Font = new Font(
                    "Segoe UI",
                    20,
                    FontStyle.Bold),
                Location = new Point(20, 20)
            };

            Label caption = new Label
            {
                AutoSize = true,
                Text = "Review and manage freelancer withdrawal requests.",
                ForeColor = Color.Gray,
                Location = new Point(23, 58)
            };

            // ============================================================
            // SEARCH
            // ============================================================

            Label searchLabel = new Label
            {
                Text = "Search:",
                AutoSize = true,
                Location = new Point(20, 106)
            };

            _searchInput = new TextBox
            {
                Width = 270,
                Location = new Point(75, 100)
            };

            _searchInput.TextChanged += delegate
            {
                LoadWithdrawals();
            };

            // ============================================================
            // STATUS FILTER
            // ============================================================

            Label statusFilterLabel = new Label
            {
                Text = "Status:",
                AutoSize = true,
                Location = new Point(365, 106)
            };

            _statusInput = new ComboBox
            {
                Width = 170,
                Location = new Point(420, 100),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            _statusInput.Items.Add("All Status");
            _statusInput.Items.Add("Pending");
            _statusInput.Items.Add("Approved");
            _statusInput.Items.Add("Rejected");

            _statusInput.SelectedIndex = 0;

            _statusInput.SelectedIndexChanged += delegate
            {
                LoadWithdrawals();
            };

            // ============================================================
            // REFRESH
            // ============================================================

            Button refreshButton = new Button
            {
                Text = "Refresh",
                Width = 100,
                Height = 32,
                Location = new Point(610, 98)
            };

            refreshButton.Click += delegate
            {
                LoadWithdrawals();
            };

            // ============================================================
            // CLEAR FILTERS
            // ============================================================

            Button clearFilterButton = new Button
            {
                Text = "Clear Filters",
                Width = 120,
                Height = 32,
                Location = new Point(720, 98)
            };

            clearFilterButton.Click += delegate
            {
                _searchInput.Clear();
                _statusInput.SelectedIndex = 0;
            };

            // ============================================================
            // GRID
            // ============================================================

            _withdrawalsGrid = new DataGridView
            {
                Location = new Point(20, 145),

                Anchor = AnchorStyles.Top
                    | AnchorStyles.Bottom
                    | AnchorStyles.Left
                    | AnchorStyles.Right,

                Width = 1240,
                Height = 400,

                ReadOnly = true,

                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,

                AutoGenerateColumns = true,

                SelectionMode =
                    DataGridViewSelectionMode.FullRowSelect,

                MultiSelect = false,

                AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill
            };

            _withdrawalsGrid.SelectionChanged +=
                WithdrawalsGridSelectionChanged;

            // ============================================================
            // EDITOR PANEL
            // ============================================================

            GroupBox editorPanel = new GroupBox
            {
                Text = "Selected Withdrawal",
                Location = new Point(20, 555),
                Width = 1240,
                Height = 180,

                Anchor = AnchorStyles.Left
                    | AnchorStyles.Right
                    | AnchorStyles.Bottom
            };

            // ============================================================
            // WITHDRAWAL ID
            // ============================================================

            Label withdrawalIdLabel = new Label
            {
                Text = "Withdrawal ID",
                AutoSize = true,
                Location = new Point(15, 25)
            };

            _withdrawalIdInput = new TextBox
            {
                Width = 100,
                ReadOnly = true,
                Location = new Point(15, 45)
            };

            // ============================================================
            // FREELANCER ID
            // ============================================================

            Label freelancerIdLabel = new Label
            {
                Text = "Freelancer ID",
                AutoSize = true,
                Location = new Point(130, 25)
            };

            _freelancerIdInput = new TextBox
            {
                Width = 100,
                ReadOnly = true,
                Location = new Point(130, 45)
            };

            // ============================================================
            // FREELANCER NAME
            // ============================================================

            Label freelancerNameLabel = new Label
            {
                Text = "Freelancer",
                AutoSize = true,
                Location = new Point(245, 25)
            };

            _freelancerNameInput = new TextBox
            {
                Width = 180,
                ReadOnly = true,
                Location = new Point(245, 45)
            };

            // ============================================================
            // AMOUNT
            // ============================================================

            Label amountLabel = new Label
            {
                Text = "Amount",
                AutoSize = true,
                Location = new Point(440, 25)
            };

            _amountInput = new TextBox
            {
                Width = 120,
                ReadOnly = true,
                Location = new Point(440, 45)
            };

            // ============================================================
            // REQUEST DATE
            // ============================================================

            Label requestDateLabel = new Label
            {
                Text = "Request Date",
                AutoSize = true,
                Location = new Point(575, 25)
            };

            _requestDateInput = new TextBox
            {
                Width = 180,
                ReadOnly = true,
                Location = new Point(575, 45)
            };

            // ============================================================
            // STATUS
            // ============================================================

            Label statusLabel = new Label
            {
                Text = "Status",
                AutoSize = true,
                Location = new Point(770, 25)
            };

            _editStatusInput = new ComboBox
            {
                Width = 145,
                Location = new Point(770, 45),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            _editStatusInput.Items.Add("Pending");
            _editStatusInput.Items.Add("Approved");
            _editStatusInput.Items.Add("Rejected");

            _editStatusInput.SelectedIndex = 0;

            // ============================================================
            // ADMIN NOTE
            // ============================================================

            Label adminNoteLabel = new Label
            {
                Text = "Admin Note",
                AutoSize = true,
                Location = new Point(15, 90)
            };

            _adminNoteInput = new TextBox
            {
                Width = 500,
                Height = 45,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Location = new Point(15, 110)
            };

            // ============================================================
            // UPDATE BUTTON
            // ============================================================

            Button updateButton = new Button
            {
                Text = "Update Status",
                Width = 125,
                Height = 36,
                Location = new Point(550, 115)
            };

            updateButton.Click +=
                UpdateStatusButtonClick;

            // ============================================================
            // CLEAR BUTTON
            // ============================================================

            Button clearButton = new Button
            {
                Text = "Clear",
                Width = 100,
                Height = 36,
                Location = new Point(690, 115)
            };

            clearButton.Click += delegate
            {
                ClearEditor();
            };

            // ============================================================
            // ADD CONTROLS TO EDITOR
            // ============================================================

            editorPanel.Controls.Add(withdrawalIdLabel);
            editorPanel.Controls.Add(_withdrawalIdInput);

            editorPanel.Controls.Add(freelancerIdLabel);
            editorPanel.Controls.Add(_freelancerIdInput);

            editorPanel.Controls.Add(freelancerNameLabel);
            editorPanel.Controls.Add(_freelancerNameInput);

            editorPanel.Controls.Add(amountLabel);
            editorPanel.Controls.Add(_amountInput);

            editorPanel.Controls.Add(requestDateLabel);
            editorPanel.Controls.Add(_requestDateInput);

            editorPanel.Controls.Add(statusLabel);
            editorPanel.Controls.Add(_editStatusInput);

            editorPanel.Controls.Add(adminNoteLabel);
            editorPanel.Controls.Add(_adminNoteInput);

            editorPanel.Controls.Add(updateButton);
            editorPanel.Controls.Add(clearButton);

            // ============================================================
            // ADD TO MAIN PANEL
            // ============================================================

            mainPanel.Controls.Add(heading);
            mainPanel.Controls.Add(caption);

            mainPanel.Controls.Add(searchLabel);
            mainPanel.Controls.Add(_searchInput);

            mainPanel.Controls.Add(statusFilterLabel);
            mainPanel.Controls.Add(_statusInput);

            mainPanel.Controls.Add(refreshButton);
            mainPanel.Controls.Add(clearFilterButton);

            mainPanel.Controls.Add(_withdrawalsGrid);
            mainPanel.Controls.Add(editorPanel);

            Controls.Add(mainPanel);
        }

        // ================================================================
        // LOAD WITHDRAWALS
        // ================================================================

        private void LoadWithdrawals()
        {
            if (_withdrawalsGrid == null)
            {
                return;
            }

            try
            {
                string search =
                    (_searchInput.Text ?? string.Empty).Trim();

                string status =
                    Convert.ToString(
                        _statusInput.SelectedItem);

                string statusFilter = null;

                if (status != "All Status")
                {
                    statusFilter = status;
                }

                using (SqlConnection connection =
                    _database.OpenConnection())

                using (SqlCommand command =
                    new SqlCommand(
                        "SELECT " +

                        "w.WithdrawalId, " +
                        "w.FreelancerId, " +

                        "u.FullName AS FreelancerName, " +

                        "w.Amount, " +
                        "w.Status, " +
                        "w.RequestDate, " +

                        "w.ProcessedBy, " +

                        "processedUser.FullName AS ProcessedByName, " +

                        "w.ProcessedAt, " +
                        "w.AdminNote " +

                        "FROM dbo.WithdrawalRequests AS w " +

                        "INNER JOIN dbo.Users AS u " +
                        "ON u.UserId = w.FreelancerId " +

                        "LEFT JOIN dbo.Users AS processedUser " +
                        "ON processedUser.UserId = w.ProcessedBy " +

                        "WHERE " +

                        "(@Search = N'' " +

                        "OR u.FullName LIKE @Pattern " +

                        "OR CONVERT(NVARCHAR(50), w.WithdrawalId) "
                        + "LIKE @Pattern " +

                        "OR CONVERT(NVARCHAR(50), w.FreelancerId) "
                        + "LIKE @Pattern) " +

                        "AND " +

                        "(@Status IS NULL " +
                        "OR w.Status = @Status) " +

                        "ORDER BY w.WithdrawalId DESC;",
                        connection))
                {
                    // SEARCH

                    DatabaseConnection.AddParameter(
                        command,
                        "@Search",
                        SqlDbType.NVarChar,
                        search,
                        150);

                    DatabaseConnection.AddParameter(
                        command,
                        "@Pattern",
                        SqlDbType.NVarChar,
                        "%" + search + "%",
                        152);

                    // STATUS

                    SqlParameter statusParameter =
                        command.Parameters.Add(
                            "@Status",
                            SqlDbType.NVarChar,
                            20);

                    statusParameter.Value =
                        statusFilter == null
                            ? (object)DBNull.Value
                            : statusFilter;

                    // LOAD DATA

                    using (SqlDataAdapter adapter =
                        new SqlDataAdapter(command))
                    {
                        DataTable table =
                            new DataTable("Withdrawals");

                        adapter.Fill(table);

                        _withdrawalsGrid.DataSource = table;
                    }
                }

                ConfigureGrid();
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    this,
                    exception.Message,
                    "Withdrawal Loading Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ================================================================
        // GRID CONFIGURATION
        // ================================================================

        private void ConfigureGrid()
        {
            if (_withdrawalsGrid.Columns.Contains("WithdrawalId"))
            {
                _withdrawalsGrid.Columns["WithdrawalId"].HeaderText =
                    "Withdrawal ID";
            }

            if (_withdrawalsGrid.Columns.Contains("FreelancerId"))
            {
                _withdrawalsGrid.Columns["FreelancerId"].HeaderText =
                    "Freelancer ID";
            }

            if (_withdrawalsGrid.Columns.Contains("FreelancerName"))
            {
                _withdrawalsGrid.Columns["FreelancerName"].HeaderText =
                    "Freelancer";
            }

            if (_withdrawalsGrid.Columns.Contains("Amount"))
            {
                _withdrawalsGrid.Columns["Amount"].HeaderText =
                    "Amount";
            }

            if (_withdrawalsGrid.Columns.Contains("Status"))
            {
                _withdrawalsGrid.Columns["Status"].HeaderText =
                    "Status";
            }

            if (_withdrawalsGrid.Columns.Contains("RequestDate"))
            {
                _withdrawalsGrid.Columns["RequestDate"].HeaderText =
                    "Request Date";
            }

            if (_withdrawalsGrid.Columns.Contains("ProcessedBy"))
            {
                _withdrawalsGrid.Columns["ProcessedBy"].HeaderText =
                    "Processed By ID";
            }

            if (_withdrawalsGrid.Columns.Contains("ProcessedByName"))
            {
                _withdrawalsGrid.Columns["ProcessedByName"].HeaderText =
                    "Processed By";
            }

            if (_withdrawalsGrid.Columns.Contains("ProcessedAt"))
            {
                _withdrawalsGrid.Columns["ProcessedAt"].HeaderText =
                    "Processed At";
            }

            if (_withdrawalsGrid.Columns.Contains("AdminNote"))
            {
                _withdrawalsGrid.Columns["AdminNote"].HeaderText =
                    "Admin Note";
            }
        }

        // ================================================================
        // SELECT WITHDRAWAL
        // ================================================================

        private void WithdrawalsGridSelectionChanged(
            object sender,
            EventArgs arguments)
        {
            if (_withdrawalsGrid.CurrentRow == null)
            {
                return;
            }

            try
            {
                DataGridViewRow row =
                    _withdrawalsGrid.CurrentRow;

                if (row.Cells["WithdrawalId"].Value == null ||
                    row.Cells["WithdrawalId"].Value == DBNull.Value)
                {
                    return;
                }

                _selectedWithdrawalId =
                    Convert.ToInt32(
                        row.Cells["WithdrawalId"].Value);

                _withdrawalIdInput.Text =
                    Convert.ToString(
                        row.Cells["WithdrawalId"].Value);

                _freelancerIdInput.Text =
                    Convert.ToString(
                        row.Cells["FreelancerId"].Value);

                _freelancerNameInput.Text =
                    row.Cells["FreelancerName"].Value == DBNull.Value
                        ? string.Empty
                        : Convert.ToString(
                            row.Cells["FreelancerName"].Value);

                _amountInput.Text =
                    row.Cells["Amount"].Value == DBNull.Value
                        ? string.Empty
                        : Convert.ToString(
                            row.Cells["Amount"].Value);

                _requestDateInput.Text =
                    row.Cells["RequestDate"].Value == DBNull.Value
                        ? string.Empty
                        : Convert.ToString(
                            row.Cells["RequestDate"].Value);

                _adminNoteInput.Text =
                    row.Cells["AdminNote"].Value == DBNull.Value
                        ? string.Empty
                        : Convert.ToString(
                            row.Cells["AdminNote"].Value);

                string status =
                    Convert.ToString(
                        row.Cells["Status"].Value);

                if (_editStatusInput.Items.Contains(status))
                {
                    _editStatusInput.SelectedItem =
                        status;
                }
            }
            catch
            {
                // Ignore temporary grid selection changes.
            }
        }

        // ================================================================
        // UPDATE STATUS
        // ================================================================

        private void UpdateStatusButtonClick(
            object sender,
            EventArgs arguments)
        {
            try
            {
                if (!_selectedWithdrawalId.HasValue)
                {
                    throw new InvalidOperationException(
                        "Select a withdrawal request from the grid first.");
                }

                string newStatus =
                    Convert.ToString(
                        _editStatusInput.SelectedItem);

                string adminNote =
                    (_adminNoteInput.Text ?? string.Empty).Trim();

                if (adminNote.Length > 500)
                {
                    throw new ArgumentException(
                        "Admin note cannot exceed 500 characters.");
                }

                DialogResult result =
                    MessageBox.Show(
                        this,
                        "Update Withdrawal ID "
                        + _selectedWithdrawalId.Value
                        + " to '"
                        + newStatus
                        + "'?",
                        "Confirm Withdrawal Update",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                {
                    return;
                }

                UpdateWithdrawal(
                    _selectedWithdrawalId.Value,
                    newStatus,
                    adminNote);

                MessageBox.Show(
                    this,
                    "Withdrawal request updated successfully.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                ClearEditor();
                LoadWithdrawals();
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    this,
                    exception.Message,
                    "Withdrawal Update Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ================================================================
        // DATABASE UPDATE
        // ================================================================

        private void UpdateWithdrawal(
            int withdrawalId,
            string status,
            string adminNote)
        {
            using (SqlConnection connection =
                _database.OpenConnection())

            using (SqlCommand command =
                new SqlCommand(
                    "UPDATE dbo.WithdrawalRequests " +

                    "SET Status = @Status, " +

                    "ProcessedBy = @ProcessedBy, " +

                    "ProcessedAt = SYSDATETIME(), " +

                    "AdminNote = @AdminNote " +

                    "WHERE WithdrawalId = @WithdrawalId;",
                    connection))
            {
                DatabaseConnection.AddParameter(
                    command,
                    "@Status",
                    SqlDbType.NVarChar,
                    status,
                    20);

                DatabaseConnection.AddParameter(
                    command,
                    "@ProcessedBy",
                    SqlDbType.Int,
                    UserSession.UserId);

                DatabaseConnection.AddParameter(
                    command,
                    "@AdminNote",
                    SqlDbType.NVarChar,
                    string.IsNullOrWhiteSpace(adminNote)
                        ? (object)DBNull.Value
                        : adminNote,
                    500);

                DatabaseConnection.AddParameter(
                    command,
                    "@WithdrawalId",
                    SqlDbType.Int,
                    withdrawalId);

                if (command.ExecuteNonQuery() != 1)
                {
                    throw new InvalidOperationException(
                        "The selected withdrawal request "
                        + "could not be updated.");
                }
            }
        }

        // ================================================================
        // CLEAR EDITOR
        // ================================================================

        private void ClearEditor()
        {
            _selectedWithdrawalId = null;

            _withdrawalIdInput.Clear();
            _freelancerIdInput.Clear();
            _freelancerNameInput.Clear();
            _amountInput.Clear();
            _requestDateInput.Clear();
            _adminNoteInput.Clear();

            _editStatusInput.SelectedIndex = 0;

            if (_withdrawalsGrid != null)
            {
                _withdrawalsGrid.ClearSelection();
            }
        }
    }
}