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
    /// Super-admin platform revenue and financial reporting.
    /// Shows order totals, completed revenue, commission,
    /// freelancer earnings and payment statistics.
    /// </summary>
    public sealed class FrmRevenueReport : Form
    {
        private readonly DatabaseConnection _database;

        private DateTimePicker _fromDateInput;
        private DateTimePicker _toDateInput;

        private Label _totalOrdersLabel;
        private Label _completedOrdersLabel;
        private Label _grossRevenueLabel;
        private Label _commissionLabel;
        private Label _freelancerEarningLabel;
        private Label _paidAmountLabel;

        private DataGridView _reportGrid;

        public FrmRevenueReport()
        {
            AuthorizationService.DemandAdmin();

            _database = new DatabaseConnection();

            Text = "SkillHub | Revenue & Financial Report";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(1350, 800);
            MinimumSize = new Size(1150, 700);

            BuildLayout();
            LoadReport();
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
                Text = "Revenue & Financial Report",
                Font = new Font(
                    "Segoe UI",
                    22,
                    FontStyle.Bold),
                Location = new Point(20, 20)
            };

            Label caption = new Label
            {
                AutoSize = true,
                Text = "View platform sales, payments, commission earnings and freelancer earnings.",
                ForeColor = Color.Gray,
                Location = new Point(23, 60)
            };

            // ============================================================
            // DATE FILTER
            // ============================================================

            Label fromLabel = new Label
            {
                AutoSize = true,
                Text = "From Date",
                Location = new Point(20, 105)
            };

            _fromDateInput = new DateTimePicker
            {
                Width = 150,
                Format = DateTimePickerFormat.Short,
                Location = new Point(20, 128)
            };

            Label toLabel = new Label
            {
                AutoSize = true,
                Text = "To Date",
                Location = new Point(190, 105)
            };

            _toDateInput = new DateTimePicker
            {
                Width = 150,
                Format = DateTimePickerFormat.Short,
                Location = new Point(190, 128)
            };

            _fromDateInput.Value = DateTime.Today.AddMonths(-1);
            _toDateInput.Value = DateTime.Today;

            Button reportButton = new Button
            {
                Text = "Generate Report",
                Width = 140,
                Height = 32,
                Location = new Point(360, 126)
            };

            reportButton.Click += delegate
            {
                LoadReport();
            };

            Button refreshButton = new Button
            {
                Text = "Refresh",
                Width = 100,
                Height = 32,
                Location = new Point(515, 126)
            };

            refreshButton.Click += delegate
            {
                LoadReport();
            };

            // ============================================================
            // SUMMARY CARDS
            // ============================================================

            _totalOrdersLabel = CreateSummaryLabel(
                "Total Orders",
                20,
                190);

            _completedOrdersLabel = CreateSummaryLabel(
                "Completed Orders",
                225,
                190);

            _grossRevenueLabel = CreateSummaryLabel(
                "Gross Revenue",
                430,
                190);

            _commissionLabel = CreateSummaryLabel(
                "Platform Commission",
                635,
                190);

            _freelancerEarningLabel = CreateSummaryLabel(
                "Freelancer Earnings",
                840,
                190);

            _paidAmountLabel = CreateSummaryLabel(
                "Paid Amount",
                1045,
                190);

            // ============================================================
            // REPORT GRID
            // ============================================================

            _reportGrid = new DataGridView
            {
                Location = new Point(20, 275),
                Anchor = AnchorStyles.Top
                    | AnchorStyles.Bottom
                    | AnchorStyles.Left
                    | AnchorStyles.Right,
                Width = 1270,
                Height = 440,
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

            mainPanel.Controls.Add(fromLabel);
            mainPanel.Controls.Add(_fromDateInput);

            mainPanel.Controls.Add(toLabel);
            mainPanel.Controls.Add(_toDateInput);

            mainPanel.Controls.Add(reportButton);
            mainPanel.Controls.Add(refreshButton);

            mainPanel.Controls.Add(_totalOrdersLabel);
            mainPanel.Controls.Add(_completedOrdersLabel);
            mainPanel.Controls.Add(_grossRevenueLabel);
            mainPanel.Controls.Add(_commissionLabel);
            mainPanel.Controls.Add(_freelancerEarningLabel);
            mainPanel.Controls.Add(_paidAmountLabel);

            mainPanel.Controls.Add(_reportGrid);

            Controls.Add(mainPanel);
        }

        // ================================================================
        // SUMMARY LABEL
        // ================================================================

        private Label CreateSummaryLabel(
            string title,
            int x,
            int y)
        {
            Label label = new Label
            {
                AutoSize = false,
                Width = 185,
                Height = 65,
                Text = title + Environment.NewLine + "0",
                Font = new Font(
                    "Segoe UI",
                    10,
                    FontStyle.Bold),
                BorderStyle = BorderStyle.FixedSingle,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(x, y)
            };

            return label;
        }

        // ================================================================
        // LOAD REPORT
        // ================================================================

        private void LoadReport()
        {
            try
            {
                if (_fromDateInput.Value.Date >
                    _toDateInput.Value.Date)
                {
                    MessageBox.Show(
                        "From Date cannot be greater than To Date.",
                        "Invalid Date Range",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                DateTime fromDate =
                    _fromDateInput.Value.Date;

                DateTime toDate =
                    _toDateInput.Value.Date.AddDays(1);

                LoadSummary(fromDate, toDate);
                LoadDetails(fromDate, toDate);
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "Unable to load financial report.\n\n"
                    + exception.Message,
                    "Revenue Report Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ================================================================
        // SUMMARY
        // ================================================================

        private void LoadSummary(
            DateTime fromDate,
            DateTime toDate)
        {
            const string query = @"
SELECT
    COUNT(*) AS TotalOrders,

    SUM(
        CASE
            WHEN o.OrderStatus = N'Completed'
            THEN 1
            ELSE 0
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
    ) AS GrossRevenue,

    COALESCE(
        SUM(
            CASE
                WHEN o.OrderStatus = N'Completed'
                THEN o.CommissionAmount
                ELSE 0
            END
        ),
        0.00
    ) AS CommissionRevenue,

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
        (
            SELECT SUM(p.Amount)
            FROM dbo.Payments AS p
            WHERE p.PaymentStatus = N'Paid'
              AND p.PaidAt >= @FromDate
              AND p.PaidAt < @ToDate
        ),
        0.00
    ) AS PaidAmount

FROM dbo.Orders AS o
WHERE o.CreatedAt >= @FromDate
  AND o.CreatedAt < @ToDate;
";

            using (SqlConnection connection =
                _database.OpenConnection())
            using (SqlCommand command =
                new SqlCommand(query, connection))
            {
                DatabaseConnection.AddParameter(
                    command,
                    "@FromDate",
                    SqlDbType.DateTime2,
                    fromDate);

                DatabaseConnection.AddParameter(
                    command,
                    "@ToDate",
                    SqlDbType.DateTime2,
                    toDate);

                using (SqlDataReader reader =
                    command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int totalOrders =
                            Convert.ToInt32(
                                reader["TotalOrders"]);

                        int completedOrders =
                            Convert.ToInt32(
                                reader["CompletedOrders"]);

                        decimal grossRevenue =
                            Convert.ToDecimal(
                                reader["GrossRevenue"]);

                        decimal commissionRevenue =
                            Convert.ToDecimal(
                                reader["CommissionRevenue"]);

                        decimal freelancerEarnings =
                            Convert.ToDecimal(
                                reader["FreelancerEarnings"]);

                        decimal paidAmount =
                            Convert.ToDecimal(
                                reader["PaidAmount"]);

                        _totalOrdersLabel.Text =
                            "Total Orders"
                            + Environment.NewLine
                            + totalOrders;

                        _completedOrdersLabel.Text =
                            "Completed Orders"
                            + Environment.NewLine
                            + completedOrders;

                        _grossRevenueLabel.Text =
                            "Gross Revenue"
                            + Environment.NewLine
                            + "BDT "
                            + grossRevenue.ToString("N2");

                        _commissionLabel.Text =
                            "Platform Commission"
                            + Environment.NewLine
                            + "BDT "
                            + commissionRevenue.ToString("N2");

                        _freelancerEarningLabel.Text =
                            "Freelancer Earnings"
                            + Environment.NewLine
                            + "BDT "
                            + freelancerEarnings.ToString("N2");

                        _paidAmountLabel.Text =
                            "Paid Amount"
                            + Environment.NewLine
                            + "BDT "
                            + paidAmount.ToString("N2");
                    }
                }
            }
        }

        // ================================================================
        // DETAIL REPORT
        // ================================================================

        private void LoadDetails(
            DateTime fromDate,
            DateTime toDate)
        {
            const string query = @"
SELECT
    o.OrderId,
    o.CreatedAt AS OrderDate,
    o.OrderStatus,
    o.GrossAmount,
    o.DiscountAmount,
    o.CommissionRate,
    o.CommissionAmount,
    o.FreelancerEarning,

    p.PaymentId,
    p.Amount AS PaymentAmount,
    p.PaymentMethod,
    p.PaymentStatus,
    p.TransactionReference,
    p.PaidAt

FROM dbo.Orders AS o

LEFT JOIN dbo.Payments AS p
    ON p.OrderId = o.OrderId

WHERE o.CreatedAt >= @FromDate
  AND o.CreatedAt < @ToDate

ORDER BY o.CreatedAt DESC,
         o.OrderId DESC;
";

            DataTable table = new DataTable();

            using (SqlConnection connection =
                _database.OpenConnection())
            using (SqlCommand command =
                new SqlCommand(query, connection))
            {
                DatabaseConnection.AddParameter(
                    command,
                    "@FromDate",
                    SqlDbType.DateTime2,
                    fromDate);

                DatabaseConnection.AddParameter(
                    command,
                    "@ToDate",
                    SqlDbType.DateTime2,
                    toDate);

                using (SqlDataAdapter adapter =
                    new SqlDataAdapter(command))
                {
                    adapter.Fill(table);
                }
            }

            _reportGrid.DataSource = table;

            FormatGrid();
        }

        // ================================================================
        // GRID FORMATTING
        // ================================================================

        private void FormatGrid()
        {
            if (_reportGrid.Columns.Count == 0)
            {
                return;
            }

            if (_reportGrid.Columns.Contains("OrderId"))
            {
                _reportGrid.Columns["OrderId"].HeaderText =
                    "Order ID";
            }

            if (_reportGrid.Columns.Contains("OrderDate"))
            {
                _reportGrid.Columns["OrderDate"].HeaderText =
                    "Order Date";
            }

            if (_reportGrid.Columns.Contains("OrderStatus"))
            {
                _reportGrid.Columns["OrderStatus"].HeaderText =
                    "Order Status";
            }

            if (_reportGrid.Columns.Contains("GrossAmount"))
            {
                _reportGrid.Columns["GrossAmount"].HeaderText =
                    "Gross Amount";

                _reportGrid.Columns["GrossAmount"]
                    .DefaultCellStyle.Format = "N2";
            }

            if (_reportGrid.Columns.Contains("DiscountAmount"))
            {
                _reportGrid.Columns["DiscountAmount"].HeaderText =
                    "Discount";

                _reportGrid.Columns["DiscountAmount"]
                    .DefaultCellStyle.Format = "N2";
            }

            if (_reportGrid.Columns.Contains("CommissionRate"))
            {
                _reportGrid.Columns["CommissionRate"].HeaderText =
                    "Commission %";

                _reportGrid.Columns["CommissionRate"]
                    .DefaultCellStyle.Format = "N2";
            }

            if (_reportGrid.Columns.Contains("CommissionAmount"))
            {
                _reportGrid.Columns["CommissionAmount"].HeaderText =
                    "Commission";

                _reportGrid.Columns["CommissionAmount"]
                    .DefaultCellStyle.Format = "N2";
            }

            if (_reportGrid.Columns.Contains("FreelancerEarning"))
            {
                _reportGrid.Columns["FreelancerEarning"].HeaderText =
                    "Freelancer Earning";

                _reportGrid.Columns["FreelancerEarning"]
                    .DefaultCellStyle.Format = "N2";
            }

            if (_reportGrid.Columns.Contains("PaymentId"))
            {
                _reportGrid.Columns["PaymentId"].HeaderText =
                    "Payment ID";
            }

            if (_reportGrid.Columns.Contains("PaymentAmount"))
            {
                _reportGrid.Columns["PaymentAmount"].HeaderText =
                    "Payment Amount";

                _reportGrid.Columns["PaymentAmount"]
                    .DefaultCellStyle.Format = "N2";
            }

            if (_reportGrid.Columns.Contains("PaymentMethod"))
            {
                _reportGrid.Columns["PaymentMethod"].HeaderText =
                    "Payment Method";
            }

            if (_reportGrid.Columns.Contains("PaymentStatus"))
            {
                _reportGrid.Columns["PaymentStatus"].HeaderText =
                    "Payment Status";
            }

            if (_reportGrid.Columns.Contains("TransactionReference"))
            {
                _reportGrid.Columns["TransactionReference"]
                    .HeaderText =
                    "Transaction Reference";
            }

            if (_reportGrid.Columns.Contains("PaidAt"))
            {
                _reportGrid.Columns["PaidAt"].HeaderText =
                    "Paid At";
            }
        }
    }
}