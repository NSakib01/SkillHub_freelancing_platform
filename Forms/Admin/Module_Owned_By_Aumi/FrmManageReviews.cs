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
    public sealed class FrmManageReviews : Form
    {
        private readonly DatabaseConnection _database;

        private DataGridView _reviewsGrid;
        private TextBox _searchInput;
        private ComboBox _ratingInput;

        public FrmManageReviews()
        {
            AuthorizationService.DemandAdmin();

            _database = new DatabaseConnection();

            Text = "SkillHub | Manage Reviews";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(1250, 750);
            MinimumSize = new Size(1000, 650);

            BuildLayout();
            LoadReviews();
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
                Text = "Review Management",
                Font = new Font(
                    "Segoe UI",
                    20,
                    FontStyle.Bold),
                Location = new Point(20, 20)
            };

            Label caption = new Label
            {
                AutoSize = true,
                Text = "View and monitor customer reviews and ratings.",
                ForeColor = Color.Gray,
                Location = new Point(23, 58)
            };

            // ------------------------------------------------------------
            // SEARCH
            // ------------------------------------------------------------

            _searchInput = new TextBox
            {
                Width = 300,
                Location = new Point(20, 100)
            };

            _searchInput.TextChanged += delegate
            {
                LoadReviews();
            };

            // ------------------------------------------------------------
            // RATING FILTER
            // ------------------------------------------------------------

            _ratingInput = new ComboBox
            {
                Width = 150,
                Location = new Point(335, 100),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            _ratingInput.Items.Add("All Ratings");
            _ratingInput.Items.Add("5 Stars");
            _ratingInput.Items.Add("4 Stars");
            _ratingInput.Items.Add("3 Stars");
            _ratingInput.Items.Add("2 Stars");
            _ratingInput.Items.Add("1 Star");

            _ratingInput.SelectedIndex = 0;

            _ratingInput.SelectedIndexChanged += delegate
            {
                LoadReviews();
            };

            // ------------------------------------------------------------
            // REFRESH
            // ------------------------------------------------------------

            Button refreshButton = new Button
            {
                Text = "Refresh",
                Width = 100,
                Height = 32,
                Location = new Point(500, 98)
            };

            refreshButton.Click += delegate
            {
                LoadReviews();
            };

            // ------------------------------------------------------------
            // CLEAR FILTERS
            // ------------------------------------------------------------

            Button clearButton = new Button
            {
                Text = "Clear Filters",
                Width = 120,
                Height = 32,
                Location = new Point(610, 98)
            };

            clearButton.Click += delegate
            {
                _searchInput.Clear();
                _ratingInput.SelectedIndex = 0;
                LoadReviews();
            };

            // ------------------------------------------------------------
            // GRID
            // ------------------------------------------------------------

            _reviewsGrid = new DataGridView
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

                SelectionMode =
                    DataGridViewSelectionMode.FullRowSelect,

                MultiSelect = false,

                AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill
            };

            // ------------------------------------------------------------
            // CONTROLS
            // ------------------------------------------------------------

            mainPanel.Controls.Add(heading);
            mainPanel.Controls.Add(caption);

            mainPanel.Controls.Add(_searchInput);
            mainPanel.Controls.Add(_ratingInput);

            mainPanel.Controls.Add(refreshButton);
            mainPanel.Controls.Add(clearButton);

            mainPanel.Controls.Add(_reviewsGrid);

            Controls.Add(mainPanel);
        }

        // ================================================================
        // LOAD REVIEWS
        // ================================================================

        private void LoadReviews()
        {
            if (_reviewsGrid == null)
            {
                return;
            }

            try
            {
                string search =
                    (_searchInput.Text ?? string.Empty).Trim();

                int? rating = null;

                if (_ratingInput.SelectedIndex > 0)
                {
                    rating =
                        6 - _ratingInput.SelectedIndex;
                }

                using (SqlConnection connection =
                    _database.OpenConnection())

                using (SqlCommand command =
                    new SqlCommand(
                        "SELECT " +

                        "r.ReviewId, " +

                        "r.OrderId, " +

                        "r.ClientId, " +
                        "clientUser.FullName AS ClientName, " +

                        "r.FreelancerId, " +
                        "freelancerUser.FullName AS FreelancerName, " +

                        "r.Rating, " +
                        "r.Comment, " +
                        "r.ReviewDate " +

                        "FROM dbo.Reviews AS r " +

                        "INNER JOIN dbo.Users AS clientUser " +
                        "ON clientUser.UserId = r.ClientId " +

                        "INNER JOIN dbo.Users AS freelancerUser " +
                        "ON freelancerUser.UserId = r.FreelancerId " +

                        "WHERE " +

                        "(@Search = N'' " +

                        "OR r.Comment LIKE @Pattern " +

                        "OR clientUser.FullName LIKE @Pattern " +

                        "OR freelancerUser.FullName LIKE @Pattern " +

                        "OR CONVERT(NVARCHAR(50), r.OrderId) " +
                        "LIKE @Pattern) " +

                        "AND " +

                        "(@Rating IS NULL " +
                        "OR r.Rating = @Rating) " +

                        "ORDER BY r.ReviewId DESC;",
                        connection))
                {
                    // ----------------------------------------------------
                    // SEARCH
                    // ----------------------------------------------------

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

                    // ----------------------------------------------------
                    // RATING
                    // ----------------------------------------------------

                    SqlParameter ratingParameter =
                        command.Parameters.Add(
                            "@Rating",
                            SqlDbType.TinyInt);

                    ratingParameter.Value =
                        rating.HasValue
                            ? (object)rating.Value
                            : DBNull.Value;

                    // ----------------------------------------------------
                    // DATA
                    // ----------------------------------------------------

                    using (SqlDataAdapter adapter =
                        new SqlDataAdapter(command))
                    {
                        DataTable table =
                            new DataTable("Reviews");

                        adapter.Fill(table);

                        _reviewsGrid.DataSource = table;
                    }
                }

                ConfigureGrid();
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    this,
                    exception.Message,
                    "Review Loading Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ================================================================
        // GRID CONFIGURATION
        // ================================================================

        private void ConfigureGrid()
        {
            if (_reviewsGrid.Columns.Contains("ReviewId"))
            {
                _reviewsGrid.Columns["ReviewId"].HeaderText =
                    "Review ID";
            }

            if (_reviewsGrid.Columns.Contains("OrderId"))
            {
                _reviewsGrid.Columns["OrderId"].HeaderText =
                    "Order ID";
            }

            if (_reviewsGrid.Columns.Contains("ClientId"))
            {
                _reviewsGrid.Columns["ClientId"].HeaderText =
                    "Client ID";
            }

            if (_reviewsGrid.Columns.Contains("ClientName"))
            {
                _reviewsGrid.Columns["ClientName"].HeaderText =
                    "Client";
            }

            if (_reviewsGrid.Columns.Contains("FreelancerId"))
            {
                _reviewsGrid.Columns["FreelancerId"].HeaderText =
                    "Freelancer ID";
            }

            if (_reviewsGrid.Columns.Contains("FreelancerName"))
            {
                _reviewsGrid.Columns["FreelancerName"].HeaderText =
                    "Freelancer";
            }

            if (_reviewsGrid.Columns.Contains("Rating"))
            {
                _reviewsGrid.Columns["Rating"].HeaderText =
                    "Rating";
            }

            if (_reviewsGrid.Columns.Contains("Comment"))
            {
                _reviewsGrid.Columns["Comment"].HeaderText =
                    "Comment";
            }

            if (_reviewsGrid.Columns.Contains("ReviewDate"))
            {
                _reviewsGrid.Columns["ReviewDate"].HeaderText =
                    "Review Date";
            }
        }
    }
}