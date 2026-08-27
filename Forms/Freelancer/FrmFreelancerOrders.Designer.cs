namespace SkillHub.Forms.Freelancer
{
    partial class FrmFreelancerOrders
    {
        private System.ComponentModel.IContainer components = null;

        // ============================================================
        // HEADER
        // ============================================================

        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label lblSubtitle;

        // ============================================================
        // STATISTICS
        // ============================================================

        private System.Windows.Forms.Panel statsPanel;

        private System.Windows.Forms.Panel totalCard;
        private System.Windows.Forms.Label lblTotalCaption;
        private System.Windows.Forms.Label lblTotalValue;

        private System.Windows.Forms.Panel placedCard;
        private System.Windows.Forms.Label lblPlacedCaption;
        private System.Windows.Forms.Label lblPlacedValue;

        private System.Windows.Forms.Panel progressCard;
        private System.Windows.Forms.Label lblProgressCaption;
        private System.Windows.Forms.Label lblProgressValue;

        private System.Windows.Forms.Panel deliveredCard;
        private System.Windows.Forms.Label lblDeliveredCaption;
        private System.Windows.Forms.Label lblDeliveredValue;

        // ============================================================
        // TOOLBAR
        // ============================================================

        private System.Windows.Forms.Panel toolbarPanel;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Button btnRefresh;

        // ============================================================
        // ORDERS LIST
        // ============================================================

        private System.Windows.Forms.Panel ordersPanel;
        private System.Windows.Forms.Label lblOrdersTitle;
        private System.Windows.Forms.DataGridView dgvOrders;

        // ============================================================
        // DETAILS
        // ============================================================

        private System.Windows.Forms.Panel detailsPanel;
        private System.Windows.Forms.Panel detailsHeaderPanel;

        private System.Windows.Forms.Label lblOrderNumber;
        private System.Windows.Forms.Label lblStatusBadge;

        private System.Windows.Forms.Panel emptyDetailsPanel;
        private System.Windows.Forms.Label lblEmptyIcon;
        private System.Windows.Forms.Label lblEmptyTitle;
        private System.Windows.Forms.Label lblEmptySubtitle;

        private System.Windows.Forms.Label lblClientCaption;
        private System.Windows.Forms.Label lblClientValue;

        private System.Windows.Forms.Label lblServiceCaption;
        private System.Windows.Forms.Label lblServiceValue;

        private System.Windows.Forms.Label lblQuantityCaption;
        private System.Windows.Forms.Label lblQuantityValue;

        private System.Windows.Forms.Label lblUnitPriceCaption;
        private System.Windows.Forms.Label lblUnitPriceValue;

        private System.Windows.Forms.Label lblDiscountCaption;
        private System.Windows.Forms.Label lblDiscountValue;

        private System.Windows.Forms.Label lblGrossCaption;
        private System.Windows.Forms.Label lblGrossValue;

        private System.Windows.Forms.Label lblEarningCaption;
        private System.Windows.Forms.Label lblEarningValue;

        private System.Windows.Forms.Label lblCreatedCaption;
        private System.Windows.Forms.Label lblCreatedValue;

        private System.Windows.Forms.Label lblAcceptedCaption;
        private System.Windows.Forms.Label lblAcceptedValue;

        

        private System.Windows.Forms.Label lblCompletedCaption;
        private System.Windows.Forms.Label lblCompletedValue;

        private System.Windows.Forms.Label lblDeliveryNoteCaption;
        private System.Windows.Forms.Label lblDeliveryNoteValue;

        // ============================================================
        // ACTION BUTTONS
        // ============================================================

        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.Button btnDeliver;

        // ============================================================
        // DISPOSE
        // ============================================================

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        // ============================================================
        // INITIALIZE COMPONENT
        // ============================================================

        private void InitializeComponent()
        {
            this.components =
                new System.ComponentModel.Container();

            // ========================================================
            // COLORS
            // ========================================================

            System.Drawing.Color pageBack =
                System.Drawing.Color.FromArgb(246, 248, 252);

            System.Drawing.Color white =
                System.Drawing.Color.White;

            System.Drawing.Color primary =
                System.Drawing.Color.FromArgb(31, 91, 255);

            System.Drawing.Color darkText =
                System.Drawing.Color.FromArgb(27, 38, 55);

            System.Drawing.Color mutedText =
                System.Drawing.Color.FromArgb(112, 124, 141);

            System.Drawing.Color lightBorder =
                System.Drawing.Color.FromArgb(226, 231, 238);

            System.Drawing.Color headerBack =
                System.Drawing.Color.FromArgb(249, 250, 252);

            System.Drawing.Color softBlue =
                System.Drawing.Color.FromArgb(235, 241, 255);

            // ========================================================
            // FORM
            // ========================================================

            this.SuspendLayout();

            this.AutoScaleDimensions =
                new System.Drawing.SizeF(7F, 15F);

            this.AutoScaleMode =
                System.Windows.Forms.AutoScaleMode.Font;

            this.BackColor = pageBack;

            this.ClientSize =
                new System.Drawing.Size(1380, 820);

            this.MinimumSize =
                new System.Drawing.Size(1100, 680);

            this.StartPosition =
                System.Windows.Forms.FormStartPosition.CenterScreen;

            this.Text =
                "SkillHub | Freelancer Orders";

            this.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    9F);

            // ========================================================
            // HEADER
            // ========================================================

            this.headerPanel =
                new System.Windows.Forms.Panel();

            this.headerPanel.Dock =
                System.Windows.Forms.DockStyle.Top;

            this.headerPanel.Height = 88;

            this.headerPanel.BackColor = white;

            this.lblHeader =
                new System.Windows.Forms.Label();

            this.lblHeader.AutoSize = true;

            this.lblHeader.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    20F,
                    System.Drawing.FontStyle.Bold);

            this.lblHeader.ForeColor = darkText;

            this.lblHeader.Location =
                new System.Drawing.Point(
                    30,
                    14);

            this.lblHeader.Text = "Orders";

            this.lblSubtitle =
                new System.Windows.Forms.Label();

            this.lblSubtitle.AutoSize = true;

            this.lblSubtitle.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    9.5F);

            this.lblSubtitle.ForeColor = mutedText;

            this.lblSubtitle.Location =
                new System.Drawing.Point(
                    32,
                    51);

            this.lblSubtitle.Text =
                "Manage your client orders, track progress and deliver completed work.";

            this.headerPanel.Controls.Add(
                this.lblSubtitle);

            this.headerPanel.Controls.Add(
                this.lblHeader);

            // ========================================================
            // STATISTICS PANEL
            // ========================================================

            this.statsPanel =
                new System.Windows.Forms.Panel();

            this.statsPanel.Dock =
                System.Windows.Forms.DockStyle.Top;

            this.statsPanel.Height = 112;

            this.statsPanel.BackColor = pageBack;

            System.Windows.Forms.FlowLayoutPanel statsFlow =
                new System.Windows.Forms.FlowLayoutPanel();

            statsFlow.Dock =
                System.Windows.Forms.DockStyle.Fill;

            statsFlow.Padding =
                new System.Windows.Forms.Padding(
                    30,
                    12,
                    30,
                    12);

            statsFlow.Margin =
                new System.Windows.Forms.Padding(0);

            statsFlow.WrapContents = false;

            statsFlow.FlowDirection =
                System.Windows.Forms.FlowDirection.LeftToRight;

            statsFlow.BackColor = pageBack;

            // ========================================================
            // STAT CARDS
            // ========================================================

            this.totalCard = CreateStatCard(
                "TOTAL ORDERS",
                out this.lblTotalCaption,
                out this.lblTotalValue);

            this.placedCard = CreateStatCard(
                "PLACED",
                out this.lblPlacedCaption,
                out this.lblPlacedValue);

            this.progressCard = CreateStatCard(
                "IN PROGRESS",
                out this.lblProgressCaption,
                out this.lblProgressValue);

            this.deliveredCard = CreateStatCard(
                "DELIVERED",
                out this.lblDeliveredCaption,
                out this.lblDeliveredValue);

            this.lblTotalValue.Text = "0";
            this.lblPlacedValue.Text = "0";
            this.lblProgressValue.Text = "0";
            this.lblDeliveredValue.Text = "0";

            statsFlow.Controls.Add(this.totalCard);
            statsFlow.Controls.Add(this.placedCard);
            statsFlow.Controls.Add(this.progressCard);
            statsFlow.Controls.Add(this.deliveredCard);

            this.statsPanel.Controls.Add(statsFlow);

            // ========================================================
            // TOOLBAR
            // ========================================================

            this.toolbarPanel =
                new System.Windows.Forms.Panel();

            this.toolbarPanel.Dock =
                System.Windows.Forms.DockStyle.Top;

            this.toolbarPanel.Height = 64;

            this.toolbarPanel.BackColor = white;

            // ========================================================
            // SEARCH BOX
            // ========================================================

            this.txtSearch =
                new System.Windows.Forms.TextBox();

            this.txtSearch.Name =
                "txtSearch";

            this.txtSearch.Location =
                new System.Drawing.Point(
                    30,
                    15);

            this.txtSearch.Size =
                new System.Drawing.Size(
                    380,
                    32);

            this.txtSearch.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    9.5F);

            this.txtSearch.BorderStyle =
                System.Windows.Forms.BorderStyle.FixedSingle;

            this.txtSearch.BackColor = white;

            this.txtSearch.ForeColor =
                System.Drawing.Color.FromArgb(
                    140,
                    150,
                    165);

            this.txtSearch.Text =
                "Search orders...";

            this.txtSearch.GotFocus +=
                new System.EventHandler(
                    this.txtSearch_GotFocus);

            this.txtSearch.LostFocus +=
                new System.EventHandler(
                    this.txtSearch_LostFocus);

            this.txtSearch.TextChanged +=
                new System.EventHandler(
                    this.txtSearch_TextChanged);

            // ========================================================
            // STATUS FILTER
            // ========================================================

            this.cmbStatus =
                new System.Windows.Forms.ComboBox();

            this.cmbStatus.Name =
                "cmbStatus";

            this.cmbStatus.DropDownStyle =
                System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.cmbStatus.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    9F);

            this.cmbStatus.Location =
                new System.Drawing.Point(
                    425,
                    15);

            this.cmbStatus.Size =
                new System.Drawing.Size(
                    180,
                    32);

            this.cmbStatus.BackColor = white;

            this.cmbStatus.Items.AddRange(
                new object[]
                {
                    "All Statuses",
                    "Placed",
                    "In Progress",
                    "Delivered",
                    "Completed"
                });

            this.cmbStatus.SelectedIndex = 0;

            this.cmbStatus.SelectedIndexChanged +=
                new System.EventHandler(
                    this.cmbStatus_SelectedIndexChanged);

            // ========================================================
            // REFRESH BUTTON
            // ========================================================

            this.btnRefresh =
                new System.Windows.Forms.Button();

            this.btnRefresh.Name =
                "btnRefresh";

            this.btnRefresh.Text =
                "↻  Refresh";

            this.btnRefresh.Location =
                new System.Drawing.Point(
                    620,
                    13);

            this.btnRefresh.Size =
                new System.Drawing.Size(
                    120,
                    36);

            this.btnRefresh.FlatStyle =
                System.Windows.Forms.FlatStyle.Flat;

            this.btnRefresh.FlatAppearance.BorderSize = 1;

            this.btnRefresh.FlatAppearance.BorderColor =
                lightBorder;

            this.btnRefresh.BackColor = white;

            this.btnRefresh.ForeColor =
                System.Drawing.Color.FromArgb(
                    65,
                    77,
                    94);

            this.btnRefresh.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    9F,
                    System.Drawing.FontStyle.Bold);

            this.btnRefresh.Cursor =
                System.Windows.Forms.Cursors.Hand;

            this.btnRefresh.Click +=
                new System.EventHandler(
                    this.btnRefresh_Click);

            this.toolbarPanel.Controls.Add(
                this.btnRefresh);

            this.toolbarPanel.Controls.Add(
                this.cmbStatus);

            this.toolbarPanel.Controls.Add(
                this.txtSearch);

            // ========================================================
            // DETAILS PANEL
            // ========================================================

            this.detailsPanel =
                new System.Windows.Forms.Panel();

            this.detailsPanel.Dock =
                System.Windows.Forms.DockStyle.Right;

            this.detailsPanel.Width = 440;

            this.detailsPanel.BackColor = white;

            this.detailsPanel.Padding =
                new System.Windows.Forms.Padding(
                    24,
                    18,
                    24,
                    18);

            // ========================================================
            // DETAILS HEADER
            // ========================================================

            this.detailsHeaderPanel =
                new System.Windows.Forms.Panel();

            this.detailsHeaderPanel.Dock =
                System.Windows.Forms.DockStyle.Top;

            this.detailsHeaderPanel.Height = 60;

            this.detailsHeaderPanel.BackColor = white;

            this.lblOrderNumber =
                new System.Windows.Forms.Label();

            this.lblOrderNumber.AutoSize = true;

            this.lblOrderNumber.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    15F,
                    System.Drawing.FontStyle.Bold);

            this.lblOrderNumber.ForeColor = darkText;

            this.lblOrderNumber.Location =
                new System.Drawing.Point(
                    0,
                    4);

            this.lblOrderNumber.Text =
                "#ORD-0000";

            this.lblStatusBadge =
                new System.Windows.Forms.Label();

            this.lblStatusBadge.AutoSize = false;

            this.lblStatusBadge.TextAlign =
                System.Drawing.ContentAlignment.MiddleCenter;

            this.lblStatusBadge.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    7.5F,
                    System.Drawing.FontStyle.Bold);

            this.lblStatusBadge.Anchor =
                System.Windows.Forms.AnchorStyles.Top |
                System.Windows.Forms.AnchorStyles.Right;

            this.lblStatusBadge.Location =
                new System.Drawing.Point(
                    255,
                    3);

            this.lblStatusBadge.Size =
                new System.Drawing.Size(
                    137,
                    28);

            this.lblStatusBadge.BackColor =
                System.Drawing.Color.FromArgb(
                    239,
                    242,
                    246);

            this.lblStatusBadge.ForeColor =
                System.Drawing.Color.FromArgb(
                    105,
                    115,
                    130);

            this.lblStatusBadge.Text =
                "NO ORDER";

            this.detailsHeaderPanel.Controls.Add(
                this.lblStatusBadge);

            this.detailsHeaderPanel.Controls.Add(
                this.lblOrderNumber);

            // ========================================================
            // DETAIL CONTENT PANEL
            // ========================================================

            System.Windows.Forms.Panel detailContent =
                new System.Windows.Forms.Panel();

            detailContent.Dock =
                System.Windows.Forms.DockStyle.Fill;

            detailContent.BackColor = white;

            detailContent.AutoScroll = true;

            // ========================================================
            // CLIENT
            // ========================================================

            CreateDetailLabel(
                detailContent,
                "CLIENT",
                0,
                8,
                385,
                out this.lblClientCaption,
                out this.lblClientValue);

            // ========================================================
            // SERVICE
            // ========================================================

            CreateDetailLabel(
                detailContent,
                "SERVICE",
                0,
                70,
                385,
                out this.lblServiceCaption,
                out this.lblServiceValue);

            // ========================================================
            // FINANCIAL INFORMATION
            // ========================================================

            System.Windows.Forms.Label financialTitle =
                new System.Windows.Forms.Label();

            financialTitle.AutoSize = true;

            financialTitle.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    8F,
                    System.Drawing.FontStyle.Bold);

            financialTitle.ForeColor =
                primary;

            financialTitle.Location =
                new System.Drawing.Point(
                    0,
                    132);

            financialTitle.Text =
                "ORDER SUMMARY";

            detailContent.Controls.Add(
                financialTitle);

            // Quantity
            CreateDetailLabel(
                detailContent,
                "QUANTITY",
                0,
                160,
                175,
                out this.lblQuantityCaption,
                out this.lblQuantityValue);

            // Unit Price
            CreateDetailLabel(
                detailContent,
                "UNIT PRICE",
                195,
                160,
                190,
                out this.lblUnitPriceCaption,
                out this.lblUnitPriceValue);

            // Discount
            CreateDetailLabel(
                detailContent,
                "DISCOUNT",
                0,
                222,
                175,
                out this.lblDiscountCaption,
                out this.lblDiscountValue);

            // Gross
            CreateDetailLabel(
                detailContent,
                "GROSS AMOUNT",
                195,
                222,
                190,
                out this.lblGrossCaption,
                out this.lblGrossValue);

            // Earning
            CreateDetailLabel(
                detailContent,
                "YOUR EARNING",
                0,
                284,
                385,
                out this.lblEarningCaption,
                out this.lblEarningValue);

            this.lblEarningValue.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    11F,
                    System.Drawing.FontStyle.Bold);

            this.lblEarningValue.ForeColor = primary;

            // ========================================================
            // TIMELINE
            // ========================================================

            System.Windows.Forms.Label timelineTitle =
                new System.Windows.Forms.Label();

            timelineTitle.AutoSize = true;

            timelineTitle.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    8F,
                    System.Drawing.FontStyle.Bold);

            timelineTitle.ForeColor = primary;

            timelineTitle.Location =
                new System.Drawing.Point(
                    0,
                    346);

            timelineTitle.Text =
                "ORDER TIMELINE";

            detailContent.Controls.Add(
                timelineTitle);

            // Created
            CreateDetailLabel(
                detailContent,
                "CREATED",
                0,
                374,
                175,
                out this.lblCreatedCaption,
                out this.lblCreatedValue);

            // Accepted
            CreateDetailLabel(
                detailContent,
                "ACCEPTED",
                195,
                374,
                190,
                out this.lblAcceptedCaption,
                out this.lblAcceptedValue);

            // Delivered
            CreateDetailLabel(
                detailContent,
                "DELIVERED",
                0,
                436,
                175,
                out this.lblDeliveredCaption,
                out this.lblDeliveredValue);

            // Completed
            CreateDetailLabel(
                detailContent,
                "COMPLETED",
                195,
                436,
                190,
                out this.lblCompletedCaption,
                out this.lblCompletedValue);

            // ========================================================
            // DELIVERY NOTE
            // ========================================================

            CreateDetailLabel(
                detailContent,
                "DELIVERY NOTE",
                0,
                498,
                385,
                out this.lblDeliveryNoteCaption,
                out this.lblDeliveryNoteValue);

            this.lblDeliveryNoteValue.Height = 55;

            this.lblDeliveryNoteValue.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    8.5F);

            this.lblDeliveryNoteValue.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    8.5F);

            this.lblDeliveryNoteValue.AutoEllipsis = true;

            // ========================================================
            // ACTION PANEL
            // ========================================================

            System.Windows.Forms.Panel actionPanel =
                new System.Windows.Forms.Panel();

            actionPanel.Dock =
                System.Windows.Forms.DockStyle.Bottom;

            actionPanel.Height = 64;

            actionPanel.BackColor = white;

            // ========================================================
            // ACCEPT BUTTON
            // ========================================================

            this.btnAccept =
                new System.Windows.Forms.Button();

            this.btnAccept.Name =
                "btnAccept";

            this.btnAccept.Text =
                "Accept Order";

            this.btnAccept.Location =
                new System.Drawing.Point(
                    0,
                    12);

            this.btnAccept.Size =
                new System.Drawing.Size(
                    185,
                    42);

            this.btnAccept.FlatStyle =
                System.Windows.Forms.FlatStyle.Flat;

            this.btnAccept.FlatAppearance.BorderSize = 0;

            this.btnAccept.BackColor =
                primary;

            this.btnAccept.ForeColor =
                white;

            this.btnAccept.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    9F,
                    System.Drawing.FontStyle.Bold);

            this.btnAccept.Cursor =
                System.Windows.Forms.Cursors.Hand;

            this.btnAccept.Click +=
                new System.EventHandler(
                    this.btnAccept_Click);

            // ========================================================
            // DELIVER BUTTON
            // ========================================================

            this.btnDeliver =
                new System.Windows.Forms.Button();

            this.btnDeliver.Name =
                "btnDeliver";

            this.btnDeliver.Text =
                "Deliver Order";

            this.btnDeliver.Location =
                new System.Drawing.Point(
                    195,
                    12);

            this.btnDeliver.Size =
                new System.Drawing.Size(
                    197,
                    42);

            this.btnDeliver.FlatStyle =
                System.Windows.Forms.FlatStyle.Flat;

            this.btnDeliver.FlatAppearance.BorderSize = 1;

            this.btnDeliver.FlatAppearance.BorderColor =
                primary;

            this.btnDeliver.BackColor =
                softBlue;

            this.btnDeliver.ForeColor =
                primary;

            this.btnDeliver.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    9F,
                    System.Drawing.FontStyle.Bold);

            this.btnDeliver.Cursor =
                System.Windows.Forms.Cursors.Hand;

            this.btnDeliver.Click +=
                new System.EventHandler(
                    this.btnDeliver_Click);

            actionPanel.Controls.Add(
                this.btnDeliver);

            actionPanel.Controls.Add(
                this.btnAccept);

            // ========================================================
            // EMPTY STATE
            // ========================================================

            this.emptyDetailsPanel =
                new System.Windows.Forms.Panel();

            this.emptyDetailsPanel.Dock =
                System.Windows.Forms.DockStyle.Fill;

            this.emptyDetailsPanel.BackColor = white;

            this.lblEmptyIcon =
                new System.Windows.Forms.Label();

            this.lblEmptyIcon.AutoSize = false;

            this.lblEmptyIcon.TextAlign =
                System.Drawing.ContentAlignment.MiddleCenter;

            this.lblEmptyIcon.Font =
                new System.Drawing.Font(
                    "Segoe UI Symbol",
                    32F);

            this.lblEmptyIcon.ForeColor = primary;

            this.lblEmptyIcon.Location =
                new System.Drawing.Point(
                    0,
                    125);

            this.lblEmptyIcon.Size =
                new System.Drawing.Size(
                    392,
                    55);

            this.lblEmptyIcon.Text =
                "◎";

            this.lblEmptyTitle =
                new System.Windows.Forms.Label();

            this.lblEmptyTitle.AutoSize = false;

            this.lblEmptyTitle.TextAlign =
                System.Drawing.ContentAlignment.MiddleCenter;

            this.lblEmptyTitle.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    11F,
                    System.Drawing.FontStyle.Bold);

            this.lblEmptyTitle.ForeColor = darkText;

            this.lblEmptyTitle.Location =
                new System.Drawing.Point(
                    0,
                    190);

            this.lblEmptyTitle.Size =
                new System.Drawing.Size(
                    392,
                    34);

            this.lblEmptyTitle.Text =
                "Select an order";

            this.lblEmptySubtitle =
                new System.Windows.Forms.Label();

            this.lblEmptySubtitle.AutoSize = false;

            this.lblEmptySubtitle.TextAlign =
                System.Drawing.ContentAlignment.TopCenter;

            this.lblEmptySubtitle.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    8.5F);

            this.lblEmptySubtitle.ForeColor = mutedText;

            this.lblEmptySubtitle.Location =
                new System.Drawing.Point(
                    25,
                    228);

            this.lblEmptySubtitle.Size =
                new System.Drawing.Size(
                    342,
                    55);

            this.lblEmptySubtitle.Text =
                "Choose an order from the list to view its details.";

            this.emptyDetailsPanel.Controls.Add(
                this.lblEmptySubtitle);

            this.emptyDetailsPanel.Controls.Add(
                this.lblEmptyTitle);

            this.emptyDetailsPanel.Controls.Add(
                this.lblEmptyIcon);

            // ========================================================
            // ADD DETAILS CONTENT
            // ========================================================

            this.detailsPanel.Controls.Add(
                this.emptyDetailsPanel);

            this.detailsPanel.Controls.Add(
                detailContent);

            this.detailsPanel.Controls.Add(
                actionPanel);

            this.detailsPanel.Controls.Add(
                this.detailsHeaderPanel);

            // ========================================================
            // ORDERS PANEL
            // ========================================================

            this.ordersPanel =
                new System.Windows.Forms.Panel();

            this.ordersPanel.Dock =
                System.Windows.Forms.DockStyle.Fill;

            this.ordersPanel.BackColor = white;

            this.ordersPanel.Padding =
                new System.Windows.Forms.Padding(
                    25,
                    16,
                    20,
                    20);

            // ========================================================
            // ORDERS TITLE
            // ========================================================

            this.lblOrdersTitle =
                new System.Windows.Forms.Label();

            this.lblOrdersTitle.AutoSize = true;

            this.lblOrdersTitle.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    12F,
                    System.Drawing.FontStyle.Bold);

            this.lblOrdersTitle.ForeColor = darkText;

            this.lblOrdersTitle.Location =
                new System.Drawing.Point(
                    25,
                    14);

            this.lblOrdersTitle.Text =
                "Your Orders";

            // ========================================================
            // DATA GRID
            // ========================================================

            this.dgvOrders =
                new System.Windows.Forms.DataGridView();

            this.dgvOrders.Name =
                "dgvOrders";

            this.dgvOrders.Anchor =
                System.Windows.Forms.AnchorStyles.Top |
                System.Windows.Forms.AnchorStyles.Bottom |
                System.Windows.Forms.AnchorStyles.Left |
                System.Windows.Forms.AnchorStyles.Right;

            this.dgvOrders.Location =
                new System.Drawing.Point(
                    25,
                    52);

            this.dgvOrders.Size =
                new System.Drawing.Size(
                    780,
                    620);

            this.dgvOrders.BackgroundColor = white;

            this.dgvOrders.BorderStyle =
                System.Windows.Forms.BorderStyle.None;

            this.dgvOrders.CellBorderStyle =
                System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;

            this.dgvOrders.GridColor =
                lightBorder;

            this.dgvOrders.RowHeadersVisible = false;

            this.dgvOrders.AllowUserToAddRows = false;

            this.dgvOrders.AllowUserToDeleteRows = false;

            this.dgvOrders.AllowUserToResizeRows = false;

            this.dgvOrders.AllowUserToResizeColumns = false;

            this.dgvOrders.ReadOnly = true;

            this.dgvOrders.MultiSelect = false;

            this.dgvOrders.SelectionMode =
                System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            this.dgvOrders.AutoGenerateColumns = false;

            this.dgvOrders.RowTemplate.Height = 48;

            this.dgvOrders.EnableHeadersVisualStyles = false;

            this.dgvOrders.ColumnHeadersHeight = 44;

            this.dgvOrders.ColumnHeadersBorderStyle =
                System.Windows.Forms.DataGridViewHeaderBorderStyle.None;

            this.dgvOrders.ColumnHeadersDefaultCellStyle =
                new System.Windows.Forms.DataGridViewCellStyle
                {
                    BackColor =
                        headerBack,

                    ForeColor =
                        System.Drawing.Color.FromArgb(
                            92,
                            104,
                            120),

                    Font =
                        new System.Drawing.Font(
                            "Segoe UI",
                            8F,
                            System.Drawing.FontStyle.Bold),

                    Alignment =
                        System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft,

                    SelectionBackColor =
                        headerBack,

                    SelectionForeColor =
                        System.Drawing.Color.FromArgb(
                            92,
                            104,
                            120),

                    Padding =
                        new System.Windows.Forms.Padding(
                            10,
                            0,
                            10,
                            0)
                };

            this.dgvOrders.DefaultCellStyle =
                new System.Windows.Forms.DataGridViewCellStyle
                {
                    BackColor = white,

                    ForeColor =
                        System.Drawing.Color.FromArgb(
                            48,
                            61,
                            79),

                    Font =
                        new System.Drawing.Font(
                            "Segoe UI",
                            9F),

                    SelectionBackColor =
                        softBlue,

                    SelectionForeColor =
                        System.Drawing.Color.FromArgb(
                            27,
                            45,
                            75),

                    Padding =
                        new System.Windows.Forms.Padding(
                            10,
                            0,
                            10,
                            0),

                    Alignment =
                        System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
                };

            this.dgvOrders.CellFormatting +=
                new System.Windows.Forms.DataGridViewCellFormattingEventHandler(
                    this.dgvOrders_CellFormatting);

            this.dgvOrders.SelectionChanged +=
                new System.EventHandler(
                    this.dgvOrders_SelectionChanged);

            this.ordersPanel.Controls.Add(
                this.dgvOrders);

            this.ordersPanel.Controls.Add(
                this.lblOrdersTitle);

            // ========================================================
            // FORM CONTROLS
            // ========================================================

            this.Controls.Add(
                this.ordersPanel);

            this.Controls.Add(
                this.detailsPanel);

            this.Controls.Add(
                this.toolbarPanel);

            this.Controls.Add(
                this.statsPanel);

            this.Controls.Add(
                this.headerPanel);

            // ========================================================
            // FINAL
            // ========================================================

            this.ResumeLayout(false);
        }

        // ============================================================
        // CREATE STAT CARD
        // ============================================================

        private System.Windows.Forms.Panel CreateStatCard(
            string caption,
            out System.Windows.Forms.Label captionLabel,
            out System.Windows.Forms.Label valueLabel)
        {
            System.Windows.Forms.Panel card =
                new System.Windows.Forms.Panel();

            card.Size =
                new System.Drawing.Size(
                    250,
                    86);

            card.Margin =
                new System.Windows.Forms.Padding(
                    0,
                    0,
                    16,
                    0);

            card.BackColor =
                System.Drawing.Color.White;

            card.BorderStyle =
                System.Windows.Forms.BorderStyle.FixedSingle;

            captionLabel =
                new System.Windows.Forms.Label();

            captionLabel.AutoSize = true;

            captionLabel.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    7.5F,
                    System.Drawing.FontStyle.Bold);

            captionLabel.ForeColor =
                System.Drawing.Color.FromArgb(
                    125,
                    137,
                    153);

            captionLabel.Location =
                new System.Drawing.Point(
                    18,
                    12);

            captionLabel.Text =
                caption;

            valueLabel =
                new System.Windows.Forms.Label();

            valueLabel.AutoSize = true;

            valueLabel.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    20F,
                    System.Drawing.FontStyle.Bold);

            valueLabel.ForeColor =
                System.Drawing.Color.FromArgb(
                    27,
                    38,
                    55);

            valueLabel.Location =
                new System.Drawing.Point(
                    18,
                    34);

            valueLabel.Text =
                "0";

            card.Controls.Add(
                valueLabel);

            card.Controls.Add(
                captionLabel);

            return card;
        }

        // ============================================================
        // CREATE DETAIL FIELD
        // ============================================================

        private void CreateDetailLabel(
            System.Windows.Forms.Control parent,
            string caption,
            int x,
            int y,
            int width,
            out System.Windows.Forms.Label captionLabel,
            out System.Windows.Forms.Label valueLabel)
        {
            captionLabel =
                new System.Windows.Forms.Label();

            captionLabel.AutoSize = true;

            captionLabel.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    7.5F,
                    System.Drawing.FontStyle.Bold);

            captionLabel.ForeColor =
                System.Drawing.Color.FromArgb(
                    125,
                    137,
                    153);

            captionLabel.Location =
                new System.Drawing.Point(
                    x,
                    y);

            captionLabel.Text =
                caption;

            valueLabel =
                new System.Windows.Forms.Label();

            valueLabel.AutoSize = false;

            valueLabel.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    9F,
                    System.Drawing.FontStyle.Bold);

            valueLabel.ForeColor =
                System.Drawing.Color.FromArgb(
                    43,
                    55,
                    72);

            valueLabel.Location =
                new System.Drawing.Point(
                    x,
                    y + 18);

            valueLabel.Size =
                new System.Drawing.Size(
                    width,
                    30);

            valueLabel.Text =
                "—";

            valueLabel.AutoEllipsis = true;

            parent.Controls.Add(
                valueLabel);

            parent.Controls.Add(
                captionLabel);
        }
    }
}