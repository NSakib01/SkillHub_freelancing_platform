using SkillHub.Data;
using SkillHub.Forms.Common;
using SkillHub.Models;
using SkillHub.Services;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace SkillHub.Forms.Admin
{
    public sealed class FrmManageServices : Form
    {
        private readonly AuthenticationService _authentication;
        private readonly DatabaseConnection _database;

        private DataGridView _servicesGrid;
        private TextBox _searchInput;
        private ComboBox _categoryInput;
        private ComboBox _statusInput;

        public FrmManageServices(AuthenticationService authentication)
        {
            AuthorizationService.DemandAdmin();

            if (authentication == null)
            {
                throw new ArgumentNullException(nameof(authentication));
            }

            _authentication = authentication;
            _database = new DatabaseConnection();

            Text = "SkillHub | Service Moderation";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(1250, 750);
            MinimumSize = new Size(1000, 650);

            BuildLayout();
            LoadCategories();
            LoadServices();
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
                Text = "Service Moderation",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Location = new Point(20, 20)
            };

            Label caption = new Label
            {
                AutoSize = true,
                Text = "View, search, deactivate and reactivate freelancer service listings.",
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
                LoadServices();
            };

            _categoryInput = new ComboBox
            {
                Width = 210,
                Location = new Point(315, 100),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            _categoryInput.Items.Add("All Categories");
            _categoryInput.SelectedIndex = 0;

            _categoryInput.SelectedIndexChanged += delegate
            {
                LoadServices();
            };

            _statusInput = new ComboBox
            {
                Width = 150,
                Location = new Point(540, 100),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            _statusInput.Items.Add("All Status");
            _statusInput.Items.Add("Active");
            _statusInput.Items.Add("Inactive");
            _statusInput.SelectedIndex = 0;

            _statusInput.SelectedIndexChanged += delegate
            {
                LoadServices();
            };

            Button refreshButton = new Button
            {
                Text = "Refresh",
                Width = 100,
                Height = 32,
                Location = new Point(710, 98)
            };

            refreshButton.Click += delegate
            {
                LoadCategories();
                LoadServices();
            };

            _servicesGrid = new DataGridView
            {
                Location = new Point(20, 145),
                Anchor = AnchorStyles.Top
                    | AnchorStyles.Bottom
                    | AnchorStyles.Left
                    | AnchorStyles.Right,
                Width = 1160,
                Height = 470,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoGenerateColumns = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            Button deactivateButton = new Button
            {
                Text = "Deactivate",
                Width = 130,
                Height = 38,
                Location = new Point(20, 630)
            };

            deactivateButton.Click += DeactivateButtonClick;

            Button reactivateButton = new Button
            {
                Text = "Reactivate",
                Width = 130,
                Height = 38,
                Location = new Point(165, 630)
            };

            reactivateButton.Click += ReactivateButtonClick;

            Button clearButton = new Button
            {
                Text = "Clear Filters",
                Width = 130,
                Height = 38,
                Location = new Point(310, 630)
            };

            clearButton.Click += delegate
            {
                _searchInput.Clear();
                _categoryInput.SelectedIndex = 0;
                _statusInput.SelectedIndex = 0;
                LoadServices();
            };

            mainPanel.Controls.Add(heading);
            mainPanel.Controls.Add(caption);
            mainPanel.Controls.Add(_searchInput);
            mainPanel.Controls.Add(_categoryInput);
            mainPanel.Controls.Add(_statusInput);
            mainPanel.Controls.Add(refreshButton);
            mainPanel.Controls.Add(_servicesGrid);
            mainPanel.Controls.Add(deactivateButton);
            mainPanel.Controls.Add(reactivateButton);
            mainPanel.Controls.Add(clearButton);

            Controls.Add(mainPanel);
        }

        private void LoadCategories()
        {
            if (_categoryInput == null)
            {
                return;
            }

            try
            {
                string selectedCategory = Convert.ToString(
                    _categoryInput.SelectedItem);

                _categoryInput.SelectedIndexChanged -= CategoryChanged;

                _categoryInput.Items.Clear();
                _categoryInput.Items.Add("All Categories");

                using (SqlConnection connection = _database.OpenConnection())
                using (SqlCommand command = new SqlCommand(
                    "SELECT CategoryId, CategoryName " +
                    "FROM dbo.Categories " +
                    "WHERE IsActive = 1 " +
                    "ORDER BY CategoryName;",
                    connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _categoryInput.Items.Add(
                            new CategoryItem
                            {
                                Id = Convert.ToInt32(reader["CategoryId"]),
                                Name = Convert.ToString(reader["CategoryName"])
                            });
                    }
                }

                _categoryInput.SelectedIndex = 0;

                if (!string.IsNullOrEmpty(selectedCategory))
                {
                    for (int i = 0; i < _categoryInput.Items.Count; i++)
                    {
                        if (Convert.ToString(_categoryInput.Items[i])
                            == selectedCategory)
                        {
                            _categoryInput.SelectedIndex = i;
                            break;
                        }
                    }
                }

                _categoryInput.SelectedIndexChanged += CategoryChanged;
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    this,
                    exception.Message,
                    "Category Loading Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void CategoryChanged(object sender, EventArgs e)
        {
            LoadServices();
        }

        private void LoadServices()
        {
            if (_servicesGrid == null)
            {
                return;
            }

            try
            {
                string search = (_searchInput.Text ?? string.Empty).Trim();

                int? categoryId = null;

                CategoryItem category =
                    _categoryInput.SelectedItem as CategoryItem;

                if (category != null)
                {
                    categoryId = category.Id;
                }

                string status = Convert.ToString(_statusInput.SelectedItem);

                string statusFilter = null;

                if (status == "Active")
                {
                    statusFilter = "1";
                }
                else if (status == "Inactive")
                {
                    statusFilter = "0";
                }

                using (SqlConnection connection = _database.OpenConnection())
                using (SqlCommand command = new SqlCommand(
                    "SELECT " +
                    "s.ServiceId, " +
                    "s.FreelancerId, " +
                    "u.FullName AS FreelancerName, " +
                    "s.CategoryId, " +
                    "c.CategoryName, " +
                    "s.Title, " +
                    "s.Description, " +
                    "s.Price, " +
                    "s.DeliveryDays, " +
                    "s.AvailableSlots, " +
                    "s.IsActive, " +
                    "s.CreatedAt, " +
                    "s.UpdatedAt " +
                    "FROM dbo.Services AS s " +
                    "INNER JOIN dbo.Users AS u " +
                    "ON u.UserId = s.FreelancerId " +
                    "INNER JOIN dbo.Categories AS c " +
                    "ON c.CategoryId = s.CategoryId " +
                    "WHERE " +
                    "(@Search = N'' " +
                    "OR s.Title LIKE @Pattern " +
                    "OR s.Description LIKE @Pattern " +
                    "OR u.FullName LIKE @Pattern " +
                    "OR c.CategoryName LIKE @Pattern) " +
                    "AND (@CategoryId IS NULL " +
                    "OR s.CategoryId = @CategoryId) " +
                    "AND (@Status IS NULL " +
                    "OR s.IsActive = @Status) " +
                    "ORDER BY s.ServiceId DESC;",
                    connection))
                {
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

                    SqlParameter categoryParameter =
                        command.Parameters.Add("@CategoryId", SqlDbType.Int);

                    categoryParameter.Value =
                        categoryId.HasValue
                            ? (object)categoryId.Value
                            : DBNull.Value;

                    SqlParameter statusParameter =
                        command.Parameters.Add("@Status", SqlDbType.Bit);

                    statusParameter.Value =
                        statusFilter == null
                            ? (object)DBNull.Value
                            : statusFilter == "1";

                    using (SqlDataAdapter adapter =
                        new SqlDataAdapter(command))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        _servicesGrid.DataSource = table;
                    }
                }

                ConfigureGrid();
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    this,
                    exception.Message,
                    "Service Loading Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ConfigureGrid()
        {
            if (_servicesGrid.Columns.Contains("ServiceId"))
            {
                _servicesGrid.Columns["ServiceId"].HeaderText =
                    "Service ID";
            }

            if (_servicesGrid.Columns.Contains("FreelancerId"))
            {
                _servicesGrid.Columns["FreelancerId"].HeaderText =
                    "Freelancer ID";
            }

            if (_servicesGrid.Columns.Contains("FreelancerName"))
            {
                _servicesGrid.Columns["FreelancerName"].HeaderText =
                    "Freelancer";
            }

            if (_servicesGrid.Columns.Contains("CategoryName"))
            {
                _servicesGrid.Columns["CategoryName"].HeaderText =
                    "Category";
            }

            if (_servicesGrid.Columns.Contains("IsActive"))
            {
                _servicesGrid.Columns["IsActive"].HeaderText =
                    "Active";
            }
        }

        private int RequireSelectedService()
        {
            if (_servicesGrid.CurrentRow == null)
            {
                throw new InvalidOperationException(
                    "Select a service from the grid first.");
            }

            object value =
                _servicesGrid.CurrentRow.Cells["ServiceId"].Value;

            if (value == null || value == DBNull.Value)
            {
                throw new InvalidOperationException(
                    "The selected service ID is invalid.");
            }

            return Convert.ToInt32(value);
        }

        private void DeactivateButtonClick(
            object sender,
            EventArgs arguments)
        {
            try
            {
                int serviceId = RequireSelectedService();

                DialogResult result = MessageBox.Show(
                    this,
                    "Deactivate Service ID " + serviceId + "?",
                    "Confirm Service Deactivation",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result != DialogResult.Yes)
                {
                    return;
                }

                SetServiceStatus(serviceId, false);

                MessageBox.Show(
                    this,
                    "Service deactivated successfully.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadServices();
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    this,
                    exception.Message,
                    "Deactivation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ReactivateButtonClick(
            object sender,
            EventArgs arguments)
        {
            try
            {
                int serviceId = RequireSelectedService();

                DialogResult result = MessageBox.Show(
                    this,
                    "Reactivate Service ID " + serviceId + "?",
                    "Confirm Service Reactivation",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                {
                    return;
                }

                SetServiceStatus(serviceId, true);

                MessageBox.Show(
                    this,
                    "Service reactivated successfully.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadServices();
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    this,
                    exception.Message,
                    "Reactivation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void SetServiceStatus(int serviceId, bool active)
        {
            using (SqlConnection connection = _database.OpenConnection())
            using (SqlCommand command = new SqlCommand(
                "UPDATE dbo.Services " +
                "SET IsActive = @IsActive, " +
                "UpdatedAt = SYSDATETIME() " +
                "WHERE ServiceId = @ServiceId;",
                connection))
            {
                DatabaseConnection.AddParameter(
                    command,
                    "@IsActive",
                    SqlDbType.Bit,
                    active);

                DatabaseConnection.AddParameter(
                    command,
                    "@ServiceId",
                    SqlDbType.Int,
                    serviceId);

                if (command.ExecuteNonQuery() != 1)
                {
                    throw new InvalidOperationException(
                        "The selected service could not be updated.");
                }
            }
        }

        private sealed class CategoryItem
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }
    }
}