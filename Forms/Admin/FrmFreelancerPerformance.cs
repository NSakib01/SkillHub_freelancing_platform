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
    /// Super-admin freelancer performance and low-rated provider report.
    /// </summary>
    public sealed class FrmFreelancerPerformance : Form
    {
        private readonly DatabaseConnection _database;

        private DataGridView _performanceGrid;
        private TextBox _searchInput;
        private ComboBox _ratingFilter;

        public FrmFreelancerPerformance()
        {
            AuthorizationService.DemandAdmin();

            _database = new DatabaseConnection();

            Text = "SkillHub | Freelancer Performance";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(1300, 760);
            MinimumSize = new Size(1100, 650);

            BuildLayout();
            LoadPerformance();
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
                Text = "Freelancer Performance Report",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Location = new Point(20, 20)
            };

            Label caption = new Label
            {
                AutoSize = true,
                Text = "Review freelancer sales, orders, services, ratings and low-rated providers.",
                ForeColor = Color.Gray,
                Location = new Point(23, 58)
            };

            _searchInput = new TextBox
            {
                Width = 300,
                Location = new Point(20, 100)
            };

            _searchInput.TextChanged += delegate
            {
                LoadPerformance();
            };

            _ratingFilter = new ComboBox
            {
                Width = 180,
                Location = new Point(335, 100),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            _ratingFilter.Items.Add("All Ratings");
            _ratingFilter.Items.Add("Below 3.00");
            _ratingFilter.Items.Add("Below 4.00");
            _ratingFilter.SelectedIndex = 0;

            _ratingFilter.SelectedIndexChanged += delegate
            {
                LoadPerformance();
            };

            Button refreshButton = new Button
            {
                Text = "Refresh",
                Width = 100,
                Height = 32,
                Location = new Point(530, 98)
            };

            refreshButton.Click += delegate
            {
                LoadPerformance();
            };

            _performanceGrid = new DataGridView
            {
                Location = new Point(20, 150),
                Anchor = AnchorStyles.Top
                    | AnchorStyles.Bottom
                    | AnchorStyles.Left
                    | AnchorStyles.Right,
                Width = 1250,
                Height = 500,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode =
                    DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };

            mainPanel.Controls.Add(heading);
            mainPanel.Controls.Add(caption);
            mainPanel.Controls.Add(_searchInput);
            mainPanel.Controls.Add(_ratingFilter);
            mainPanel.Controls.Add(refreshButton);
            mainPanel.Controls.Add(_performanceGrid);

            Controls.Add(mainPanel);
        }

        private void LoadPerformance()
        {
            try
            {
                string search = _searchInput.Text.Trim();

                const string query = @"
SELECT
    u.UserId AS FreelancerId,
    u.FullName AS FreelancerName,
    u.Email,

    COUNT(DISTINCT s.ServiceId) AS ServiceCount,

    COUNT(DISTINCT o.OrderId) AS TotalOrders,

    COUNT(
        DISTINCT
        CASE
            WHEN o.OrderStatus = N'Completed'
            THEN o.OrderId
        END
    ) AS CompletedOrders,

    COALESCE(
        SUM(
            CASE
                WHEN o.OrderStatus = N'Completed'
                THEN o.GrossAmount
                ELSE 0
            END
        ),
        0.00
    ) AS GrossSales,

    COALESCE(
        SUM(
            CASE
                WHEN o.OrderStatus = N'Completed'
                THEN o.CommissionAmount
                ELSE 0
            END
        ),
        0.00
    ) AS PlatformCommission,

    COALESCE(
        SUM(
            CASE
                WHEN o.OrderStatus = N'Completed'
                THEN o.FreelancerEarning
                ELSE 0
            END
        ),
        0.00
    ) AS FreelancerEarnings,

    COALESCE(
        AVG(
            CAST(r.Rating AS DECIMAL(5,2))
        ),
        0.00
    ) AS AverageRating,

    COUNT(r.ReviewId) AS ReviewCount

FROM dbo.Users AS u

INNER JOIN dbo.Roles AS roleTable
    ON roleTable.RoleId = u.RoleId
    AND roleTable.RoleName = N'Freelancer'

LEFT JOIN dbo.Services AS s
    ON s.FreelancerId = u.UserId

LEFT JOIN dbo.Orders AS o
    ON o.FreelancerId = u.UserId

LEFT JOIN dbo.Reviews AS r
    ON r.FreelancerId = u.UserId

WHERE
    @Search = N''
    OR u.FullName LIKE @Pattern
    OR u.Email LIKE @Pattern
    OR CONVERT(NVARCHAR(50), u.UserId) LIKE @Pattern

GROUP BY
    u.UserId,
    u.FullName,
    u.Email

HAVING
    @RatingFilter = N'All Ratings'
    OR (
        @RatingFilter = N'Below 3.00'
        AND COALESCE(AVG(CAST(r.Rating AS DECIMAL(5,2))), 0.00) < 3.00
    )
    OR (
        @RatingFilter = N'Below 4.00'
        AND COALESCE(AVG(CAST(r.Rating AS DECIMAL(5,2))), 0.00) < 4.00
    )

ORDER BY
    AverageRating ASC,
    GrossSales DESC;
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
                        120);

                    DatabaseConnection.AddParameter(
                        command,
                        "@Pattern",
                        SqlDbType.NVarChar,
                        "%" + search + "%",
                        140);

                    DatabaseConnection.AddParameter(
                        command,
                        "@RatingFilter",
                        SqlDbType.NVarChar,
                        _ratingFilter.SelectedItem.ToString(),
                        30);

                    using (SqlDataAdapter adapter =
                        new SqlDataAdapter(command))
                    {
                        adapter.Fill(table);
                    }
                }

                _performanceGrid.DataSource = table;

                FormatGrid();
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "Unable to load freelancer performance.\n\n"
                    + exception.Message,
                    "Performance Report Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void FormatGrid()
        {
            if (_performanceGrid.Columns.Count == 0)
            {
                return;
            }

            _performanceGrid.Columns["FreelancerId"]
                .HeaderText = "Freelancer ID";

            _performanceGrid.Columns["FreelancerName"]
                .HeaderText = "Freelancer";

            _performanceGrid.Columns["ServiceCount"]
                .HeaderText = "Services";

            _performanceGrid.Columns["TotalOrders"]
                .HeaderText = "Orders";

            _performanceGrid.Columns["CompletedOrders"]
                .HeaderText = "Completed";

            _performanceGrid.Columns["GrossSales"]
                .HeaderText = "Gross Sales";

            _performanceGrid.Columns["PlatformCommission"]
                .HeaderText = "Commission";

            _performanceGrid.Columns["FreelancerEarnings"]
                .HeaderText = "Freelancer Earnings";

            _performanceGrid.Columns["AverageRating"]
                .HeaderText = "Average Rating";

            _performanceGrid.Columns["ReviewCount"]
                .HeaderText = "Reviews";

            _performanceGrid.Columns["GrossSales"]
                .DefaultCellStyle.Format = "N2";

            _performanceGrid.Columns["PlatformCommission"]
                .DefaultCellStyle.Format = "N2";

            _performanceGrid.Columns["FreelancerEarnings"]
                .DefaultCellStyle.Format = "N2";

            _performanceGrid.Columns["AverageRating"]
                .DefaultCellStyle.Format = "N2";
        }
    }
}