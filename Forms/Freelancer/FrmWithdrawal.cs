using SkillHub.Models;
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
    public partial class FrmWithdrawal : Form
    {
        // ============================================================
        // REPOSITORIES
        // ============================================================

        private readonly FreelancerWalletRepository _walletRepository;
        private readonly WithdrawalRepository _withdrawalRepository;

        // ============================================================
        // SESSION
        // ============================================================

        private readonly int _freelancerId;

        // ============================================================
        // UI
        // ============================================================

        private Panel _headerPanel;
        private Panel _contentPanel;

        private Label _titleLabel;
        private Label _subtitleLabel;
        private Label _userLabel;

        private Label _availableBalanceValue;
        private Label _ledgerBalanceValue;
        private Label _pendingBalanceValue;

        private TextBox _amountTextBox;
        private Label _amountHintLabel;
        private Label _validationLabel;

        private Button _requestButton;
        private Button _refreshButton;
        private Button _closeButton;

        private DataGridView _withdrawalGrid;

        private Label _emptyStateLabel;

        private Label _statusLabel;

        // ============================================================
        // COLORS
        // ============================================================

        private readonly Color BackgroundColor =
            Color.FromArgb(245, 247, 250);

        private readonly Color CardColor =
            Color.White;

        private readonly Color PrimaryColor =
            Color.FromArgb(37, 99, 235);

        private readonly Color PrimaryHoverColor =
            Color.FromArgb(29, 78, 216);

        private readonly Color TextColor =
            Color.FromArgb(31, 41, 55);

        private readonly Color SecondaryTextColor =
            Color.FromArgb(107, 114, 128);

        private readonly Color BorderColor =
            Color.FromArgb(229, 231, 235);

        private readonly Color SuccessColor =
            Color.FromArgb(22, 163, 74);

        private readonly Color WarningColor =
            Color.FromArgb(217, 119, 6);

        private readonly Color DangerColor =
            Color.FromArgb(220, 38, 38);

        // ============================================================
        // CONSTRUCTOR
        // ============================================================

        public FrmWithdrawal(int freelancerId)
        {
            _freelancerId = freelancerId;

            _walletRepository =
                new FreelancerWalletRepository();

            _withdrawalRepository =
                new WithdrawalRepository();

            InitializeComponent();

            BuildInterface();
            LoadWithdrawalData();
        }

        // ============================================================
        // FORM INITIALIZATION
        // ============================================================

        private void BuildInterface()
        {
            Text = "SkillHub | Withdraw Funds";

            StartPosition =
                FormStartPosition.CenterParent;

            MinimumSize =
                new Size(1050, 700);

            Size =
                new Size(1180, 760);

            BackColor =
                BackgroundColor;

            Font =
                new Font(
                    "Segoe UI",
                    9F,
                    FontStyle.Regular);

            FormBorderStyle =
                FormBorderStyle.Sizable;

            DoubleBuffered = true;

            BuildHeader();
            BuildContent();
        }

        // ============================================================
        // HEADER
        // ============================================================

        private void BuildHeader()
        {
            _headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 92,
                BackColor = Color.White,
                Padding = new Padding(32, 18, 32, 14)
            };

            Controls.Add(_headerPanel);

            _titleLabel = new Label
            {
                AutoSize = true,
                Text = "Withdraw Funds",
                Font = new Font(
                    "Segoe UI Semibold",
                    22F,
                    FontStyle.Bold),
                ForeColor = TextColor,
                Location = new Point(32, 16)
            };

            _headerPanel.Controls.Add(_titleLabel);

            _subtitleLabel = new Label
            {
                AutoSize = true,
                Text =
                    "Request a payout from your completed freelance earnings.",
                Font = new Font(
                    "Segoe UI",
                    9.5F),
                ForeColor = SecondaryTextColor,
                Location = new Point(34, 55)
            };

            _headerPanel.Controls.Add(_subtitleLabel);

            _userLabel = new Label
            {
                AutoSize = true,
                Text =
                    "Freelancer • User ID " +
                    _freelancerId,
                Font = new Font(
                    "Segoe UI Semibold",
                    9F,
                    FontStyle.Bold),
                ForeColor = SecondaryTextColor,
                Anchor =
                    AnchorStyles.Top |
                    AnchorStyles.Right
            };

            _userLabel.Location =
                new Point(
                    _headerPanel.Width -
                    _userLabel.PreferredWidth -
                    32,
                    35);

            _headerPanel.Controls.Add(_userLabel);

            _headerPanel.Resize += delegate
            {
                _userLabel.Location =
                    new Point(
                        _headerPanel.Width -
                        _userLabel.PreferredWidth -
                        32,
                        35);
            };
        }

        // ============================================================
        // MAIN CONTENT
        // ============================================================

        private void BuildContent()
        {
            _contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = BackgroundColor,
                Padding = new Padding(32, 24, 32, 28),
                AutoScroll = true
            };

            Controls.Add(_contentPanel);

            _contentPanel.BringToFront();

            BuildBalanceCards();
            BuildWithdrawalCard();
            BuildHistoryCard();
        }

        // ============================================================
        // BALANCE CARDS
        // ============================================================

        private void BuildBalanceCards()
        {
            int cardWidth = 300;
            int cardHeight = 118;
            int gap = 18;

            Panel availableCard =
                CreateCard(
                    new Rectangle(
                        32,
                        24,
                        cardWidth,
                        cardHeight));

            _contentPanel.Controls.Add(availableCard);

            AddCardCaption(
                availableCard,
                "AVAILABLE TO WITHDRAW",
                22,
                18);

            _availableBalanceValue = AddCardValue(
                availableCard,
                "BDT 0.00",
                22,
                45,
                SuccessColor);

            AddCardDescription(
                availableCard,
                "After pending withdrawal requests",
                22,
                82);

            Panel ledgerCard =
                CreateCard(
                    new Rectangle(
                        32 + cardWidth + gap,
                        24,
                        cardWidth,
                        cardHeight));

            _contentPanel.Controls.Add(ledgerCard);

            AddCardCaption(
                ledgerCard,
                "LEDGER BALANCE",
                22,
                18);

            _ledgerBalanceValue = AddCardValue(
                ledgerCard,
                "BDT 0.00",
                22,
                45,
                TextColor);

            AddCardDescription(
                ledgerCard,
                "Total wallet ledger value",
                22,
                82);

            Panel pendingCard =
                CreateCard(
                    new Rectangle(
                        32 + (cardWidth + gap) * 2,
                        24,
                        cardWidth,
                        cardHeight));

            _contentPanel.Controls.Add(pendingCard);

            AddCardCaption(
                pendingCard,
                "PENDING WITHDRAWALS",
                22,
                18);

            _pendingBalanceValue = AddCardValue(
                pendingCard,
                "BDT 0.00",
                22,
                45,
                WarningColor);

            AddCardDescription(
                pendingCard,
                "Currently awaiting approval",
                22,
                82);
        }

        // ============================================================
        // WITHDRAWAL REQUEST CARD
        // ============================================================

        private void BuildWithdrawalCard()
        {
            Panel card =
                CreateCard(
                    new Rectangle(
                        32,
                        166,
                        616,
                        255));

            _contentPanel.Controls.Add(card);

            Label heading = new Label
            {
                AutoSize = true,
                Text = "Request a withdrawal",
                Font = new Font(
                    "Segoe UI Semibold",
                    13F,
                    FontStyle.Bold),
                ForeColor = TextColor,
                Location = new Point(24, 20)
            };

            card.Controls.Add(heading);

            Label description = new Label
            {
                AutoSize = true,
                Text =
                    "Enter the amount you would like to request.",
                Font = new Font(
                    "Segoe UI",
                    9F),
                ForeColor = SecondaryTextColor,
                Location = new Point(24, 48)
            };

            card.Controls.Add(description);

            Label amountLabel = new Label
            {
                AutoSize = true,
                Text = "Withdrawal amount",
                Font = new Font(
                    "Segoe UI Semibold",
                    9F,
                    FontStyle.Bold),
                ForeColor = TextColor,
                Location = new Point(24, 82)
            };

            card.Controls.Add(amountLabel);

            _amountTextBox = new TextBox
            {
                Location = new Point(24, 108),
                Width = 330,
                Height = 34,
                Font = new Font(
                    "Segoe UI",
                    11F),
                BorderStyle = BorderStyle.FixedSingle,
                Text = ""
            };

            card.Controls.Add(_amountTextBox);

            _amountTextBox.TextChanged +=
                AmountTextBox_TextChanged;

            _amountHintLabel = new Label
            {
                AutoSize = true,
                Text = "Enter an amount greater than BDT 0.00",
                Font = new Font(
                    "Segoe UI",
                    8.5F),
                ForeColor = SecondaryTextColor,
                Location = new Point(24, 147)
            };

            card.Controls.Add(_amountHintLabel);

            _validationLabel = new Label
            {
                AutoSize = false,
                Width = 550,
                Height = 22,
                Text = "",
                Font = new Font(
                    "Segoe UI",
                    8.5F),
                ForeColor = DangerColor,
                Location = new Point(24, 170)
            };

            card.Controls.Add(_validationLabel);

            _requestButton = CreatePrimaryButton(
                "Request Withdrawal",
                new Point(24, 202),
                new Size(180, 38));

            card.Controls.Add(_requestButton);

            _requestButton.Click +=
                RequestButton_Click;
        }

        // ============================================================
        // HISTORY CARD
        // ============================================================

        private void BuildHistoryCard()
        {
            Panel card =
                CreateCard(
                    new Rectangle(
                        666,
                        166,
                        616,
                        255));

            _contentPanel.Controls.Add(card);

            Label heading = new Label
            {
                AutoSize = true,
                Text = "Withdrawal information",
                Font = new Font(
                    "Segoe UI Semibold",
                    13F,
                    FontStyle.Bold),
                ForeColor = TextColor,
                Location = new Point(24, 20)
            };

            card.Controls.Add(heading);

            Label info = new Label
            {
                AutoSize = false,
                Width = 560,
                Height = 78,
                Text =
                    "Withdrawal requests are reviewed by the " +
                    "SkillHub administrator.\r\n\r\n" +
                    "• Pending — waiting for administrator review\r\n" +
                    "• Approved — payout has been approved\r\n" +
                    "• Rejected — request was declined",
                Font = new Font(
                    "Segoe UI",
                    9F),
                ForeColor = SecondaryTextColor,
                Location = new Point(24, 55)
            };

            card.Controls.Add(info);

            _statusLabel = new Label
            {
                AutoSize = false,
                Width = 550,
                Height = 42,
                Text =
                    "Withdrawal requests are currently available.",
                Font = new Font(
                    "Segoe UI Semibold",
                    9F,
                    FontStyle.Bold),
                ForeColor = PrimaryColor,
                Location = new Point(24, 145)
            };

            card.Controls.Add(_statusLabel);

            _refreshButton = CreateSecondaryButton(
                "Refresh",
                new Point(24, 202),
                new Size(100, 38));

            card.Controls.Add(_refreshButton);

            _refreshButton.Click +=
                delegate
                {
                    LoadWithdrawalData();
                };

            _closeButton = CreateSecondaryButton(
                "Close",
                new Point(136, 202),
                new Size(100, 38));

            card.Controls.Add(_closeButton);

            _closeButton.Click +=
                delegate
                {
                    Close();
                };
        }

        // ============================================================
        // WITHDRAWAL HISTORY TABLE
        // ============================================================

        private void BuildHistoryTable()
        {
            Panel card =
                CreateCard(
                    new Rectangle(
                        32,
                        439,
                        1250,
                        285));

            _contentPanel.Controls.Add(card);

            Label heading = new Label
            {
                AutoSize = true,
                Text = "Withdrawal history",
                Font = new Font(
                    "Segoe UI Semibold",
                    13F,
                    FontStyle.Bold),
                ForeColor = TextColor,
                Location = new Point(24, 18)
            };

            card.Controls.Add(heading);

            _withdrawalGrid = new DataGridView
            {
                Location = new Point(24, 54),
                Size = new Size(1202, 205),

                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,

                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,

                ReadOnly = true,
                MultiSelect = false,
                SelectionMode =
                    DataGridViewSelectionMode.FullRowSelect,

                AutoGenerateColumns = false,

                RowHeadersVisible = false,

                EnableHeadersVisualStyles = false,

                ColumnHeadersHeight = 38,
                RowTemplate = new DataGridViewRow
                {
                    Height = 38
                },

                Font = new Font(
                    "Segoe UI",
                    9F)
            };

            _withdrawalGrid.ColumnHeadersDefaultCellStyle =
                new DataGridViewCellStyle
                {
                    BackColor =
                        Color.FromArgb(248, 250, 252),

                    ForeColor =
                        TextColor,

                    Font =
                        new Font(
                            "Segoe UI Semibold",
                            8.5F,
                            FontStyle.Bold),

                    SelectionBackColor =
                        Color.FromArgb(248, 250, 252),

                    SelectionForeColor =
                        TextColor,

                    Padding =
                        new Padding(8, 0, 8, 0)
                };

            _withdrawalGrid.DefaultCellStyle =
                new DataGridViewCellStyle
                {
                    BackColor = Color.White,
                    ForeColor = TextColor,
                    SelectionBackColor =
                        Color.FromArgb(239, 246, 255),
                    SelectionForeColor = TextColor,
                    Padding =
                        new Padding(8, 0, 8, 0)
                };

            _withdrawalGrid.AlternatingRowsDefaultCellStyle =
                new DataGridViewCellStyle
                {
                    BackColor =
                        Color.FromArgb(250, 251, 252)
                };

            AddGridColumn(
                "WithdrawalId",
                "REQUEST",
                100);

            AddGridColumn(
                "Amount",
                "AMOUNT",
                150);

            AddGridColumn(
                "Status",
                "STATUS",
                150);

            AddGridColumn(
                "RequestDate",
                "REQUESTED",
                190);

            AddGridColumn(
                "ProcessedAt",
                "PROCESSED",
                190);

            AddGridColumn(
                "AdminNote",
                "ADMIN NOTE",
                400);

            _contentPanel.Controls.Add(card);
        }

        private void AddGridColumn(
            string propertyName,
            string headerText,
            int width)
        {
            DataGridViewTextBoxColumn column =
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = propertyName,
                    HeaderText = headerText,
                    Width = width,
                    SortMode =
                        DataGridViewColumnSortMode.NotSortable
                };

            _withdrawalGrid.Columns.Add(column);
        }

        // ============================================================
        // LOAD DATA
        // ============================================================

        private void LoadWithdrawalData()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                WalletBalance balance =
                    _walletRepository.GetBalance(
                        _freelancerId);

                UpdateBalanceDisplay(balance);

                List<Withdrawal> withdrawals =
                    _withdrawalRepository.GetWithdrawals(
                        _freelancerId);

                UpdateWithdrawalGrid(withdrawals);

                UpdateWithdrawalState(balance);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load withdrawal information.\r\n\r\n" +
                    ex.Message,
                    "SkillHub",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        // ============================================================
        // BALANCE DISPLAY
        // ============================================================

        private void UpdateBalanceDisplay(
            WalletBalance balance)
        {
            _availableBalanceValue.Text =
                FormatCurrency(
                    balance.AvailableBalance);

            _ledgerBalanceValue.Text =
                FormatCurrency(
                    balance.LedgerBalance);

            _pendingBalanceValue.Text =
                FormatCurrency(
                    balance.PendingWithdrawalAmount);

            _amountHintLabel.Text =
                "Maximum available: " +
                FormatCurrency(
                    balance.AvailableBalance);
        }

        private string FormatCurrency(decimal amount)
        {
            return "BDT " +
                   amount.ToString("N2");
        }

        // ============================================================
        // WITHDRAWAL GRID
        // ============================================================

        private void UpdateWithdrawalGrid(
            List<Withdrawal> withdrawals)
        {
            if (_withdrawalGrid == null)
            {
                BuildHistoryTable();
            }

            _withdrawalGrid.DataSource = null;

            if (withdrawals == null ||
                withdrawals.Count == 0)
            {
                _withdrawalGrid.Visible = false;

                if (_emptyStateLabel == null)
                {
                    _emptyStateLabel = new Label
                    {
                        AutoSize = false,
                        TextAlign =
                            ContentAlignment.MiddleCenter,
                        Text =
                            "No withdrawal requests yet.\r\n" +
                            "Your submitted requests will appear here.",
                        Font = new Font(
                            "Segoe UI",
                            9.5F),
                        ForeColor =
                            SecondaryTextColor,
                        BackColor = Color.White,
                        Location =
                            new Point(24, 54),
                        Size =
                            new Size(1202, 205)
                    };

                    Control parent =
                        _withdrawalGrid.Parent;

                    parent.Controls.Add(
                        _emptyStateLabel);

                    _emptyStateLabel.BringToFront();
                }
                else
                {
                    _emptyStateLabel.Visible = true;
                }

                return;
            }

            if (_emptyStateLabel != null)
            {
                _emptyStateLabel.Visible = false;
            }

            _withdrawalGrid.Visible = true;

            List<WithdrawalGridRow> rows =
                withdrawals
                    .Select(
                        withdrawal =>
                            new WithdrawalGridRow
                            {
                                WithdrawalId =
                                    "#" +
                                    withdrawal
                                        .WithdrawalId
                                        .ToString("D5"),

                                Amount =
                                    FormatCurrency(
                                        withdrawal.Amount),

                                Status =
                                    withdrawal.Status,

                                RequestDate =
                                    withdrawal
                                        .RequestDate
                                        .ToString(
                                            "dd MMM yyyy, hh:mm tt"),

                                ProcessedAt =
                                    withdrawal.ProcessedAt
                                    .HasValue
                                        ? withdrawal
                                            .ProcessedAt
                                            .Value
                                            .ToString(
                                                "dd MMM yyyy, hh:mm tt")
                                        : "—",

                                AdminNote =
                                    string.IsNullOrWhiteSpace(
                                        withdrawal.AdminNote)
                                        ? "—"
                                        : withdrawal.AdminNote
                            })
                    .ToList();

            _withdrawalGrid.DataSource = rows;
        }

        // ============================================================
        // WITHDRAWAL STATE
        // ============================================================

        private void UpdateWithdrawalState(
            WalletBalance balance)
        {
            if (balance.AvailableBalance <= 0m)
            {
                _requestButton.Enabled = false;

                _statusLabel.Text =
                    "You currently have no available balance " +
                    "to withdraw.";

                _statusLabel.ForeColor =
                    SecondaryTextColor;

                return;
            }

            _requestButton.Enabled = true;

            _statusLabel.Text =
                "You can request up to " +
                FormatCurrency(
                    balance.AvailableBalance) +
                ".";

            _statusLabel.ForeColor =
                SuccessColor;
        }

        // ============================================================
        // AMOUNT VALIDATION
        // ============================================================

        private void AmountTextBox_TextChanged(
            object sender,
            EventArgs e)
        {
            ValidateAmount(false);
        }

        private bool ValidateAmount(
            bool showMessage)
        {
            _validationLabel.Text = "";

            string text =
                _amountTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(text))
            {
                return false;
            }

            decimal amount;

            if (!decimal.TryParse(
                    text,
                    out amount))
            {
                _validationLabel.Text =
                    "Enter a valid withdrawal amount.";

                return false;
            }

            if (amount <= 0m)
            {
                _validationLabel.Text =
                    "Withdrawal amount must be greater than zero.";

                return false;
            }

            WalletBalance balance =
                _walletRepository.GetBalance(
                    _freelancerId);

            if (amount > balance.AvailableBalance)
            {
                _validationLabel.Text =
                    "Amount exceeds your available balance of " +
                    FormatCurrency(
                        balance.AvailableBalance) +
                    ".";

                return false;
            }

            if (amount > 9999999999999999.99m)
            {
                _validationLabel.Text =
                    "The requested amount is too large.";

                return false;
            }

            return true;
        }

        // ============================================================
        // REQUEST WITHDRAWAL
        // ============================================================

        private void RequestButton_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                if (!ValidateAmount(true))
                {
                    return;
                }

                decimal amount =
                    Convert.ToDecimal(
                        _amountTextBox.Text.Trim());

                DialogResult confirmation =
                    MessageBox.Show(
                        "Request a withdrawal of " +
                        FormatCurrency(amount) +
                        "?\r\n\r\n" +
                        "The request will be submitted with " +
                        "Pending status for administrator review.",
                        "Confirm Withdrawal",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                if (confirmation != DialogResult.Yes)
                {
                    return;
                }

                Cursor = Cursors.WaitCursor;

                int withdrawalId =
                    _withdrawalRepository
                        .CreateWithdrawal(
                            _freelancerId,
                            amount);

                _amountTextBox.Clear();

                LoadWithdrawalData();

                MessageBox.Show(
                    "Your withdrawal request was submitted successfully.\r\n\r\n" +
                    "Request ID: #" +
                    withdrawalId.ToString("D5") +
                    "\r\n" +
                    "Amount: " +
                    FormatCurrency(amount) +
                    "\r\n" +
                    "Status: Pending",
                    "Withdrawal Submitted",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Withdrawal Request",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                LoadWithdrawalData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "The withdrawal request could not be submitted.\r\n\r\n" +
                    ex.Message,
                    "SkillHub",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        // ============================================================
        // CARD HELPERS
        // ============================================================

        private Panel CreateCard(
            Rectangle bounds)
        {
            Panel panel = new Panel
            {
                Bounds = bounds,
                BackColor = CardColor
            };

            panel.Paint +=
                delegate (object sender, PaintEventArgs e)
                {
                    using (Pen pen =
                           new Pen(BorderColor))
                    {
                        e.Graphics.DrawRectangle(
                            pen,
                            0,
                            0,
                            panel.Width - 1,
                            panel.Height - 1);
                    }
                };

            return panel;
        }

        private void AddCardCaption(
            Panel parent,
            string text,
            int x,
            int y)
        {
            Label label = new Label
            {
                AutoSize = true,
                Text = text,
                Font = new Font(
                    "Segoe UI Semibold",
                    8F,
                    FontStyle.Bold),
                ForeColor = SecondaryTextColor,
                Location = new Point(x, y)
            };

            parent.Controls.Add(label);
        }

        private Label AddCardValue(
            Panel parent,
            string text,
            int x,
            int y,
            Color color)
        {
            Label label = new Label
            {
                AutoSize = true,
                Text = text,
                Font = new Font(
                    "Segoe UI Semibold",
                    19F,
                    FontStyle.Bold),
                ForeColor = color,
                Location = new Point(x, y)
            };

            parent.Controls.Add(label);

            return label;
        }

        private void AddCardDescription(
            Panel parent,
            string text,
            int x,
            int y)
        {
            Label label = new Label
            {
                AutoSize = true,
                Text = text,
                Font = new Font(
                    "Segoe UI",
                    8F),
                ForeColor = SecondaryTextColor,
                Location = new Point(x, y)
            };

            parent.Controls.Add(label);
        }

        // ============================================================
        // BUTTON HELPERS
        // ============================================================

        private Button CreatePrimaryButton(
            string text,
            Point location,
            Size size)
        {
            Button button = new Button
            {
                Text = text,
                Location = location,
                Size = size,

                FlatStyle = FlatStyle.Flat,

                BackColor = PrimaryColor,
                ForeColor = Color.White,

                Font = new Font(
                    "Segoe UI Semibold",
                    9F,
                    FontStyle.Bold),

                Cursor = Cursors.Hand,

                FlatAppearance =
                {
                    BorderSize = 0
                }
            };

            button.MouseEnter +=
                delegate
                {
                    button.BackColor =
                        PrimaryHoverColor;
                };

            button.MouseLeave +=
                delegate
                {
                    button.BackColor =
                        PrimaryColor;
                };

            return button;
        }

        private Button CreateSecondaryButton(
            string text,
            Point location,
            Size size)
        {
            Button button = new Button
            {
                Text = text,
                Location = location,
                Size = size,

                FlatStyle = FlatStyle.Flat,

                BackColor = Color.White,
                ForeColor = TextColor,

                Font = new Font(
                    "Segoe UI Semibold",
                    9F,
                    FontStyle.Bold),

                Cursor = Cursors.Hand,

                FlatAppearance =
                {
                    BorderColor = BorderColor,
                    BorderSize = 1
                }
            };

            return button;
        }

        // ============================================================
        // GRID ROW MODEL
        // ============================================================

        private class WithdrawalGridRow
        {
            public string WithdrawalId { get; set; }

            public string Amount { get; set; }

            public string Status { get; set; }

            public string RequestDate { get; set; }

            public string ProcessedAt { get; set; }

            public string AdminNote { get; set; }
        }
    }
}