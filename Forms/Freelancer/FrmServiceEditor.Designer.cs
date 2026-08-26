namespace SkillHub.Forms.Freelancer
{
    partial class FrmServiceEditor
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label lblSubtitle;

        private System.Windows.Forms.Panel contentPanel;
        private System.Windows.Forms.Panel editorCard;

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label lblTitleCounter;

        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.ComboBox cmbCategory;

        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblDescriptionCounter;

        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.NumericUpDown nudPrice;

        private System.Windows.Forms.Label lblDeliveryDays;
        private System.Windows.Forms.NumericUpDown nudDeliveryDays;

        private System.Windows.Forms.Label lblAvailableSlots;
        private System.Windows.Forms.NumericUpDown nudAvailableSlots;

        private System.Windows.Forms.Label lblServiceStatus;

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.headerPanel = new System.Windows.Forms.Panel();
            this.lblHeader = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();

            this.contentPanel = new System.Windows.Forms.Panel();
            this.editorCard = new System.Windows.Forms.Panel();

            this.lblTitle = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.lblTitleCounter = new System.Windows.Forms.Label();

            this.lblCategory = new System.Windows.Forms.Label();
            this.cmbCategory = new System.Windows.Forms.ComboBox();

            this.lblDescription = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblDescriptionCounter = new System.Windows.Forms.Label();

            this.lblPrice = new System.Windows.Forms.Label();
            this.nudPrice = new System.Windows.Forms.NumericUpDown();

            this.lblDeliveryDays = new System.Windows.Forms.Label();
            this.nudDeliveryDays = new System.Windows.Forms.NumericUpDown();

            this.lblAvailableSlots = new System.Windows.Forms.Label();
            this.nudAvailableSlots = new System.Windows.Forms.NumericUpDown();

            this.lblServiceStatus = new System.Windows.Forms.Label();

            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.nudPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDeliveryDays)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAvailableSlots)).BeginInit();

            this.SuspendLayout();

            // =========================================================
            // FORM
            // =========================================================

            this.AutoScaleDimensions =
                new System.Drawing.SizeF(7F, 15F);

            this.AutoScaleMode =
                System.Windows.Forms.AutoScaleMode.Font;

            this.BackColor =
                System.Drawing.Color.FromArgb(245, 247, 250);

            this.ClientSize =
                new System.Drawing.Size(1040, 760);

            this.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10F,
                    System.Drawing.FontStyle.Regular,
                    System.Drawing.GraphicsUnit.Point);

            this.FormBorderStyle =
                System.Windows.Forms.FormBorderStyle.FixedSingle;

            this.MaximizeBox = false;
            this.MinimizeBox = false;

            this.StartPosition =
                System.Windows.Forms.FormStartPosition.CenterParent;

            this.Text = "SkillHub | Add Service";

            // =========================================================
            // HEADER
            // =========================================================

            this.headerPanel.BackColor =
                System.Drawing.Color.FromArgb(31, 41, 55);

            this.headerPanel.Dock =
                System.Windows.Forms.DockStyle.Top;

            this.headerPanel.Height = 112;

            this.headerPanel.Padding =
                new System.Windows.Forms.Padding(34, 20, 34, 15);

            this.headerPanel.Controls.Add(this.lblSubtitle);
            this.headerPanel.Controls.Add(this.lblHeader);

            // Header title

            this.lblHeader.AutoSize = true;

            this.lblHeader.Font =
                new System.Drawing.Font(
                    "Segoe UI Semibold",
                    23F,
                    System.Drawing.FontStyle.Bold);

            this.lblHeader.ForeColor =
                System.Drawing.Color.White;

            this.lblHeader.Location =
                new System.Drawing.Point(34, 18);

            this.lblHeader.Name = "lblHeader";

            this.lblHeader.Text =
                "Create New Service";

            // Header subtitle

            this.lblSubtitle.AutoSize = true;

            this.lblSubtitle.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10.5F);

            this.lblSubtitle.ForeColor =
                System.Drawing.Color.FromArgb(
                    203,
                    213,
                    225);

            this.lblSubtitle.Location =
                new System.Drawing.Point(37, 65);

            this.lblSubtitle.Name =
                "lblSubtitle";

            this.lblSubtitle.Text =
                "Publish a professional software-service listing.";

            // =========================================================
            // CONTENT
            // =========================================================

            this.contentPanel.Dock =
                System.Windows.Forms.DockStyle.Fill;

            this.contentPanel.Padding =
                new System.Windows.Forms.Padding(30);

            this.contentPanel.BackColor =
                System.Drawing.Color.FromArgb(245, 247, 250);

            this.contentPanel.Controls.Add(this.editorCard);

            // =========================================================
            // EDITOR CARD
            // =========================================================

            this.editorCard.BackColor =
                System.Drawing.Color.White;

            this.editorCard.Dock =
                System.Windows.Forms.DockStyle.Fill;

            this.editorCard.Padding =
                new System.Windows.Forms.Padding(32);

            // =========================================================
            // TITLE
            // =========================================================

            this.lblTitle.AutoSize = true;

            this.lblTitle.Font =
                new System.Drawing.Font(
                    "Segoe UI Semibold",
                    10F,
                    System.Drawing.FontStyle.Bold);

            this.lblTitle.ForeColor =
                System.Drawing.Color.FromArgb(55, 65, 81);

            this.lblTitle.Location =
                new System.Drawing.Point(32, 25);

            this.lblTitle.Text =
                "Service Title";

            this.txtTitle.BorderStyle =
                System.Windows.Forms.BorderStyle.FixedSingle;

            this.txtTitle.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    11F);

            this.txtTitle.Location =
                new System.Drawing.Point(32, 51);

            this.txtTitle.Size =
                new System.Drawing.Size(730, 32);

            this.txtTitle.Name =
                "txtTitle";

            

            this.lblTitleCounter.AutoSize = true;

            this.lblTitleCounter.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    8.5F);

            this.lblTitleCounter.ForeColor =
                System.Drawing.Color.Gray;

            this.lblTitleCounter.Location =
                new System.Drawing.Point(770, 61);

            this.lblTitleCounter.Text =
                "0 / 150";

            // =========================================================
            // CATEGORY
            // =========================================================

            this.lblCategory.AutoSize = true;

            this.lblCategory.Font =
                new System.Drawing.Font(
                    "Segoe UI Semibold",
                    10F,
                    System.Drawing.FontStyle.Bold);

            this.lblCategory.ForeColor =
                System.Drawing.Color.FromArgb(55, 65, 81);

            this.lblCategory.Location =
                new System.Drawing.Point(32, 101);

            this.lblCategory.Text =
                "Category";

            this.cmbCategory.DropDownStyle =
                System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.cmbCategory.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10.5F);

            this.cmbCategory.FormattingEnabled = true;

            this.cmbCategory.Location =
                new System.Drawing.Point(32, 127);

            this.cmbCategory.Size =
                new System.Drawing.Size(768, 31);

            this.cmbCategory.Name =
                "cmbCategory";

            // =========================================================
            // DESCRIPTION
            // =========================================================

            this.lblDescription.AutoSize = true;

            this.lblDescription.Font =
                new System.Drawing.Font(
                    "Segoe UI Semibold",
                    10F,
                    System.Drawing.FontStyle.Bold);

            this.lblDescription.ForeColor =
                System.Drawing.Color.FromArgb(55, 65, 81);

            this.lblDescription.Location =
                new System.Drawing.Point(32, 178);

            this.lblDescription.Text =
                "Service Description";

            this.txtDescription.BorderStyle =
                System.Windows.Forms.BorderStyle.FixedSingle;

            this.txtDescription.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10.5F);

            this.txtDescription.Location =
                new System.Drawing.Point(32, 204);

            this.txtDescription.Multiline = true;

            this.txtDescription.ScrollBars =
                System.Windows.Forms.ScrollBars.Vertical;

            this.txtDescription.Size =
                new System.Drawing.Size(768, 145);

            this.txtDescription.Name =
                "txtDescription";

            

            this.lblDescriptionCounter.AutoSize = true;

            this.lblDescriptionCounter.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    8.5F);

            this.lblDescriptionCounter.ForeColor =
                System.Drawing.Color.Gray;

            this.lblDescriptionCounter.Location =
                new System.Drawing.Point(700, 355);

            this.lblDescriptionCounter.Text =
                "0 / 1500";

            // =========================================================
            // PRICE
            // =========================================================

            this.lblPrice.AutoSize = true;

            this.lblPrice.Font =
                new System.Drawing.Font(
                    "Segoe UI Semibold",
                    10F,
                    System.Drawing.FontStyle.Bold);

            this.lblPrice.ForeColor =
                System.Drawing.Color.FromArgb(55, 65, 81);

            this.lblPrice.Location =
                new System.Drawing.Point(32, 388);

            this.lblPrice.Text =
                "Price (BDT)";

            this.nudPrice.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10.5F);

            this.nudPrice.Location =
                new System.Drawing.Point(32, 414);

            this.nudPrice.Size =
                new System.Drawing.Size(220, 31);

            this.nudPrice.Name =
                "nudPrice";

            this.nudPrice.DecimalPlaces = 2;

            this.nudPrice.Maximum =
                100000000;

            this.nudPrice.Minimum = 0;

            // =========================================================
            // DELIVERY DAYS
            // =========================================================

            this.lblDeliveryDays.AutoSize = true;

            this.lblDeliveryDays.Font =
                new System.Drawing.Font(
                    "Segoe UI Semibold",
                    10F,
                    System.Drawing.FontStyle.Bold);

            this.lblDeliveryDays.ForeColor =
                System.Drawing.Color.FromArgb(55, 65, 81);

            this.lblDeliveryDays.Location =
                new System.Drawing.Point(290, 388);

            this.lblDeliveryDays.Text =
                "Delivery Days";

            this.nudDeliveryDays.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10.5F);

            this.nudDeliveryDays.Location =
                new System.Drawing.Point(290, 414);

            this.nudDeliveryDays.Size =
                new System.Drawing.Size(220, 31);

            this.nudDeliveryDays.Name =
                "nudDeliveryDays";

            this.nudDeliveryDays.Minimum = 1;

            this.nudDeliveryDays.Maximum = 365;

            this.nudDeliveryDays.Value = 1;

            // =========================================================
            // AVAILABLE SLOTS
            // =========================================================

            this.lblAvailableSlots.AutoSize = true;

            this.lblAvailableSlots.Font =
                new System.Drawing.Font(
                    "Segoe UI Semibold",
                    10F,
                    System.Drawing.FontStyle.Bold);

            this.lblAvailableSlots.ForeColor =
                System.Drawing.Color.FromArgb(55, 65, 81);

            this.lblAvailableSlots.Location =
                new System.Drawing.Point(548, 388);

            this.lblAvailableSlots.Text =
                "Available Slots";

            this.nudAvailableSlots.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10.5F);

            this.nudAvailableSlots.Location =
                new System.Drawing.Point(548, 414);

            this.nudAvailableSlots.Size =
                new System.Drawing.Size(220, 31);

            this.nudAvailableSlots.Name =
                "nudAvailableSlots";

            this.nudAvailableSlots.Minimum = 0;

            this.nudAvailableSlots.Maximum = 100000;

            this.nudAvailableSlots.Value = 1;

            // =========================================================
            // STATUS
            // =========================================================

            this.lblServiceStatus.AutoSize = true;

            this.lblServiceStatus.Font =
                new System.Drawing.Font(
                    "Segoe UI Semibold",
                    9.5F,
                    System.Drawing.FontStyle.Bold);

            this.lblServiceStatus.ForeColor =
                System.Drawing.Color.SeaGreen;

            this.lblServiceStatus.Location =
                new System.Drawing.Point(32, 468);

            this.lblServiceStatus.Text =
                "New Service";

            // =========================================================
            // CANCEL BUTTON
            // =========================================================

            this.btnCancel.BackColor =
                System.Drawing.Color.FromArgb(229, 231, 235);

            this.btnCancel.FlatAppearance.BorderSize = 0;

            this.btnCancel.FlatStyle =
                System.Windows.Forms.FlatStyle.Flat;

            this.btnCancel.Font =
                new System.Drawing.Font(
                    "Segoe UI Semibold",
                    10F,
                    System.Drawing.FontStyle.Bold);

            this.btnCancel.ForeColor =
                System.Drawing.Color.FromArgb(55, 65, 81);

            this.btnCancel.Location =
                new System.Drawing.Point(530, 500);

            this.btnCancel.Size =
                new System.Drawing.Size(135, 42);

            this.btnCancel.Name =
                "btnCancel";

            this.btnCancel.Text =
                "Cancel";

            this.btnCancel.UseVisualStyleBackColor = false;

            this.btnCancel.Click +=
                new System.EventHandler(
                    this.btnCancel_Click);

            // =========================================================
            // SAVE BUTTON
            // =========================================================

            this.btnSave.BackColor =
                System.Drawing.Color.FromArgb(37, 99, 235);

            this.btnSave.FlatAppearance.BorderSize = 0;

            this.btnSave.FlatStyle =
                System.Windows.Forms.FlatStyle.Flat;

            this.btnSave.Font =
                new System.Drawing.Font(
                    "Segoe UI Semibold",
                    10F,
                    System.Drawing.FontStyle.Bold);

            this.btnSave.ForeColor =
                System.Drawing.Color.White;

            this.btnSave.Location =
                new System.Drawing.Point(680, 500);

            this.btnSave.Size =
                new System.Drawing.Size(155, 42);

            this.btnSave.Name =
                "btnSave";

            this.btnSave.Text =
                "Publish Service";

            this.btnSave.UseVisualStyleBackColor = false;

            this.btnSave.Click +=
                new System.EventHandler(
                    this.btnSave_Click);

            // =========================================================
            // ADD CONTROLS
            // =========================================================

            this.editorCard.Controls.Add(this.lblTitle);
            this.editorCard.Controls.Add(this.txtTitle);
            this.editorCard.Controls.Add(this.lblTitleCounter);

            this.editorCard.Controls.Add(this.lblCategory);
            this.editorCard.Controls.Add(this.cmbCategory);

            this.editorCard.Controls.Add(this.lblDescription);
            this.editorCard.Controls.Add(this.txtDescription);
            this.editorCard.Controls.Add(this.lblDescriptionCounter);

            this.editorCard.Controls.Add(this.lblPrice);
            this.editorCard.Controls.Add(this.nudPrice);

            this.editorCard.Controls.Add(this.lblDeliveryDays);
            this.editorCard.Controls.Add(this.nudDeliveryDays);

            this.editorCard.Controls.Add(this.lblAvailableSlots);
            this.editorCard.Controls.Add(this.nudAvailableSlots);

            this.editorCard.Controls.Add(this.lblServiceStatus);

            this.editorCard.Controls.Add(this.btnCancel);
            this.editorCard.Controls.Add(this.btnSave);

            // =========================================================
            // FORM CONTROLS
            // =========================================================

            this.Controls.Add(this.contentPanel);
            this.Controls.Add(this.headerPanel);

            ((System.ComponentModel.ISupportInitialize)(this.nudPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDeliveryDays)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAvailableSlots)).EndInit();

            this.ResumeLayout(false);
        }
    }
}