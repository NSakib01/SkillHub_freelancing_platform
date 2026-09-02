using SkillHub.Data;
using SkillHub.Forms.Common;
using SkillHub.Models;
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
    /// Super-admin offer management.
    /// Create, read/search, update, deactivate and reactivate marketplace offers.
    /// </summary>
    public partial class FrmManageOffers : Form
    {
        private readonly DatabaseConnection _database;

        private DataGridView _offersGrid;
        private TextBox _searchInput;

        private TextBox _serviceIdInput;
        private TextBox _titleInput;
        private TextBox _discountInput;
        private DateTimePicker _startDateInput;
        private DateTimePicker _endDateInput;
        private CheckBox _activeInput;

        private int? _selectedOfferId;

        public FrmManageOffers()
        {
            AuthorizationService.DemandAdmin();

            _database = new DatabaseConnection();

            UiFactory.ConfigureForm(
                this,
                "SkillHub | Manage Offers",
                new Size(1200, 750));

            BuildLayout();
            LoadOffers();

            UiFactory.AddBackToDashboardButton(this);
        }

        // ================================================================
        // MAIN LAYOUT
        // ================================================================

        private void BuildLayout()
        {
            TableLayoutPanel shell = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1,
                Padding = new Padding(20)
            };

            shell.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 70F));

            shell.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100F));

            Label heading = UiFactory.CreateHeading(
                "Offer Management",
                22);

            heading.Dock = DockStyle.Fill;
            heading.TextAlign = ContentAlignment.MiddleLeft;

            TableLayoutPanel body = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1
            };

            body.ColumnStyles.Add(
                new ColumnStyle(SizeType.Absolute, 360F));

            body.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 100F));

            body.Controls.Add(
                BuildEditorPanel(),
                0,
                0);

            body.Controls.Add(
                BuildGridPanel(),
                1,
                0);

            shell.Controls.Add(heading, 0, 0);
            shell.Controls.Add(body, 0, 1);

            Controls.Add(shell);
        }

        // ================================================================
        // EDITOR
        // ================================================================

        private Panel BuildEditorPanel()
        {
            Panel panel = UiFactory.CreateCard(340, 650);

            panel.Dock = DockStyle.Fill;
            panel.AutoScroll = true;
            panel.Padding = new Padding(16);

            FlowLayoutPanel fields = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true
            };

            fields.Controls.Add(
                UiFactory.CreateHeading(
                    "Offer Editor",
                    16));

            fields.Controls.Add(
                UiFactory.CreateCaption(
                    "Create, update, deactivate and reactivate marketplace offers."));

            _serviceIdInput = UiFactory.CreateTextBox(300);
            _serviceIdInput.MaxLength = 10;

            _titleInput = UiFactory.CreateTextBox(300);
            _titleInput.MaxLength = 200;

            _discountInput = UiFactory.CreateTextBox(300);
            _discountInput.MaxLength = 10;

            _startDateInput = new DateTimePicker
            {
                Width = 300,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "yyyy-MM-dd HH:mm",
                Value = DateTime.Now
            };

            _endDateInput = new DateTimePicker
            {
                Width = 300,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "yyyy-MM-dd HH:mm",
                Value = DateTime.Now.AddDays(7)
            };

            _activeInput = new CheckBox
            {
                Text = "Active",
                AutoSize = true,
                Checked = true,
                Margin = new Padding(0, 4, 0, 8)
            };

            AddField(
                fields,
                "Service ID",
                _serviceIdInput);

            AddField(
                fields,
                "Offer Title",
                _titleInput);

            AddField(
                fields,
                "Discount Percent",
                _discountInput);

            AddField(
                fields,
                "Start Date",
                _startDateInput);

            AddField(
                fields,
                "End Date",
                _endDateInput);

            fields.Controls.Add(
                UiFactory.CreateFieldLabel("Status"));

            fields.Controls.Add(_activeInput);

            FlowLayoutPanel buttons = new FlowLayoutPanel
            {
                Width = 310,
                Height = 160,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight
            };

            Button addButton =
                UiFactory.CreateButton(
                    "Add",
                    true,
                    140);

            addButton.Click += AddButtonClick;

            Button updateButton =
                UiFactory.CreateButton(
                    "Update",
                    false,
                    140);

            updateButton.Click += UpdateButtonClick;

            Button deactivateButton =
                UiFactory.CreateDangerButton(
                    "Deactivate",
                    140);

            deactivateButton.Click +=
                DeactivateButtonClick;

            Button reactivateButton =
                UiFactory.CreateButton(
                    "Reactivate",
                    false,
                    140);

            reactivateButton.Click +=
                ReactivateButtonClick;

            Button clearButton =
                UiFactory.CreateButton(
                    "Clear",
                    false,
                    140);

            clearButton.Click +=
                delegate
                {
                    ClearEditor();
                };

            buttons.Controls.Add(addButton);
            buttons.Controls.Add(updateButton);
            buttons.Controls.Add(deactivateButton);
            buttons.Controls.Add(reactivateButton);
            buttons.Controls.Add(clearButton);

            fields.Controls.Add(buttons);

            panel.Controls.Add(fields);

            return panel;
        }

        private static void AddField(
            FlowLayoutPanel fields,
            string label,
            Control input)
        {
            fields.Controls.Add(
                UiFactory.CreateFieldLabel(label));

            fields.Controls.Add(input);
        }

        // ================================================================
        // GRID
        // ================================================================

        private Panel BuildGridPanel()
        {
            Panel panel = UiFactory.CreateCard(
                800,
                650);

            panel.Dock = DockStyle.Fill;
            panel.Padding = new Padding(16);

            TableLayoutPanel layout =
                new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    RowCount = 2,
                    ColumnCount = 1
                };

            layout.RowStyles.Add(
                new RowStyle(
                    SizeType.Absolute,
                    50F));

            layout.RowStyles.Add(
                new RowStyle(
                    SizeType.Percent,
                    100F));

            FlowLayoutPanel searchBar =
                new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    FlowDirection =
                        FlowDirection.LeftToRight,
                    WrapContents = false
                };

            _searchInput =
                UiFactory.CreateTextBox(400);

            _searchInput.Margin =
                new Padding(0, 6, 10, 0);

            _searchInput.TextChanged +=
                delegate
                {
                    LoadOffers();
                };

            Button refreshButton =
                UiFactory.CreateButton(
                    "Refresh",
                    false,
                    110,
                    35);

            refreshButton.Click +=
                delegate
                {
                    LoadOffers();
                };

            searchBar.Controls.Add(
                _searchInput);

            searchBar.Controls.Add(
                refreshButton);

            _offersGrid =
                UiFactory.CreateReadOnlyGrid();

            _offersGrid.SelectionChanged +=
                OffersGridSelectionChanged;

            layout.Controls.Add(
                searchBar,
                0,
                0);

            layout.Controls.Add(
                _offersGrid,
                0,
                1);

            panel.Controls.Add(layout);

            return panel;
        }

        // ================================================================
        // LOAD OFFERS
        // ================================================================

        private void LoadOffers()
        {
            if (_offersGrid == null)
            {
                return;
            }

            try
            {
                string search =
                    (_searchInput == null
                        ? string.Empty
                        : _searchInput.Text.Trim());

                using (SqlConnection connection =
                    _database.OpenConnection())

                using (SqlCommand command =
                    new SqlCommand(
                        "SELECT "
                        + "o.OfferId, "
                        + "o.ServiceId, "
                        + "s.Title AS ServiceTitle, "
                        + "o.OfferTitle, "
                        + "o.DiscountPercent, "
                        + "o.StartDate, "
                        + "o.EndDate, "
                        + "o.IsActive, "
                        + "o.CreatedBy, "
                        + "o.CreatedAt "
                        + "FROM dbo.Offers AS o "
                        + "INNER JOIN dbo.Services AS s "
                        + "ON s.ServiceId = o.ServiceId "
                        + "WHERE @Search = N'' "
                        + "OR o.OfferTitle LIKE @Pattern "
                        + "OR s.Title LIKE @Pattern "
                        + "ORDER BY o.OfferId DESC;",
                        connection))
                {
                    DatabaseConnection.AddParameter(
                        command,
                        "@Search",
                        SqlDbType.NVarChar,
                        search,
                        200);

                    DatabaseConnection.AddParameter(
                        command,
                        "@Pattern",
                        SqlDbType.NVarChar,
                        "%" + search + "%",
                        202);

                    using (SqlDataAdapter adapter =
                        new SqlDataAdapter(command))
                    {
                        DataTable table =
                            new DataTable("Offers");

                        adapter.Fill(table);

                        _offersGrid.DataSource = table;
                    }
                }

                FormatGrid();
            }
            catch (Exception exception)
            {
                UiFactory.ShowError(
                    this,
                    exception);
            }
        }

        // ================================================================
        // GRID FORMATTING
        // ================================================================

        private void FormatGrid()
        {
            if (_offersGrid.Columns.Contains("OfferId"))
            {
                _offersGrid.Columns["OfferId"]
                    .HeaderText = "Offer ID";
            }

            if (_offersGrid.Columns.Contains("ServiceId"))
            {
                _offersGrid.Columns["ServiceId"]
                    .HeaderText = "Service ID";
            }

            if (_offersGrid.Columns.Contains("ServiceTitle"))
            {
                _offersGrid.Columns["ServiceTitle"]
                    .HeaderText = "Service";
            }

            if (_offersGrid.Columns.Contains("OfferTitle"))
            {
                _offersGrid.Columns["OfferTitle"]
                    .HeaderText = "Offer Title";
            }

            if (_offersGrid.Columns.Contains("DiscountPercent"))
            {
                _offersGrid.Columns["DiscountPercent"]
                    .HeaderText = "Discount %";
            }

            if (_offersGrid.Columns.Contains("StartDate"))
            {
                _offersGrid.Columns["StartDate"]
                    .HeaderText = "Start Date";
            }

            if (_offersGrid.Columns.Contains("EndDate"))
            {
                _offersGrid.Columns["EndDate"]
                    .HeaderText = "End Date";
            }

            if (_offersGrid.Columns.Contains("IsActive"))
            {
                _offersGrid.Columns["IsActive"]
                    .HeaderText = "Active";
            }

            if (_offersGrid.Columns.Contains("CreatedBy"))
            {
                _offersGrid.Columns["CreatedBy"]
                    .HeaderText = "Created By";
            }

            if (_offersGrid.Columns.Contains("CreatedAt"))
            {
                _offersGrid.Columns["CreatedAt"]
                    .HeaderText = "Created At";
            }
        }

        // ================================================================
        // SELECT OFFER
        // ================================================================

        private void OffersGridSelectionChanged(
            object sender,
            EventArgs arguments)
        {
            if (_offersGrid.CurrentRow == null ||
                _offersGrid.SelectedRows.Count == 0)
            {
                return;
            }

            DataGridViewRow row =
                _offersGrid.CurrentRow;

            try
            {
                _selectedOfferId =
                    Convert.ToInt32(
                        row.Cells["OfferId"].Value);

                _serviceIdInput.Text =
                    Convert.ToString(
                        row.Cells["ServiceId"].Value);

                _titleInput.Text =
                    Convert.ToString(
                        row.Cells["OfferTitle"].Value);

                _discountInput.Text =
                    Convert.ToString(
                        row.Cells["DiscountPercent"].Value);

                _startDateInput.Value =
                    Convert.ToDateTime(
                        row.Cells["StartDate"].Value);

                _endDateInput.Value =
                    Convert.ToDateTime(
                        row.Cells["EndDate"].Value);

                _activeInput.Checked =
                    Convert.ToBoolean(
                        row.Cells["IsActive"].Value);
            }
            catch
            {
                // Ignore temporary grid selection changes.
            }
        }

        // ================================================================
        // ADD OFFER
        // ================================================================

        private void AddButtonClick(
            object sender,
            EventArgs arguments)
        {
            try
            {
                if (_selectedOfferId.HasValue)
                {
                    throw new InvalidOperationException(
                        "Click Clear before creating a new offer.");
                }

                ValidateEditor();

                int serviceId =
                    Convert.ToInt32(
                        _serviceIdInput.Text.Trim());

                decimal discount =
                    Convert.ToDecimal(
                        _discountInput.Text.Trim());

                if (!ServiceExists(serviceId))
                {
                    throw new ArgumentException(
                        "The selected Service ID does not exist.");
                }

                if (_endDateInput.Value <=
                    _startDateInput.Value)
                {
                    throw new ArgumentException(
                        "End date must be later than start date.");
                }

                using (SqlConnection connection =
                    _database.OpenConnection())

                using (SqlCommand command =
                    new SqlCommand(
                        "INSERT INTO dbo.Offers "
                        + "(ServiceId, OfferTitle, DiscountPercent, "
                        + "StartDate, EndDate, IsActive, CreatedBy) "
                        + "VALUES "
                        + "(@ServiceId, @OfferTitle, @DiscountPercent, "
                        + "@StartDate, @EndDate, @IsActive, @CreatedBy);",
                        connection))
                {
                    AddOfferParameters(
                        command,
                        serviceId,
                        discount);

                    DatabaseConnection.AddParameter(
                        command,
                        "@CreatedBy",
                        SqlDbType.Int,
                        UserSession.UserId);

                    command.ExecuteNonQuery();
                }

                UiFactory.ShowSuccess(
                    this,
                    "Offer created successfully.");

                ClearEditor();
                LoadOffers();
            }
            catch (Exception exception)
            {
                UiFactory.ShowError(
                    this,
                    exception);
            }
        }

        // ================================================================
        // UPDATE OFFER
        // ================================================================

        private void UpdateButtonClick(
            object sender,
            EventArgs arguments)
        {
            try
            {
                if (!_selectedOfferId.HasValue)
                {
                    throw new InvalidOperationException(
                        "Select an offer before updating.");
                }

                ValidateEditor();

                int serviceId =
                    Convert.ToInt32(
                        _serviceIdInput.Text.Trim());

                decimal discount =
                    Convert.ToDecimal(
                        _discountInput.Text.Trim());

                if (!ServiceExists(serviceId))
                {
                    throw new ArgumentException(
                        "The selected Service ID does not exist.");
                }

                if (_endDateInput.Value <=
                    _startDateInput.Value)
                {
                    throw new ArgumentException(
                        "End date must be later than start date.");
                }

                using (SqlConnection connection =
                    _database.OpenConnection())

                using (SqlCommand command =
                    new SqlCommand(
                        "UPDATE dbo.Offers "
                        + "SET ServiceId = @ServiceId, "
                        + "OfferTitle = @OfferTitle, "
                        + "DiscountPercent = @DiscountPercent, "
                        + "StartDate = @StartDate, "
                        + "EndDate = @EndDate, "
                        + "IsActive = @IsActive "
                        + "WHERE OfferId = @OfferId;",
                        connection))
                {
                    AddOfferParameters(
                        command,
                        serviceId,
                        discount);

                    DatabaseConnection.AddParameter(
                        command,
                        "@OfferId",
                        SqlDbType.Int,
                        _selectedOfferId.Value);

                    if (command.ExecuteNonQuery() != 1)
                    {
                        throw new InvalidOperationException(
                            "The selected offer could not be updated.");
                    }
                }

                UiFactory.ShowSuccess(
                    this,
                    "Offer updated successfully.");

                ClearEditor();
                LoadOffers();
            }
            catch (Exception exception)
            {
                UiFactory.ShowError(
                    this,
                    exception);
            }
        }

        // ================================================================
        // DEACTIVATE OFFER
        // ================================================================

        private void DeactivateButtonClick(
            object sender,
            EventArgs arguments)
        {
            try
            {
                if (!_selectedOfferId.HasValue)
                {
                    throw new InvalidOperationException(
                        "Select an offer before deactivating.");
                }

                DialogResult result =
                    MessageBox.Show(
                        this,
                        "Deactivate Offer ID "
                        + _selectedOfferId.Value
                        + "?",
                        "Confirm Deactivation",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                if (result != DialogResult.Yes)
                {
                    return;
                }

                using (SqlConnection connection =
                    _database.OpenConnection())

                using (SqlCommand command =
                    new SqlCommand(
                        "UPDATE dbo.Offers "
                        + "SET IsActive = 0 "
                        + "WHERE OfferId = @OfferId;",
                        connection))
                {
                    DatabaseConnection.AddParameter(
                        command,
                        "@OfferId",
                        SqlDbType.Int,
                        _selectedOfferId.Value);

                    if (command.ExecuteNonQuery() != 1)
                    {
                        throw new InvalidOperationException(
                            "The offer could not be deactivated.");
                    }
                }

                UiFactory.ShowSuccess(
                    this,
                    "Offer deactivated successfully.");

                ClearEditor();
                LoadOffers();
            }
            catch (Exception exception)
            {
                UiFactory.ShowError(
                    this,
                    exception);
            }
        }

        // ================================================================
        // REACTIVATE OFFER
        // ================================================================

        private void ReactivateButtonClick(
            object sender,
            EventArgs arguments)
        {
            try
            {
                if (!_selectedOfferId.HasValue)
                {
                    throw new InvalidOperationException(
                        "Select an offer before reactivating.");
                }

                DialogResult result =
                    MessageBox.Show(
                        this,
                        "Reactivate Offer ID "
                        + _selectedOfferId.Value
                        + "?",
                        "Confirm Reactivation",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                {
                    return;
                }

                using (SqlConnection connection =
                    _database.OpenConnection())

                using (SqlCommand command =
                    new SqlCommand(
                        "UPDATE dbo.Offers "
                        + "SET IsActive = 1 "
                        + "WHERE OfferId = @OfferId;",
                        connection))
                {
                    DatabaseConnection.AddParameter(
                        command,
                        "@OfferId",
                        SqlDbType.Int,
                        _selectedOfferId.Value);

                    if (command.ExecuteNonQuery() != 1)
                    {
                        throw new InvalidOperationException(
                            "The offer could not be reactivated.");
                    }
                }

                UiFactory.ShowSuccess(
                    this,
                    "Offer reactivated successfully.");

                ClearEditor();
                LoadOffers();
            }
            catch (Exception exception)
            {
                UiFactory.ShowError(
                    this,
                    exception);
            }
        }

        // ================================================================
        // VALIDATION
        // ================================================================

        private void ValidateEditor()
        {
            if (string.IsNullOrWhiteSpace(
                _serviceIdInput.Text))
            {
                throw new ArgumentException(
                    "Service ID is required.");
            }

            int serviceId;

            if (!int.TryParse(
                _serviceIdInput.Text.Trim(),
                out serviceId) ||
                serviceId <= 0)
            {
                throw new ArgumentException(
                    "Service ID must be a valid positive number.");
            }

            if (string.IsNullOrWhiteSpace(
                _titleInput.Text))
            {
                throw new ArgumentException(
                    "Offer title is required.");
            }

            decimal discount;

            if (!decimal.TryParse(
                _discountInput.Text.Trim(),
                out discount))
            {
                throw new ArgumentException(
                    "Discount percent must be a valid number.");
            }

            if (discount < 0 ||
                discount > 100)
            {
                throw new ArgumentException(
                    "Discount percent must be between 0 and 100.");
            }
        }

        // ================================================================
        // SERVICE CHECK
        // ================================================================

        private bool ServiceExists(int serviceId)
        {
            using (SqlConnection connection =
                _database.OpenConnection())

            using (SqlCommand command =
                new SqlCommand(
                    "SELECT COUNT(*) "
                    + "FROM dbo.Services "
                    + "WHERE ServiceId = @ServiceId;",
                    connection))
            {
                DatabaseConnection.AddParameter(
                    command,
                    "@ServiceId",
                    SqlDbType.Int,
                    serviceId);

                return Convert.ToInt32(
                    command.ExecuteScalar()) > 0;
            }
        }

        // ================================================================
        // SQL PARAMETERS
        // ================================================================

        private void AddOfferParameters(
            SqlCommand command,
            int serviceId,
            decimal discount)
        {
            DatabaseConnection.AddParameter(
                command,
                "@ServiceId",
                SqlDbType.Int,
                serviceId);

            DatabaseConnection.AddParameter(
                command,
                "@OfferTitle",
                SqlDbType.NVarChar,
                _titleInput.Text.Trim(),
                200);

            SqlParameter discountParameter =
                command.Parameters.Add(
                    "@DiscountPercent",
                    SqlDbType.Decimal);

            discountParameter.Precision = 5;
            discountParameter.Scale = 2;
            discountParameter.Value = discount;

            DatabaseConnection.AddParameter(
                command,
                "@StartDate",
                SqlDbType.DateTime2,
                _startDateInput.Value);

            DatabaseConnection.AddParameter(
                command,
                "@EndDate",
                SqlDbType.DateTime2,
                _endDateInput.Value);

            DatabaseConnection.AddParameter(
                command,
                "@IsActive",
                SqlDbType.Bit,
                _activeInput.Checked);
        }

        // ================================================================
        // CLEAR
        // ================================================================

        private void ClearEditor()
        {
            _selectedOfferId = null;

            _serviceIdInput.Clear();
            _titleInput.Clear();
            _discountInput.Clear();

            _startDateInput.Value =
                DateTime.Now;

            _endDateInput.Value =
                DateTime.Now.AddDays(7);

            _activeInput.Checked = true;

            if (_offersGrid != null)
            {
                _offersGrid.ClearSelection();
            }

            _serviceIdInput.Focus();
        }
    }
}