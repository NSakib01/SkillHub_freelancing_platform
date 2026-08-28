namespace SkillHub.Forms.Freelancer
{
    partial class FrmWallet
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;

        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnWithdraw;

        private System.Windows.Forms.Panel availableCard;
        private System.Windows.Forms.Panel earnedCard;
        private System.Windows.Forms.Panel pendingCard;
        private System.Windows.Forms.Panel ledgerCard;

        private System.Windows.Forms.Label lblAvailableCaption;
        private System.Windows.Forms.Label lblAvailableBalance;
        private System.Windows.Forms.Label lblBalanceStatus;

        private System.Windows.Forms.Label lblEarnedCaption;
        private System.Windows.Forms.Label lblTotalEarned;

        private System.Windows.Forms.Label lblPendingCaption;
        private System.Windows.Forms.Label lblPendingWithdrawal;

        private System.Windows.Forms.Label lblLedgerCaption;
        private System.Windows.Forms.Label lblLedgerBalance;

        private System.Windows.Forms.Panel transactionPanel;
        private System.Windows.Forms.Label lblTransactionTitle;
        private System.Windows.Forms.Label lblTransactionCount;
        private System.Windows.Forms.Label lblEmptyTransactions;

        private System.Windows.Forms.DataGridView dgvTransactions;

        private System.Windows.Forms.DataGridViewTextBoxColumn colDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReference;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAmount;

        private System.Windows.Forms.Label lblLastRefresh;
        private System.Windows.Forms.Button btnClose;

        protected override void Dispose(bool disposing)
        {
            if (disposing &&
                (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }


        private void InitializeComponent()
        {
            this.components =
                new System.ComponentModel.Container();

            this.headerPanel =
                new System.Windows.Forms.Panel();

            this.lblTitle =
                new System.Windows.Forms.Label();

            this.lblSubtitle =
                new System.Windows.Forms.Label();

            this.btnRefresh =
                new System.Windows.Forms.Button();

            this.btnWithdraw =
                new System.Windows.Forms.Button();

            this.availableCard =
                new System.Windows.Forms.Panel();

            this.lblAvailableCaption =
                new System.Windows.Forms.Label();

            this.lblAvailableBalance =
                new System.Windows.Forms.Label();

            this.lblBalanceStatus =
                new System.Windows.Forms.Label();

            this.earnedCard =
                new System.Windows.Forms.Panel();

            this.lblEarnedCaption =
                new System.Windows.Forms.Label();

            this.lblTotalEarned =
                new System.Windows.Forms.Label();

            this.pendingCard =
                new System.Windows.Forms.Panel();

            this.lblPendingCaption =
                new System.Windows.Forms.Label();

            this.lblPendingWithdrawal =
                new System.Windows.Forms.Label();

            this.ledgerCard =
                new System.Windows.Forms.Panel();

            this.lblLedgerCaption =
                new System.Windows.Forms.Label();

            this.lblLedgerBalance =
                new System.Windows.Forms.Label();

            this.transactionPanel =
                new System.Windows.Forms.Panel();

            this.lblTransactionTitle =
                new System.Windows.Forms.Label();

            this.lblTransactionCount =
                new System.Windows.Forms.Label();

            this.lblEmptyTransactions =
                new System.Windows.Forms.Label();

            this.dgvTransactions =
                new System.Windows.Forms.DataGridView();

            this.colDate =
                new System.Windows.Forms.DataGridViewTextBoxColumn();

            this.colType =
                new System.Windows.Forms.DataGridViewTextBoxColumn();

            this.colReference =
                new System.Windows.Forms.DataGridViewTextBoxColumn();

            this.colDescription =
                new System.Windows.Forms.DataGridViewTextBoxColumn();

            this.colAmount =
                new System.Windows.Forms.DataGridViewTextBoxColumn();

            this.lblLastRefresh =
                new System.Windows.Forms.Label();

            this.btnClose =
                new System.Windows.Forms.Button();

            // ========================================================
            // FORM
            // ========================================================

            this.SuspendLayout();

            this.ClientSize =
                new System.Drawing.Size(1180, 720);

            this.MinimumSize =
                new System.Drawing.Size(1000, 650);

            this.Name =
                "FrmWallet";

            this.StartPosition =
                System.Windows.Forms.FormStartPosition.CenterParent;

            this.Text =
                "My Wallet | SkillHub";

            this.BackColor =
                System.Drawing.Color.FromArgb(245, 247, 250);

            // ========================================================
            // HEADER
            // ========================================================

            this.headerPanel.Dock =
                System.Windows.Forms.DockStyle.Top;

            this.headerPanel.Height =
                92;

            this.headerPanel.BackColor =
                System.Drawing.Color.White;

            this.headerPanel.Padding =
                new System.Windows.Forms.Padding(28, 18, 28, 12);

            this.headerPanel.Controls.Add(
                this.lblSubtitle);

            this.headerPanel.Controls.Add(
                this.lblTitle);

            this.headerPanel.Controls.Add(
                this.btnClose);

            this.headerPanel.Controls.Add(
                this.btnRefresh);

            this.headerPanel.Controls.Add(
                this.btnWithdraw);

            // TITLE

            this.lblTitle.AutoSize =
                true;

            this.lblTitle.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    21F,
                    System.Drawing.FontStyle.Bold);

            this.lblTitle.ForeColor =
                System.Drawing.Color.FromArgb(
                    30, 41, 59);

            this.lblTitle.Location =
                new System.Drawing.Point(28, 14);

            this.lblTitle.Text =
                "My Wallet";

            // SUBTITLE

            this.lblSubtitle.AutoSize =
                true;

            this.lblSubtitle.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    9.5F);

            this.lblSubtitle.ForeColor =
                System.Drawing.Color.FromArgb(
                    100, 116, 139);

            this.lblSubtitle.Location =
                new System.Drawing.Point(31, 53);

            this.lblSubtitle.Text =
                "Track your earnings, balance and wallet activity";

            // WITHDRAW BUTTON

            this.btnWithdraw.Anchor =
                System.Windows.Forms.AnchorStyles.Top |
                System.Windows.Forms.AnchorStyles.Right;

            this.btnWithdraw.FlatStyle =
                System.Windows.Forms.FlatStyle.Flat;

            this.btnWithdraw.FlatAppearance.BorderSize =
                0;

            this.btnWithdraw.BackColor =
                System.Drawing.Color.FromArgb(
                    37, 99, 235);

            this.btnWithdraw.ForeColor =
                System.Drawing.Color.White;

            this.btnWithdraw.Font =
                new System.Drawing.Font(
                    "Segoe UI Semibold",
                    9.5F,
                    System.Drawing.FontStyle.Bold);

            this.btnWithdraw.Size =
                new System.Drawing.Size(120, 38);

            this.btnWithdraw.Location =
                new System.Drawing.Point(865, 27);

            this.btnWithdraw.Text =
                "Withdraw";

            this.btnWithdraw.Cursor =
                System.Windows.Forms.Cursors.Hand;

            this.btnWithdraw.Click +=
                new System.EventHandler(
                    this.btnWithdraw_Click);

            // REFRESH

            this.btnRefresh.Anchor =
                System.Windows.Forms.AnchorStyles.Top |
                System.Windows.Forms.AnchorStyles.Right;

            this.btnRefresh.FlatStyle =
                System.Windows.Forms.FlatStyle.Flat;

            this.btnRefresh.FlatAppearance.BorderColor =
                System.Drawing.Color.FromArgb(
                    203, 213, 225);

            this.btnRefresh.BackColor =
                System.Drawing.Color.White;

            this.btnRefresh.ForeColor =
                System.Drawing.Color.FromArgb(
                    51, 65, 85);

            this.btnRefresh.Size =
                new System.Drawing.Size(95, 38);

            this.btnRefresh.Location =
                new System.Drawing.Point(995, 27);

            this.btnRefresh.Text =
                "Refresh";

            this.btnRefresh.Cursor =
                System.Windows.Forms.Cursors.Hand;

            this.btnRefresh.Click +=
                new System.EventHandler(
                    this.btnRefresh_Click);

            // CLOSE

            this.btnClose.Anchor =
                System.Windows.Forms.AnchorStyles.Top |
                System.Windows.Forms.AnchorStyles.Right;

            this.btnClose.FlatStyle =
                System.Windows.Forms.FlatStyle.Flat;

            this.btnClose.FlatAppearance.BorderSize =
                0;

            this.btnClose.BackColor =
                System.Drawing.Color.Transparent;

            this.btnClose.ForeColor =
                System.Drawing.Color.FromArgb(
                    100, 116, 139);

            this.btnClose.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    12F);

            this.btnClose.Size =
                new System.Drawing.Size(35, 35);

            this.btnClose.Location =
                new System.Drawing.Point(1118, 28);

            this.btnClose.Text =
                "×";

            this.btnClose.Cursor =
                System.Windows.Forms.Cursors.Hand;

            this.btnClose.Click +=
                new System.EventHandler(
                    this.btnClose_Click);

            // ========================================================
            // AVAILABLE CARD
            // ========================================================

            ConfigureCard(
                this.availableCard,
                28,
                116,
                260,
                125);

            this.availableCard.Controls.Add(
                this.lblAvailableCaption);

            this.availableCard.Controls.Add(
                this.lblAvailableBalance);

            this.availableCard.Controls.Add(
                this.lblBalanceStatus);

            ConfigureCaption(
                this.lblAvailableCaption,
                "AVAILABLE BALANCE",
                18,
                15);

            ConfigureMoneyLabel(
                this.lblAvailableBalance,
                "৳ 0.00",
                18,
                42);

            this.lblAvailableBalance.ForeColor =
                System.Drawing.Color.FromArgb(
                    22, 101, 52);

            ConfigureSmallLabel(
                this.lblBalanceStatus,
                "No withdrawable funds yet",
                18,
                88);

            // ========================================================
            // TOTAL EARNED
            // ========================================================

            ConfigureCard(
                this.earnedCard,
                302,
                116,
                260,
                125);

            this.earnedCard.Controls.Add(
                this.lblEarnedCaption);

            this.earnedCard.Controls.Add(
                this.lblTotalEarned);

            ConfigureCaption(
                this.lblEarnedCaption,
                "TOTAL EARNED",
                18,
                15);

            ConfigureMoneyLabel(
                this.lblTotalEarned,
                "৳ 0.00",
                18,
                50);

            // ========================================================
            // PENDING
            // ========================================================

            ConfigureCard(
                this.pendingCard,
                576,
                116,
                260,
                125);

            this.pendingCard.Controls.Add(
                this.lblPendingCaption);

            this.pendingCard.Controls.Add(
                this.lblPendingWithdrawal);

            ConfigureCaption(
                this.lblPendingCaption,
                "PENDING WITHDRAWAL",
                18,
                15);

            ConfigureMoneyLabel(
                this.lblPendingWithdrawal,
                "৳ 0.00",
                18,
                50);

            // ========================================================
            // LEDGER
            // ========================================================

            ConfigureCard(
                this.ledgerCard,
                850,
                116,
                260,
                125);

            this.ledgerCard.Controls.Add(
                this.lblLedgerCaption);

            this.ledgerCard.Controls.Add(
                this.lblLedgerBalance);

            ConfigureCaption(
                this.lblLedgerCaption,
                "LEDGER BALANCE",
                18,
                15);

            ConfigureMoneyLabel(
                this.lblLedgerBalance,
                "৳ 0.00",
                18,
                50);

            // ========================================================
            // TRANSACTION PANEL
            // ========================================================

            this.transactionPanel.Anchor =
                System.Windows.Forms.AnchorStyles.Top |
                System.Windows.Forms.AnchorStyles.Bottom |
                System.Windows.Forms.AnchorStyles.Left |
                System.Windows.Forms.AnchorStyles.Right;

            this.transactionPanel.BackColor =
                System.Drawing.Color.White;

            this.transactionPanel.Location =
                new System.Drawing.Point(28, 260);

            this.transactionPanel.Size =
                new System.Drawing.Size(1082, 385);

            this.transactionPanel.Padding =
                new System.Windows.Forms.Padding(20);

            this.transactionPanel.Controls.Add(
                this.dgvTransactions);

            this.transactionPanel.Controls.Add(
                this.lblEmptyTransactions);

            this.transactionPanel.Controls.Add(
                this.lblTransactionCount);

            this.transactionPanel.Controls.Add(
                this.lblTransactionTitle);

            // TITLE

            this.lblTransactionTitle.AutoSize =
                true;

            this.lblTransactionTitle.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    13F,
                    System.Drawing.FontStyle.Bold);

            this.lblTransactionTitle.ForeColor =
                System.Drawing.Color.FromArgb(
                    30, 41, 59);

            this.lblTransactionTitle.Location =
                new System.Drawing.Point(20, 18);

            this.lblTransactionTitle.Text =
                "Transaction History";

            // COUNT

            this.lblTransactionCount.Anchor =
                System.Windows.Forms.AnchorStyles.Top |
                System.Windows.Forms.AnchorStyles.Right;

            this.lblTransactionCount.AutoSize =
                true;

            this.lblTransactionCount.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    9F);

            this.lblTransactionCount.ForeColor =
                System.Drawing.Color.FromArgb(
                    100, 116, 139);

            this.lblTransactionCount.Location =
                new System.Drawing.Point(935, 21);

            this.lblTransactionCount.Text =
                "0 transactions";

            // EMPTY

            this.lblEmptyTransactions.AutoSize =
                true;

            this.lblEmptyTransactions.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10F);

            this.lblEmptyTransactions.ForeColor =
                System.Drawing.Color.FromArgb(
                    100, 116, 139);

            this.lblEmptyTransactions.Location =
                new System.Drawing.Point(410, 180);

            this.lblEmptyTransactions.Text =
                "No wallet transactions yet.";

            // ========================================================
            // DATA GRID
            // ========================================================

            this.dgvTransactions.Anchor =
                System.Windows.Forms.AnchorStyles.Top |
                System.Windows.Forms.AnchorStyles.Bottom |
                System.Windows.Forms.AnchorStyles.Left |
                System.Windows.Forms.AnchorStyles.Right;

            this.dgvTransactions.BackgroundColor =
                System.Drawing.Color.White;

            this.dgvTransactions.BorderStyle =
                System.Windows.Forms.BorderStyle.None;

            this.dgvTransactions.CellBorderStyle =
                System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;

            this.dgvTransactions.ColumnHeadersBorderStyle =
                System.Windows.Forms.DataGridViewHeaderBorderStyle.None;

            this.dgvTransactions.ColumnHeadersHeight =
                42;

            this.dgvTransactions.EnableHeadersVisualStyles =
                false;

            this.dgvTransactions.ColumnHeadersDefaultCellStyle =
                new System.Windows.Forms.DataGridViewCellStyle
                {
                    BackColor =
                        System.Drawing.Color.FromArgb(
                            248, 250, 252),

                    ForeColor =
                        System.Drawing.Color.FromArgb(
                            71, 85, 105),

                    Font =
                        new System.Drawing.Font(
                            "Segoe UI Semibold",
                            9F,
                            System.Drawing.FontStyle.Bold),

                    Alignment =
                        System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
                };

            this.dgvTransactions.DefaultCellStyle =
                new System.Windows.Forms.DataGridViewCellStyle
                {
                    BackColor =
                        System.Drawing.Color.White,

                    ForeColor =
                        System.Drawing.Color.FromArgb(
                            51, 65, 85),

                    Font =
                        new System.Drawing.Font(
                            "Segoe UI",
                            9F),

                    SelectionBackColor =
                        System.Drawing.Color.FromArgb(
                            239, 246, 255),

                    SelectionForeColor =
                        System.Drawing.Color.FromArgb(
                            30, 41, 59),

                    Padding =
                        new System.Windows.Forms.Padding(
                            5, 0, 5, 0)
                };

            this.dgvTransactions.GridColor =
                System.Drawing.Color.FromArgb(
                    226, 232, 240);

            this.dgvTransactions.Location =
                new System.Drawing.Point(20, 58);

            this.dgvTransactions.Size =
                new System.Drawing.Size(1042, 307);

            this.dgvTransactions.RowTemplate.Height =
                40;

            this.dgvTransactions.AllowUserToResizeRows =
                false;

            this.dgvTransactions.ReadOnly =
                true;

            this.dgvTransactions.SelectionMode =
                System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            // COLUMNS

            this.colDate.HeaderText =
                "DATE";

            this.colDate.Name =
                "colDate";

            this.colDate.Width =
                165;

            this.colType.HeaderText =
                "TYPE";

            this.colType.Name =
                "colType";

            this.colType.Width =
                110;

            this.colReference.HeaderText =
                "REFERENCE";

            this.colReference.Name =
                "colReference";

            this.colReference.Width =
                130;

            this.colDescription.HeaderText =
                "DESCRIPTION";

            this.colDescription.Name =
                "colDescription";

            this.colDescription.AutoSizeMode =
                System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;

            this.colAmount.HeaderText =
                "AMOUNT";

            this.colAmount.Name =
                "colAmount";

            this.colAmount.Width =
                130;

            this.colAmount.DefaultCellStyle =
                new System.Windows.Forms.DataGridViewCellStyle
                {
                    Alignment =
                        System.Windows.Forms.DataGridViewContentAlignment.MiddleRight,

                    Font =
                        new System.Drawing.Font(
                            "Segoe UI Semibold",
                            9F,
                            System.Drawing.FontStyle.Bold)
                };

            this.dgvTransactions.Columns.AddRange(
                new System.Windows.Forms.DataGridViewColumn[]
                {
                    this.colDate,
                    this.colType,
                    this.colReference,
                    this.colDescription,
                    this.colAmount
                });

            // ========================================================
            // LAST REFRESH
            // ========================================================

            this.lblLastRefresh.Anchor =
                System.Windows.Forms.AnchorStyles.Bottom |
                System.Windows.Forms.AnchorStyles.Left;

            this.lblLastRefresh.AutoSize =
                true;

            this.lblLastRefresh.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    8.5F);

            this.lblLastRefresh.ForeColor =
                System.Drawing.Color.FromArgb(
                    100, 116, 139);

            this.lblLastRefresh.Location =
                new System.Drawing.Point(30, 665);

            this.lblLastRefresh.Text =
                "Last updated: -";

            // ========================================================
            // ADD CONTROLS
            // ========================================================

            this.Controls.Add(
                this.transactionPanel);

            this.Controls.Add(
                this.ledgerCard);

            this.Controls.Add(
                this.pendingCard);

            this.Controls.Add(
                this.earnedCard);

            this.Controls.Add(
                this.availableCard);

            this.Controls.Add(
                this.lblLastRefresh);

            this.Controls.Add(
                this.headerPanel);

            this.ResumeLayout(false);
        }


        // ============================================================
        // DESIGN HELPERS
        // ============================================================

        private void ConfigureCard(
            System.Windows.Forms.Panel panel,
            int x,
            int y,
            int width,
            int height)
        {
            panel.BackColor =
                System.Drawing.Color.White;

            panel.Location =
                new System.Drawing.Point(x, y);

            panel.Size =
                new System.Drawing.Size(
                    width,
                    height);

            panel.BorderStyle =
                System.Windows.Forms.BorderStyle.FixedSingle;
        }


        private void ConfigureCaption(
            System.Windows.Forms.Label label,
            string text,
            int x,
            int y)
        {
            label.AutoSize = true;

            label.Font =
                new System.Drawing.Font(
                    "Segoe UI Semibold",
                    8.5F,
                    System.Drawing.FontStyle.Bold);

            label.ForeColor =
                System.Drawing.Color.FromArgb(
                    100, 116, 139);

            label.Location =
                new System.Drawing.Point(x, y);

            label.Text = text;
        }


        private void ConfigureMoneyLabel(
            System.Windows.Forms.Label label,
            string text,
            int x,
            int y)
        {
            label.AutoSize = true;

            label.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    18F,
                    System.Drawing.FontStyle.Bold);

            label.ForeColor =
                System.Drawing.Color.FromArgb(
                    30, 41, 59);

            label.Location =
                new System.Drawing.Point(x, y);

            label.Text = text;
        }


        private void ConfigureSmallLabel(
            System.Windows.Forms.Label label,
            string text,
            int x,
            int y)
        {
            label.AutoSize = true;

            label.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    8.5F);

            label.ForeColor =
                System.Drawing.Color.FromArgb(
                    100, 116, 139);

            label.Location =
                new System.Drawing.Point(x, y);

            label.Text = text;
        }
    }
}