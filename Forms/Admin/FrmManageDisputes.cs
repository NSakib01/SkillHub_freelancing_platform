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
    /// Super-admin dispute management.
    /// View, search, review, resolve and reject customer disputes.
    /// </summary>
    public sealed class FrmManageDisputes : Form
    {
        private readonly DatabaseConnection _database;

        private DataGridView _disputesGrid;

        private TextBox _searchInput;
        private ComboBox _statusInput;

        private TextBox _disputeIdInput;
        private TextBox _orderIdInput;
        private TextBox _openedByInput;
        private TextBox _reasonInput;
        private TextBox _resolutionInput;

        private ComboBox _editStatusInput;

        private int? _selectedDisputeId;

        public FrmManageDisputes()
        {
            AuthorizationService.DemandAdmin();

            _database = new DatabaseConnection();

            Text = "SkillHub | Manage Disputes";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(1300, 800);
            MinimumSize = new Size(1100, 700);

            BuildLayout();
            LoadDisputes();
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
                Text = "Dispute Management",
                Font = new Font(
                    "Segoe UI",
                    20,
                    FontStyle.Bold),
                Location = new Point(20, 20)
            };

            Label caption = new Label
            {
                AutoSize = true,
                Text = "Review customer complaints and manage dispute resolution.",
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
                LoadDisputes();
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
            _statusInput.Items.Add("Open");
            _statusInput.Items.Add("Under Review");
            _statusInput.Items.Add("Resolved");
            _statusInput.Items.Add("Rejected");

            _statusInput.SelectedIndex = 0;

            _statusInput.SelectedIndexChanged += delegate
            {
                LoadDisputes();
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
                LoadDisputes();
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

            _disputesGrid = new DataGridView
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

            _disputesGrid.SelectionChanged +=
                DisputesGridSelectionChanged;

            // ============================================================
            // EDITOR PANEL
            // ============================================================

            GroupBox editorPanel = new GroupBox
            {
                Text = "Selected Dispute",
                Location = new Point(20, 555),
                Width = 1240,
                Height = 180,

                Anchor = AnchorStyles.Left
                    | AnchorStyles.Right
                    | AnchorStyles.Bottom
            };

            // ============================================================
            // DISPUTE ID
            // ============================================================

            Label disputeIdLabel = new Label
            {
                Text = "Dispute ID",
                AutoSize = true,
                Location = new Point(15, 25)
            };

            _disputeIdInput = new TextBox
            {
                Width = 90,
                ReadOnly = true,
                Location = new Point(15, 45)
            };

            // ============================================================
            // ORDER ID
            // ============================================================

            Label orderIdLabel = new Label
            {
                Text = "Order ID",
                AutoSize = true,
                Location = new Point(120, 25)
            };

            _orderIdInput = new TextBox
            {
                Width = 90,
                ReadOnly = true,
                Location = new Point(120, 45)
            };

            // ============================================================
            // OPENED BY
            // ============================================================

            Label openedByLabel = new Label
            {
                Text = "Opened By",
                AutoSize = true,
                Location = new Point(225, 25)
            };

            _openedByInput = new TextBox
            {
                Width = 90,
                ReadOnly = true,
                Location = new Point(225, 45)
            };

            // ============================================================
            // STATUS
            // ============================================================

            Label statusLabel = new Label
            {
                Text = "Status",
                AutoSize = true,
                Location = new Point(330, 25)
            };

            _editStatusInput = new ComboBox
            {
                Width = 145,
                Location = new Point(330, 45),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            _editStatusInput.Items.Add("Open");
            _editStatusInput.Items.Add("Under Review");
            _editStatusInput.Items.Add("Resolved");
            _editStatusInput.Items.Add("Rejected");

            _editStatusInput.SelectedIndex = 0;

            // ============================================================
            // REASON
            // ============================================================

            Label reasonLabel = new Label
            {
                Text = "Reason",
                AutoSize = true,
                Location = new Point(490, 25)
            };

            _reasonInput = new TextBox
            {
                Width = 330,
                Height = 55,
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Location = new Point(490, 45)
            };

            // ============================================================
            // RESOLUTION
            // ============================================================

            Label resolutionLabel = new Label
            {
                Text = "Resolution",
                AutoSize = true,
                Location = new Point(15, 110)
            };

            _resolutionInput = new TextBox
            {
                Width = 500,
                Height = 45,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Location = new Point(15, 130)
            };

            // ============================================================
            // UPDATE BUTTON
            // ============================================================

            Button updateButton = new Button
            {
                Text = "Update Status",
                Width = 125,
                Height = 36,
                Location = new Point(550, 130)
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
                Location = new Point(690, 130)
            };

            clearButton.Click += delegate
            {
                ClearEditor();
            };

            // ============================================================
            // ADD CONTROLS TO EDITOR
            // ============================================================

            editorPanel.Controls.Add(disputeIdLabel);
            editorPanel.Controls.Add(_disputeIdInput);

            editorPanel.Controls.Add(orderIdLabel);
            editorPanel.Controls.Add(_orderIdInput);

            editorPanel.Controls.Add(openedByLabel);
            editorPanel.Controls.Add(_openedByInput);

            editorPanel.Controls.Add(statusLabel);
            editorPanel.Controls.Add(_editStatusInput);

            editorPanel.Controls.Add(reasonLabel);
            editorPanel.Controls.Add(_reasonInput);

            editorPanel.Controls.Add(resolutionLabel);
            editorPanel.Controls.Add(_resolutionInput);

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

            mainPanel.Controls.Add(_disputesGrid);
            mainPanel.Controls.Add(editorPanel);

            Controls.Add(mainPanel);
        }

        // ================================================================
        // LOAD DISPUTES
        // ================================================================

        private void LoadDisputes()
        {
            if (_disputesGrid == null)
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

                        "d.DisputeId, " +
                        "d.OrderId, " +

                        "d.OpenedBy, " +
                        "openedUser.FullName AS OpenedByName, " +

                        "d.Reason, " +
                        "d.Status, " +
                        "d.Resolution, " +

                        "d.ResolvedBy, " +
                        "resolvedUser.FullName AS ResolvedByName, " +

                        "d.CreatedAt, " +
                        "d.ResolvedAt " +

                        "FROM dbo.Disputes AS d " +

                        "INNER JOIN dbo.Users AS openedUser " +
                        "ON openedUser.UserId = d.OpenedBy " +

                        "LEFT JOIN dbo.Users AS resolvedUser " +
                        "ON resolvedUser.UserId = d.ResolvedBy " +

                        "WHERE " +

                        "(@Search = N'' " +

                        "OR d.Reason LIKE @Pattern " +
                        "OR d.Resolution LIKE @Pattern " +

                        "OR openedUser.FullName LIKE @Pattern " +

                        "OR CONVERT(NVARCHAR(50), d.OrderId) " +
                        "LIKE @Pattern " +

                        "OR CONVERT(NVARCHAR(50), d.DisputeId) " +
                        "LIKE @Pattern) " +

                        "AND " +

                        "(@Status IS NULL " +
                        "OR d.Status = @Status) " +

                        "ORDER BY d.DisputeId DESC;",
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
                            new DataTable("Disputes");

                        adapter.Fill(table);

                        _disputesGrid.DataSource = table;
                    }
                }

                ConfigureGrid();
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    this,
                    exception.Message,
                    "Dispute Loading Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ================================================================
        // GRID CONFIGURATION
        // ================================================================

        private void ConfigureGrid()
        {
            if (_disputesGrid.Columns.Contains("DisputeId"))
            {
                _disputesGrid.Columns["DisputeId"].HeaderText =
                    "Dispute ID";
            }

            if (_disputesGrid.Columns.Contains("OrderId"))
            {
                _disputesGrid.Columns["OrderId"].HeaderText =
                    "Order ID";
            }

            if (_disputesGrid.Columns.Contains("OpenedBy"))
            {
                _disputesGrid.Columns["OpenedBy"].HeaderText =
                    "Opened By ID";
            }

            if (_disputesGrid.Columns.Contains("OpenedByName"))
            {
                _disputesGrid.Columns["OpenedByName"].HeaderText =
                    "Opened By";
            }

            if (_disputesGrid.Columns.Contains("Reason"))
            {
                _disputesGrid.Columns["Reason"].HeaderText =
                    "Reason";
                _disputesGrid.Columns["Reason"].FillWeight = 150;
            }

            if (_disputesGrid.Columns.Contains("Status"))
            {
                _disputesGrid.Columns["Status"].HeaderText =
                    "Status";
            }

            if (_disputesGrid.Columns.Contains("Resolution"))
            {
                _disputesGrid.Columns["Resolution"].HeaderText =
                    "Resolution";
                _disputesGrid.Columns["Resolution"].FillWeight = 150;
            }

            if (_disputesGrid.Columns.Contains("ResolvedBy"))
            {
                _disputesGrid.Columns["ResolvedBy"].HeaderText =
                    "Resolved By ID";
            }

            if (_disputesGrid.Columns.Contains("ResolvedByName"))
            {
                _disputesGrid.Columns["ResolvedByName"].HeaderText =
                    "Resolved By";
            }

            if (_disputesGrid.Columns.Contains("CreatedAt"))
            {
                _disputesGrid.Columns["CreatedAt"].HeaderText =
                    "Created At";
            }

            if (_disputesGrid.Columns.Contains("ResolvedAt"))
            {
                _disputesGrid.Columns["ResolvedAt"].HeaderText =
                    "Resolved At";
            }
        }

        // ================================================================
        // SELECT DISPUTE
        // ================================================================

        private void DisputesGridSelectionChanged(
            object sender,
            EventArgs arguments)
        {
            if (_disputesGrid.CurrentRow == null)
            {
                return;
            }

            try
            {
                DataGridViewRow row =
                    _disputesGrid.CurrentRow;

                if (row.Cells["DisputeId"].Value == null ||
                    row.Cells["DisputeId"].Value == DBNull.Value)
                {
                    return;
                }

                _selectedDisputeId =
                    Convert.ToInt32(
                        row.Cells["DisputeId"].Value);

                _disputeIdInput.Text =
                    Convert.ToString(
                        row.Cells["DisputeId"].Value);

                _orderIdInput.Text =
                    Convert.ToString(
                        row.Cells["OrderId"].Value);

                _openedByInput.Text =
                    Convert.ToString(
                        row.Cells["OpenedBy"].Value);

                _reasonInput.Text =
                    row.Cells["Reason"].Value == DBNull.Value
                        ? string.Empty
                        : Convert.ToString(
                            row.Cells["Reason"].Value);

                _resolutionInput.Text =
                    row.Cells["Resolution"].Value == DBNull.Value
                        ? string.Empty
                        : Convert.ToString(
                            row.Cells["Resolution"].Value);

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
                if (!_selectedDisputeId.HasValue)
                {
                    throw new InvalidOperationException(
                        "Select a dispute from the grid first.");
                }

                string newStatus =
                    Convert.ToString(
                        _editStatusInput.SelectedItem);

                string resolution =
                    (_resolutionInput.Text ?? string.Empty).Trim();

                if (newStatus == "Resolved" ||
                    newStatus == "Rejected")
                {
                    if (string.IsNullOrWhiteSpace(resolution))
                    {
                        throw new ArgumentException(
                            "Resolution is required when resolving "
                            + "or rejecting a dispute.");
                    }

                    if (resolution.Length > 1000)
                    {
                        throw new ArgumentException(
                            "Resolution cannot exceed 1000 characters.");
                    }
                }

                DialogResult result =
                    MessageBox.Show(
                        this,
                        "Update Dispute ID "
                        + _selectedDisputeId.Value
                        + " to '"
                        + newStatus
                        + "'?",
                        "Confirm Dispute Update",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                {
                    return;
                }

                UpdateDispute(
                    _selectedDisputeId.Value,
                    newStatus,
                    resolution);

                MessageBox.Show(
                    this,
                    "Dispute updated successfully.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                ClearEditor();
                LoadDisputes();
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    this,
                    exception.Message,
                    "Dispute Update Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ================================================================
        // DATABASE UPDATE
        // ================================================================

        private void UpdateDispute(
            int disputeId,
            string status,
            string resolution)
        {
            using (SqlConnection connection =
                _database.OpenConnection())

            using (SqlCommand command =
                new SqlCommand(
                    "UPDATE dbo.Disputes " +

                    "SET Status = @Status, " +

                    "Resolution = " +
                    "CASE " +
                    "WHEN @Status IN " +
                    "(N'Resolved', N'Rejected') " +
                    "THEN @Resolution " +
                    "ELSE Resolution " +
                    "END, " +

                    "ResolvedBy = " +
                    "CASE " +
                    "WHEN @Status IN " +
                    "(N'Resolved', N'Rejected') " +
                    "THEN @ResolvedBy " +
                    "ELSE NULL " +
                    "END, " +

                    "ResolvedAt = " +
                    "CASE " +
                    "WHEN @Status IN " +
                    "(N'Resolved', N'Rejected') " +
                    "THEN SYSDATETIME() " +
                    "ELSE NULL " +
                    "END " +

                    "WHERE DisputeId = @DisputeId;",
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
                    "@Resolution",
                    SqlDbType.NVarChar,
                    string.IsNullOrWhiteSpace(resolution)
                        ? (object)DBNull.Value
                        : resolution,
                    1000);

                DatabaseConnection.AddParameter(
                    command,
                    "@ResolvedBy",
                    SqlDbType.Int,
                    UserSession.UserId);

                DatabaseConnection.AddParameter(
                    command,
                    "@DisputeId",
                    SqlDbType.Int,
                    disputeId);

                if (command.ExecuteNonQuery() != 1)
                {
                    throw new InvalidOperationException(
                        "The selected dispute could not be updated.");
                }
            }
        }

        // ================================================================
        // CLEAR EDITOR
        // ================================================================

        private void ClearEditor()
        {
            _selectedDisputeId = null;

            _disputeIdInput.Clear();
            _orderIdInput.Clear();
            _openedByInput.Clear();
            _reasonInput.Clear();
            _resolutionInput.Clear();

            _editStatusInput.SelectedIndex = 0;

            if (_disputesGrid != null)
            {
                _disputesGrid.ClearSelection();
            }
        }
    }
}