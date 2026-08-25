namespace SkillHub.Forms.Freelancer
{
    partial class FrmManageServices
    {
        private System.ComponentModel.IContainer components = null;

        // ============================================================
        // HEADER
        // ============================================================

        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label lblSubtitle;

        // ============================================================
        // EDITOR
        // ============================================================

        private System.Windows.Forms.Panel editorPanel;
        private System.Windows.Forms.Label lblEditorTitle;
        private System.Windows.Forms.Label lblMode;

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtTitle;

        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.ComboBox cmbCategory;

        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;

        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.NumericUpDown nudPrice;

        private System.Windows.Forms.Label lblDeliveryDays;
        private System.Windows.Forms.NumericUpDown nudDeliveryDays;

        private System.Windows.Forms.Label lblAvailableSlots;
        private System.Windows.Forms.NumericUpDown nudAvailableSlots;

        private System.Windows.Forms.CheckBox chkActive;

        private System.Windows.Forms.Label lblSelectedService;

        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnToggleStatus;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClear;

        // ============================================================
        // SERVICES
        // ============================================================

        private System.Windows.Forms.Panel servicesPanel;
        private System.Windows.Forms.Label lblServicesTitle;
        private System.Windows.Forms.Label lblServiceCount;
        private System.Windows.Forms.Button btnRefresh;

        private System.Windows.Forms.DataGridView dgvServices;

        private System.Windows.Forms.DataGridViewTextBoxColumn colServiceId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDelivery;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSlots;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colStatus;

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
            this.components = new System.ComponentModel.Container();

            this.headerPanel = new System.Windows.Forms.Panel();
            this.lblHeader = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();

            this.editorPanel = new System.Windows.Forms.Panel();
            this.lblEditorTitle = new System.Windows.Forms.Label();
            this.lblMode = new System.Windows.Forms.Label();

            this.lblTitle = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();

            this.lblCategory = new System.Windows.Forms.Label();
            this.cmbCategory = new System.Windows.Forms.ComboBox();

            this.lblDescription = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();

            this.lblPrice = new System.Windows.Forms.Label();
            this.nudPrice = new System.Windows.Forms.NumericUpDown();

            this.lblDeliveryDays = new System.Windows.Forms.Label();
            this.nudDeliveryDays = new System.Windows.Forms.NumericUpDown();

            this.lblAvailableSlots = new System.Windows.Forms.Label();
            this.nudAvailableSlots = new System.Windows.Forms.NumericUpDown();

            this.chkActive = new System.Windows.Forms.CheckBox();
            this.lblSelectedService = new System.Windows.Forms.Label();

            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnToggleStatus = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();

            this.servicesPanel = new System.Windows.Forms.Panel();
            this.lblServicesTitle = new System.Windows.Forms.Label();
            this.lblServiceCount = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();

            this.dgvServices = new System.Windows.Forms.DataGridView();

            this.colServiceId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDelivery = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSlots = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewCheckBoxColumn();

            ((System.ComponentModel.ISupportInitialize)(this.nudPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDeliveryDays)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAvailableSlots)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvServices)).BeginInit();

            this.SuspendLayout();

            // ============================================================
            // FORM
            // ============================================================

            this.AutoScaleDimensions =
                new System.Drawing.SizeF(8F, 16F);

            this.AutoScaleMode =
                System.Windows.Forms.AutoScaleMode.Font;

            this.BackColor =
                System.Drawing.Color.FromArgb(244, 247, 251);

            this.ClientSize =
                new System.Drawing.Size(1200, 800);

            this.MinimumSize =
                new System.Drawing.Size(1080, 720);

            this.StartPosition =
                System.Windows.Forms.FormStartPosition.CenterParent;

            this.Text =
                "SkillHub | Manage Services";

            this.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    9F,
                    System.Drawing.FontStyle.Regular);

            // ============================================================
            // HEADER
            // ============================================================

            this.headerPanel.BackColor =
                System.Drawing.Color.FromArgb(15, 23, 42);

            this.headerPanel.Dock =
                System.Windows.Forms.DockStyle.Top;

            this.headerPanel.Height = 100;

            this.headerPanel.Padding =
                new System.Windows.Forms.Padding(
                    30,
                    0,
                    30,
                    0);

            this.headerPanel.Controls.Add(this.lblSubtitle);
            this.headerPanel.Controls.Add(this.lblHeader);

            // Header title

            this.lblHeader.AutoSize = true;

            this.lblHeader.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    23F,
                    System.Drawing.FontStyle.Bold);

            this.lblHeader.ForeColor =
                System.Drawing.Color.White;

            this.lblHeader.Location =
                new System.Drawing.Point(
                    30,
                    15);

            this.lblHeader.Text =
                "Manage Services";

            // Header subtitle

            this.lblSubtitle.AutoSize = true;

            this.lblSubtitle.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10F);

            this.lblSubtitle.ForeColor =
                System.Drawing.Color.FromArgb(
                    148,
                    163,
                    184);

            this.lblSubtitle.Location =
                new System.Drawing.Point(
                    33,
                    59);

            this.lblSubtitle.Text =
                "Create, publish and manage the services you offer to clients";

            // ============================================================
            // EDITOR PANEL
            // ============================================================

            this.editorPanel.BackColor =
                System.Drawing.Color.White;

            this.editorPanel.Location =
                new System.Drawing.Point(
                    28,
                    120);

            this.editorPanel.Size =
                new System.Drawing.Size(
                    1144,
                    330);

            this.editorPanel.Anchor =
                System.Windows.Forms.AnchorStyles.Top |
                System.Windows.Forms.AnchorStyles.Left |
                System.Windows.Forms.AnchorStyles.Right;

            this.editorPanel.BorderStyle =
                System.Windows.Forms.BorderStyle.None;

            this.editorPanel.Padding =
                new System.Windows.Forms.Padding(
                    24);

            this.editorPanel.Controls.Add(this.lblEditorTitle);
            this.editorPanel.Controls.Add(this.lblMode);

            this.editorPanel.Controls.Add(this.lblTitle);
            this.editorPanel.Controls.Add(this.txtTitle);

            this.editorPanel.Controls.Add(this.lblCategory);
            this.editorPanel.Controls.Add(this.cmbCategory);

            this.editorPanel.Controls.Add(this.lblDescription);
            this.editorPanel.Controls.Add(this.txtDescription);

            this.editorPanel.Controls.Add(this.lblPrice);
            this.editorPanel.Controls.Add(this.nudPrice);

            this.editorPanel.Controls.Add(this.lblDeliveryDays);
            this.editorPanel.Controls.Add(this.nudDeliveryDays);

            this.editorPanel.Controls.Add(this.lblAvailableSlots);
            this.editorPanel.Controls.Add(this.nudAvailableSlots);

            this.editorPanel.Controls.Add(this.chkActive);
            this.editorPanel.Controls.Add(this.lblSelectedService);

            this.editorPanel.Controls.Add(this.btnAdd);
            this.editorPanel.Controls.Add(this.btnUpdate);
            this.editorPanel.Controls.Add(this.btnToggleStatus);
            this.editorPanel.Controls.Add(this.btnDelete);
            this.editorPanel.Controls.Add(this.btnClear);

            // ============================================================
            // EDITOR TITLE
            // ============================================================

            this.lblEditorTitle.AutoSize = true;

            this.lblEditorTitle.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    14F,
                    System.Drawing.FontStyle.Bold);

            this.lblEditorTitle.ForeColor =
                System.Drawing.Color.FromArgb(
                    15,
                    23,
                    42);

            this.lblEditorTitle.Location =
                new System.Drawing.Point(
                    24,
                    18);

            this.lblEditorTitle.Text =
                "Service Information";

            // ============================================================
            // MODE LABEL
            // ============================================================

            this.lblMode.AutoSize = true;

            this.lblMode.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    9F,
                    System.Drawing.FontStyle.Bold);

            this.lblMode.ForeColor =
                System.Drawing.Color.FromArgb(
                    37,
                    99,
                    235);

            this.lblMode.Location =
                new System.Drawing.Point(
                    900,
                    23);

            this.lblMode.Anchor =
                System.Windows.Forms.AnchorStyles.Top |
                System.Windows.Forms.AnchorStyles.Right;

            this.lblMode.Text =
                "CREATE NEW SERVICE";

            // ============================================================
            // SERVICE TITLE
            // ============================================================

            this.lblTitle.AutoSize = true;

            this.lblTitle.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    9F,
                    System.Drawing.FontStyle.Bold);

            this.lblTitle.ForeColor =
                System.Drawing.Color.FromArgb(
                    51,
                    65,
                    85);

            this.lblTitle.Location =
                new System.Drawing.Point(
                    24,
                    62);

            this.lblTitle.Text =
                "Service Title";

            this.txtTitle.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10F);

            this.txtTitle.Location =
                new System.Drawing.Point(
                    24,
                    84);

            this.txtTitle.Size =
                new System.Drawing.Size(
                    510,
                    31);

            this.txtTitle.MaxLength = 150;

            this.txtTitle.BorderStyle =
                System.Windows.Forms.BorderStyle.FixedSingle;

            this.txtTitle.BackColor =
                System.Drawing.Color.FromArgb(
                    248,
                    250,
                    252);

            // ============================================================
            // CATEGORY
            // ============================================================

            this.lblCategory.AutoSize = true;

            this.lblCategory.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    9F,
                    System.Drawing.FontStyle.Bold);

            this.lblCategory.ForeColor =
                System.Drawing.Color.FromArgb(
                    51,
                    65,
                    85);

            this.lblCategory.Location =
                new System.Drawing.Point(
                    558,
                    62);

            this.lblCategory.Text =
                "Category";

            this.cmbCategory.DropDownStyle =
                System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.cmbCategory.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10F);

            this.cmbCategory.Location =
                new System.Drawing.Point(
                    558,
                    84);

            this.cmbCategory.Size =
                new System.Drawing.Size(
                    350,
                    31);

            this.cmbCategory.BackColor =
                System.Drawing.Color.FromArgb(
                    248,
                    250,
                    252);

            // ============================================================
            // DESCRIPTION
            // ============================================================

            this.lblDescription.AutoSize = true;

            this.lblDescription.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    9F,
                    System.Drawing.FontStyle.Bold);

            this.lblDescription.ForeColor =
                System.Drawing.Color.FromArgb(
                    51,
                    65,
                    85);

            this.lblDescription.Location =
                new System.Drawing.Point(
                    24,
                    128);

            this.lblDescription.Text =
                "Service Description";

            this.txtDescription.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10F);

            this.txtDescription.Location =
                new System.Drawing.Point(
                    24,
                    150);

            this.txtDescription.Size =
                new System.Drawing.Size(
                    884,
                    64);

            this.txtDescription.Multiline = true;

            this.txtDescription.ScrollBars =
                System.Windows.Forms.ScrollBars.Vertical;

            this.txtDescription.MaxLength = 1500;

            this.txtDescription.BorderStyle =
                System.Windows.Forms.BorderStyle.FixedSingle;

            this.txtDescription.BackColor =
                System.Drawing.Color.FromArgb(
                    248,
                    250,
                    252);

            // ============================================================
            // PRICE
            // ============================================================

            this.lblPrice.AutoSize = true;

            this.lblPrice.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    9F,
                    System.Drawing.FontStyle.Bold);

            this.lblPrice.ForeColor =
                System.Drawing.Color.FromArgb(
                    51,
                    65,
                    85);

            this.lblPrice.Location =
                new System.Drawing.Point(
                    24,
                    229);

            this.lblPrice.Text =
                "Price (BDT)";

            this.nudPrice.DecimalPlaces = 2;

            this.nudPrice.Maximum =
                100000000;

            this.nudPrice.Minimum = 0;

            this.nudPrice.ThousandsSeparator = true;

            this.nudPrice.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10F);

            this.nudPrice.Location =
                new System.Drawing.Point(
                    24,
                    251);

            this.nudPrice.Size =
                new System.Drawing.Size(
                    150,
                    31);

            this.nudPrice.BackColor =
                System.Drawing.Color.FromArgb(
                    248,
                    250,
                    252);

            // ============================================================
            // DELIVERY
            // ============================================================

            this.lblDeliveryDays.AutoSize = true;

            this.lblDeliveryDays.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    9F,
                    System.Drawing.FontStyle.Bold);

            this.lblDeliveryDays.ForeColor =
                System.Drawing.Color.FromArgb(
                    51,
                    65,
                    85);

            this.lblDeliveryDays.Location =
                new System.Drawing.Point(
                    195,
                    229);

            this.lblDeliveryDays.Text =
                "Delivery Days";

            this.nudDeliveryDays.Minimum = 1;

            this.nudDeliveryDays.Maximum = 365;

            this.nudDeliveryDays.Value = 1;

            this.nudDeliveryDays.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10F);

            this.nudDeliveryDays.Location =
                new System.Drawing.Point(
                    195,
                    251);

            this.nudDeliveryDays.Size =
                new System.Drawing.Size(
                    125,
                    31);

            this.nudDeliveryDays.BackColor =
                System.Drawing.Color.FromArgb(
                    248,
                    250,
                    252);

            // ============================================================
            // AVAILABLE SLOTS
            // ============================================================

            this.lblAvailableSlots.AutoSize = true;

            this.lblAvailableSlots.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    9F,
                    System.Drawing.FontStyle.Bold);

            this.lblAvailableSlots.ForeColor =
                System.Drawing.Color.FromArgb(
                    51,
                    65,
                    85);

            this.lblAvailableSlots.Location =
                new System.Drawing.Point(
                    342,
                    229);

            this.lblAvailableSlots.Text =
                "Available Slots";

            this.nudAvailableSlots.Minimum = 0;

            this.nudAvailableSlots.Maximum = 100000;

            this.nudAvailableSlots.Value = 1;

            this.nudAvailableSlots.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10F);

            this.nudAvailableSlots.Location =
                new System.Drawing.Point(
                    342,
                    251);

            this.nudAvailableSlots.Size =
                new System.Drawing.Size(
                    125,
                    31);

            this.nudAvailableSlots.BackColor =
                System.Drawing.Color.FromArgb(
                    248,
                    250,
                    252);

            // ============================================================
            // ACTIVE CHECKBOX
            // ============================================================

            this.chkActive.AutoSize = true;

            this.chkActive.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    9F,
                    System.Drawing.FontStyle.Bold);

            this.chkActive.ForeColor =
                System.Drawing.Color.FromArgb(
                    51,
                    65,
                    85);

            this.chkActive.Location =
                new System.Drawing.Point(
                    490,
                    254);

            this.chkActive.Text =
                "Service is active";

            this.chkActive.Checked = true;

            // ============================================================
            // SELECTED SERVICE
            // ============================================================

            this.lblSelectedService.AutoSize = true;

            this.lblSelectedService.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    8.5F);

            this.lblSelectedService.ForeColor =
                System.Drawing.Color.FromArgb(
                    100,
                    116,
                    139);

            this.lblSelectedService.Location =
                new System.Drawing.Point(
                    490,
                    278);

            this.lblSelectedService.Text =
                "No service selected";

            // ============================================================
            // PUBLISH BUTTON
            // ============================================================

            this.btnAdd.BackColor =
                System.Drawing.Color.FromArgb(
                    37,
                    99,
                    235);

            this.btnAdd.FlatStyle =
                System.Windows.Forms.FlatStyle.Flat;

            this.btnAdd.FlatAppearance.BorderSize = 0;

            this.btnAdd.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    9F,
                    System.Drawing.FontStyle.Bold);

            this.btnAdd.ForeColor =
                System.Drawing.Color.White;

            this.btnAdd.Location =
                new System.Drawing.Point(
                    700,
                    245);

            this.btnAdd.Size =
                new System.Drawing.Size(
                    125,
                    40);

            this.btnAdd.Text =
                "Publish Service";

            this.btnAdd.UseVisualStyleBackColor = false;

            this.btnAdd.Cursor =
                System.Windows.Forms.Cursors.Hand;

            this.btnAdd.Click +=
                new System.EventHandler(
                    this.btnAdd_Click);

            // ============================================================
            // UPDATE BUTTON
            // ============================================================

            this.btnUpdate.BackColor =
                System.Drawing.Color.FromArgb(
                    16,
                    185,
                    129);

            this.btnUpdate.FlatStyle =
                System.Windows.Forms.FlatStyle.Flat;

            this.btnUpdate.FlatAppearance.BorderSize = 0;

            this.btnUpdate.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    9F,
                    System.Drawing.FontStyle.Bold);

            this.btnUpdate.ForeColor =
                System.Drawing.Color.White;

            this.btnUpdate.Location =
                new System.Drawing.Point(
                    835,
                    245);

            this.btnUpdate.Size =
                new System.Drawing.Size(
                    95,
                    40);

            this.btnUpdate.Text =
                "Update";

            this.btnUpdate.Enabled = false;

            this.btnUpdate.UseVisualStyleBackColor = false;

            this.btnUpdate.Cursor =
                System.Windows.Forms.Cursors.Hand;

            this.btnUpdate.Click +=
                new System.EventHandler(
                    this.btnUpdate_Click);

            // ============================================================
            // TOGGLE STATUS
            // ============================================================

            this.btnToggleStatus.BackColor =
                System.Drawing.Color.FromArgb(
                    245,
                    158,
                    11);

            this.btnToggleStatus.FlatStyle =
                System.Windows.Forms.FlatStyle.Flat;

            this.btnToggleStatus.FlatAppearance.BorderSize = 0;

            this.btnToggleStatus.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    8.5F,
                    System.Drawing.FontStyle.Bold);

            this.btnToggleStatus.ForeColor =
                System.Drawing.Color.White;

            this.btnToggleStatus.Location =
                new System.Drawing.Point(
                    940,
                    245);

            this.btnToggleStatus.Size =
                new System.Drawing.Size(
                    165,
                    40);

            this.btnToggleStatus.Text =
                "Activate / Deactivate";

            this.btnToggleStatus.Enabled = false;

            this.btnToggleStatus.UseVisualStyleBackColor = false;

            this.btnToggleStatus.Cursor =
                System.Windows.Forms.Cursors.Hand;

            this.btnToggleStatus.Click +=
                new System.EventHandler(
                    this.btnToggleStatus_Click);

            // ============================================================
            // DELETE
            // ============================================================

            this.btnDelete.BackColor =
                System.Drawing.Color.White;

            this.btnDelete.FlatStyle =
                System.Windows.Forms.FlatStyle.Flat;

            this.btnDelete.FlatAppearance.BorderColor =
                System.Drawing.Color.FromArgb(
                    220,
                    38,
                    38);

            this.btnDelete.FlatAppearance.BorderSize = 1;

            this.btnDelete.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    9F,
                    System.Drawing.FontStyle.Bold);

            this.btnDelete.ForeColor =
                System.Drawing.Color.FromArgb(
                    185,
                    28,
                    28);

            this.btnDelete.Location =
                new System.Drawing.Point(
                    24,
                    294);

            this.btnDelete.Size =
                new System.Drawing.Size(
                    90,
                    30);

            this.btnDelete.Text =
                "Delete";

            this.btnDelete.Enabled = false;

            this.btnDelete.UseVisualStyleBackColor = true;

            this.btnDelete.Cursor =
                System.Windows.Forms.Cursors.Hand;

            this.btnDelete.Click +=
                new System.EventHandler(
                    this.btnDelete_Click);

            // ============================================================
            // CLEAR
            // ============================================================

            this.btnClear.BackColor =
                System.Drawing.Color.White;

            this.btnClear.FlatStyle =
                System.Windows.Forms.FlatStyle.Flat;

            this.btnClear.FlatAppearance.BorderColor =
                System.Drawing.Color.FromArgb(
                    203,
                    213,
                    225);

            this.btnClear.FlatAppearance.BorderSize = 1;

            this.btnClear.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    9F);

            this.btnClear.ForeColor =
                System.Drawing.Color.FromArgb(
                    51,
                    65,
                    85);

            this.btnClear.Location =
                new System.Drawing.Point(
                    124,
                    294);

            this.btnClear.Size =
                new System.Drawing.Size(
                    90,
                    30);

            this.btnClear.Text =
                "Clear";

            this.btnClear.UseVisualStyleBackColor = true;

            this.btnClear.Cursor =
                System.Windows.Forms.Cursors.Hand;

            this.btnClear.Click +=
                new System.EventHandler(
                    this.btnClear_Click);

            // ============================================================
            // SERVICES PANEL
            // ============================================================

            this.servicesPanel.BackColor =
                System.Drawing.Color.White;

            this.servicesPanel.Location =
                new System.Drawing.Point(
                    28,
                    470);

            this.servicesPanel.Size =
                new System.Drawing.Size(
                    1144,
                    300);

            this.servicesPanel.Anchor =
                System.Windows.Forms.AnchorStyles.Top |
                System.Windows.Forms.AnchorStyles.Bottom |
                System.Windows.Forms.AnchorStyles.Left |
                System.Windows.Forms.AnchorStyles.Right;

            this.servicesPanel.BorderStyle =
                System.Windows.Forms.BorderStyle.None;

            this.servicesPanel.Controls.Add(this.lblServicesTitle);
            this.servicesPanel.Controls.Add(this.lblServiceCount);
            this.servicesPanel.Controls.Add(this.btnRefresh);
            this.servicesPanel.Controls.Add(this.dgvServices);

            // ============================================================
            // SERVICES TITLE
            // ============================================================

            this.lblServicesTitle.AutoSize = true;

            this.lblServicesTitle.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    14F,
                    System.Drawing.FontStyle.Bold);

            this.lblServicesTitle.ForeColor =
                System.Drawing.Color.FromArgb(
                    15,
                    23,
                    42);

            this.lblServicesTitle.Location =
                new System.Drawing.Point(
                    24,
                    17);

            this.lblServicesTitle.Text =
                "Your Published Services";

            // ============================================================
            // SERVICE COUNT
            // ============================================================

            this.lblServiceCount.AutoSize = true;

            this.lblServiceCount.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    8.5F,
                    System.Drawing.FontStyle.Bold);

            this.lblServiceCount.ForeColor =
                System.Drawing.Color.FromArgb(
                    37,
                    99,
                    235);

            this.lblServiceCount.BackColor =
                System.Drawing.Color.FromArgb(
                    239,
                    246,
                    255);

            this.lblServiceCount.Location =
                new System.Drawing.Point(
                    238,
                    20);

            this.lblServiceCount.Text =
                "0 service(s)";

            // ============================================================
            // REFRESH BUTTON
            // ============================================================

            this.btnRefresh.BackColor =
                System.Drawing.Color.White;

            this.btnRefresh.FlatStyle =
                System.Windows.Forms.FlatStyle.Flat;

            this.btnRefresh.FlatAppearance.BorderColor =
                System.Drawing.Color.FromArgb(
                    203,
                    213,
                    225);

            this.btnRefresh.FlatAppearance.BorderSize = 1;

            this.btnRefresh.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    9F,
                    System.Drawing.FontStyle.Bold);

            this.btnRefresh.ForeColor =
                System.Drawing.Color.FromArgb(
                    51,
                    65,
                    85);

            this.btnRefresh.Location =
                new System.Drawing.Point(
                    1010,
                    14);

            this.btnRefresh.Size =
                new System.Drawing.Size(
                    105,
                    36);

            this.btnRefresh.Anchor =
                System.Windows.Forms.AnchorStyles.Top |
                System.Windows.Forms.AnchorStyles.Right;

            this.btnRefresh.Text =
                "Refresh";

            this.btnRefresh.UseVisualStyleBackColor = true;

            this.btnRefresh.Cursor =
                System.Windows.Forms.Cursors.Hand;

            this.btnRefresh.Click +=
                new System.EventHandler(
                    this.btnRefresh_Click);

            // ============================================================
            // DATAGRIDVIEW
            // ============================================================

            this.dgvServices.AllowUserToAddRows = false;
            this.dgvServices.AllowUserToDeleteRows = false;
            this.dgvServices.AllowUserToResizeRows = false;

            this.dgvServices.AutoGenerateColumns = false;

            this.dgvServices.BackgroundColor =
                System.Drawing.Color.White;

            this.dgvServices.BorderStyle =
                System.Windows.Forms.BorderStyle.None;

            this.dgvServices.CellBorderStyle =
                System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;

            this.dgvServices.ColumnHeadersBorderStyle =
                System.Windows.Forms.DataGridViewHeaderBorderStyle.None;

            this.dgvServices.ColumnHeadersHeight = 42;

            this.dgvServices.ColumnHeadersHeightSizeMode =
                System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            this.dgvServices.ColumnHeadersDefaultCellStyle =
                new System.Windows.Forms.DataGridViewCellStyle
                {
                    BackColor =
                        System.Drawing.Color.FromArgb(
                            248,
                            250,
                            252),

                    Font =
                        new System.Drawing.Font(
                            "Segoe UI",
                            9F,
                            System.Drawing.FontStyle.Bold),

                    ForeColor =
                        System.Drawing.Color.FromArgb(
                            71,
                            85,
                            105),

                    Alignment =
                        System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft,

                    SelectionBackColor =
                        System.Drawing.Color.FromArgb(
                            248,
                            250,
                            252),

                    SelectionForeColor =
                        System.Drawing.Color.FromArgb(
                            71,
                            85,
                            105),

                    Padding =
                        new System.Windows.Forms.Padding(
                            8,
                            0,
                            8,
                            0)
                };

            this.dgvServices.DefaultCellStyle =
                new System.Windows.Forms.DataGridViewCellStyle
                {
                    Font =
                        new System.Drawing.Font(
                            "Segoe UI",
                            9F),

                    ForeColor =
                        System.Drawing.Color.FromArgb(
                            30,
                            41,
                            59),

                    BackColor =
                        System.Drawing.Color.White,

                    SelectionBackColor =
                        System.Drawing.Color.FromArgb(
                            239,
                            246,
                            255),

                    SelectionForeColor =
                        System.Drawing.Color.FromArgb(
                            15,
                            23,
                            42),

                    Alignment =
                        System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft,

                    Padding =
                        new System.Windows.Forms.Padding(
                            8,
                            0,
                            8,
                            0)
                };

            this.dgvServices.AlternatingRowsDefaultCellStyle =
                new System.Windows.Forms.DataGridViewCellStyle
                {
                    BackColor =
                        System.Drawing.Color.FromArgb(
                            250,
                            251,
                            253)
                };

            this.dgvServices.EnableHeadersVisualStyles = false;

            this.dgvServices.Location =
                new System.Drawing.Point(
                    20,
                    62);

            this.dgvServices.Size =
                new System.Drawing.Size(
                    1100,
                    218);

            this.dgvServices.Anchor =
                System.Windows.Forms.AnchorStyles.Top |
                System.Windows.Forms.AnchorStyles.Bottom |
                System.Windows.Forms.AnchorStyles.Left |
                System.Windows.Forms.AnchorStyles.Right;

            this.dgvServices.MultiSelect = false;

            this.dgvServices.ReadOnly = true;

            this.dgvServices.RowHeadersVisible = false;

            this.dgvServices.RowTemplate.Height = 38;

            this.dgvServices.SelectionMode =
                System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            this.dgvServices.AllowUserToResizeColumns = false;

            this.dgvServices.CellClick +=
                new System.Windows.Forms.DataGridViewCellEventHandler(
                    this.dgvServices_CellClick);

            // ============================================================
            // ID COLUMN
            // ============================================================

            this.colServiceId.HeaderText = "ID";

            this.colServiceId.Name = "colServiceId";

            this.colServiceId.DataPropertyName =
                "ServiceId";

            this.colServiceId.Width = 60;

            this.colServiceId.DefaultCellStyle =
                new System.Windows.Forms.DataGridViewCellStyle
                {
                    Alignment =
                        System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
                };

            // ============================================================
            // TITLE COLUMN
            // ============================================================

            this.colTitle.HeaderText = "Service";

            this.colTitle.Name = "colTitle";

            this.colTitle.DataPropertyName =
                "Title";

            this.colTitle.AutoSizeMode =
                System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;

            this.colTitle.FillWeight = 180;

            // ============================================================
            // CATEGORY COLUMN
            // ============================================================

            this.colCategory.HeaderText = "Category";

            this.colCategory.Name = "colCategory";

            this.colCategory.DataPropertyName =
                "CategoryName";

            this.colCategory.Width = 180;

            // ============================================================
            // PRICE COLUMN
            // ============================================================

            this.colPrice.HeaderText = "Price (BDT)";

            this.colPrice.Name = "colPrice";

            this.colPrice.DataPropertyName =
                "Price";

            this.colPrice.Width = 115;

            this.colPrice.DefaultCellStyle =
                new System.Windows.Forms.DataGridViewCellStyle
                {
                    Format = "N2",
                    Alignment =
                        System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
                };

            // ============================================================
            // DELIVERY COLUMN
            // ============================================================

            this.colDelivery.HeaderText = "Delivery";

            this.colDelivery.Name = "colDelivery";

            this.colDelivery.DataPropertyName =
                "DeliveryDays";

            this.colDelivery.Width = 100;

            this.colDelivery.DefaultCellStyle =
                new System.Windows.Forms.DataGridViewCellStyle
                {
                    Alignment =
                        System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
                };

            // ============================================================
            // SLOTS COLUMN
            // ============================================================

            this.colSlots.HeaderText = "Slots";

            this.colSlots.Name = "colSlots";

            this.colSlots.DataPropertyName =
                "AvailableSlots";

            this.colSlots.Width = 90;

            this.colSlots.DefaultCellStyle =
                new System.Windows.Forms.DataGridViewCellStyle
                {
                    Alignment =
                        System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
                };

            // ============================================================
            // STATUS COLUMN
            // ============================================================

            this.colStatus.HeaderText = "Active";

            this.colStatus.Name = "colStatus";

            this.colStatus.DataPropertyName =
                "IsActive";

            this.colStatus.Width = 80;

            this.colStatus.DefaultCellStyle =
                new System.Windows.Forms.DataGridViewCellStyle
                {
                    Alignment =
                        System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
                };

            // ============================================================
            // ADD COLUMNS
            // ============================================================

            this.dgvServices.Columns.AddRange(
                new System.Windows.Forms.DataGridViewColumn[]
                {
                    this.colServiceId,
                    this.colTitle,
                    this.colCategory,
                    this.colPrice,
                    this.colDelivery,
                    this.colSlots,
                    this.colStatus
                });

            // ============================================================
            // ADD CONTROLS TO FORM
            // ============================================================

            this.Controls.Add(this.servicesPanel);
            this.Controls.Add(this.editorPanel);
            this.Controls.Add(this.headerPanel);

            // ============================================================
            // FINALIZE
            // ============================================================

            ((System.ComponentModel.ISupportInitialize)
                (this.nudPrice)).EndInit();

            ((System.ComponentModel.ISupportInitialize)
                (this.nudDeliveryDays)).EndInit();

            ((System.ComponentModel.ISupportInitialize)
                (this.nudAvailableSlots)).EndInit();

            ((System.ComponentModel.ISupportInitialize)
                (this.dgvServices)).EndInit();

            this.ResumeLayout(false);
        }
    }
}