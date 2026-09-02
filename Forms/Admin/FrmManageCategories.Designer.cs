namespace SkillHub.Forms.Admin
{
    partial class FrmManageCategories
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;

        private System.Windows.Forms.Panel pnlEditor;
        private System.Windows.Forms.Label lblCategoryName;
        private System.Windows.Forms.TextBox txtCategoryName;

        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDeactivate;
        private System.Windows.Forms.Button btnClear;

        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnRefresh;

        private System.Windows.Forms.Panel pnlGrid;
        private System.Windows.Forms.DataGridView dgvCategories;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();

            this.pnlEditor = new System.Windows.Forms.Panel();
            this.lblCategoryName = new System.Windows.Forms.Label();
            this.txtCategoryName = new System.Windows.Forms.TextBox();

            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDeactivate = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();

            this.pnlSearch = new System.Windows.Forms.Panel();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();

            this.pnlGrid = new System.Windows.Forms.Panel();
            this.dgvCategories = new System.Windows.Forms.DataGridView();

            this.pnlHeader.SuspendLayout();
            this.pnlEditor.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCategories)).BeginInit();
            this.SuspendLayout();

            // ============================================================
            // FORM
            // ============================================================

            this.AutoScaleDimensions =
                new System.Drawing.SizeF(8F, 16F);

            this.AutoScaleMode =
                System.Windows.Forms.AutoScaleMode.Font;

            this.BackColor =
                System.Drawing.Color.FromArgb(245, 247, 250);

            this.ClientSize =
                new System.Drawing.Size(1050, 700);

            this.MinimumSize =
                new System.Drawing.Size(950, 600);

            this.StartPosition =
                System.Windows.Forms.FormStartPosition.CenterParent;

            this.Text =
                "SkillHub | Manage Categories";

            this.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    9F,
                    System.Drawing.FontStyle.Regular,
                    System.Drawing.GraphicsUnit.Point);

            // ============================================================
            // HEADER
            // ============================================================

            this.pnlHeader.BackColor =
                System.Drawing.Color.FromArgb(31, 41, 55);

            this.pnlHeader.Dock =
                System.Windows.Forms.DockStyle.Top;

            this.pnlHeader.Height = 95;

            this.pnlHeader.Padding =
                new System.Windows.Forms.Padding(28, 18, 28, 10);

            this.pnlHeader.Controls.Add(this.lblSubtitle);
            this.pnlHeader.Controls.Add(this.lblTitle);

            // TITLE

            this.lblTitle.AutoSize = true;

            this.lblTitle.Font =
                new System.Drawing.Font(
                    "Segoe UI Semibold",
                    22F,
                    System.Drawing.FontStyle.Bold);

            this.lblTitle.ForeColor =
                System.Drawing.Color.White;

            this.lblTitle.Location =
                new System.Drawing.Point(28, 12);

            this.lblTitle.Text =
                "Category Management";

            // SUBTITLE

            this.lblSubtitle.AutoSize = true;

            this.lblSubtitle.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    9.5F);

            this.lblSubtitle.ForeColor =
                System.Drawing.Color.FromArgb(209, 213, 219);

            this.lblSubtitle.Location =
                new System.Drawing.Point(31, 57);

            this.lblSubtitle.Text =
                "Create, update, search and deactivate marketplace categories.";

            // ============================================================
            // EDITOR PANEL
            // ============================================================

            this.pnlEditor.BackColor =
                System.Drawing.Color.White;

            this.pnlEditor.Dock =
                System.Windows.Forms.DockStyle.Top;

            this.pnlEditor.Height = 125;

            this.pnlEditor.Padding =
                new System.Windows.Forms.Padding(25, 18, 25, 15);

            this.pnlEditor.Controls.Add(this.btnClear);
            this.pnlEditor.Controls.Add(this.btnDeactivate);
            this.pnlEditor.Controls.Add(this.btnUpdate);
            this.pnlEditor.Controls.Add(this.btnAdd);
            this.pnlEditor.Controls.Add(this.txtCategoryName);
            this.pnlEditor.Controls.Add(this.lblCategoryName);

            // CATEGORY LABEL

            this.lblCategoryName.AutoSize = true;

            this.lblCategoryName.Font =
                new System.Drawing.Font(
                    "Segoe UI Semibold",
                    9.5F,
                    System.Drawing.FontStyle.Bold);

            this.lblCategoryName.ForeColor =
                System.Drawing.Color.FromArgb(55, 65, 81);

            this.lblCategoryName.Location =
                new System.Drawing.Point(25, 20);

            this.lblCategoryName.Text =
                "Category Name";

            // CATEGORY TEXTBOX

            this.txtCategoryName.BorderStyle =
                System.Windows.Forms.BorderStyle.FixedSingle;

            this.txtCategoryName.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10F);

            this.txtCategoryName.Location =
                new System.Drawing.Point(25, 48);

            this.txtCategoryName.Size =
                new System.Drawing.Size(350, 30);

            // ADD

            this.btnAdd.BackColor =
                System.Drawing.Color.FromArgb(22, 163, 74);

            this.btnAdd.FlatAppearance.BorderSize = 0;

            this.btnAdd.FlatStyle =
                System.Windows.Forms.FlatStyle.Flat;

            this.btnAdd.Font =
                new System.Drawing.Font(
                    "Segoe UI Semibold",
                    9F,
                    System.Drawing.FontStyle.Bold);

            this.btnAdd.ForeColor =
                System.Drawing.Color.White;

            this.btnAdd.Location =
                new System.Drawing.Point(400, 47);

            this.btnAdd.Size =
                new System.Drawing.Size(115, 34);

            this.btnAdd.Text =
                "Add";

            this.btnAdd.UseVisualStyleBackColor = false;

            this.btnAdd.Click +=
                new System.EventHandler(this.btnAdd_Click);

            // UPDATE

            this.btnUpdate.BackColor =
                System.Drawing.Color.FromArgb(37, 99, 235);

            this.btnUpdate.FlatAppearance.BorderSize = 0;

            this.btnUpdate.FlatStyle =
                System.Windows.Forms.FlatStyle.Flat;

            this.btnUpdate.Font =
                new System.Drawing.Font(
                    "Segoe UI Semibold",
                    9F,
                    System.Drawing.FontStyle.Bold);

            this.btnUpdate.ForeColor =
                System.Drawing.Color.White;

            this.btnUpdate.Location =
                new System.Drawing.Point(525, 47);

            this.btnUpdate.Size =
                new System.Drawing.Size(115, 34);

            this.btnUpdate.Text =
                "Update";

            this.btnUpdate.UseVisualStyleBackColor = false;

            this.btnUpdate.Click +=
                new System.EventHandler(this.btnUpdate_Click);

            // DEACTIVATE

            this.btnDeactivate.BackColor =
                System.Drawing.Color.FromArgb(220, 38, 38);

            this.btnDeactivate.FlatAppearance.BorderSize = 0;

            this.btnDeactivate.FlatStyle =
                System.Windows.Forms.FlatStyle.Flat;

            this.btnDeactivate.Font =
                new System.Drawing.Font(
                    "Segoe UI Semibold",
                    9F,
                    System.Drawing.FontStyle.Bold);

            this.btnDeactivate.ForeColor =
                System.Drawing.Color.White;

            this.btnDeactivate.Location =
                new System.Drawing.Point(650, 47);

            this.btnDeactivate.Size =
                new System.Drawing.Size(125, 34);

            this.btnDeactivate.Text =
                "Deactivate";

            this.btnDeactivate.UseVisualStyleBackColor = false;

            this.btnDeactivate.Click +=
                new System.EventHandler(this.btnDeactivate_Click);

            // CLEAR

            this.btnClear.BackColor =
                System.Drawing.Color.FromArgb(107, 114, 128);

            this.btnClear.FlatAppearance.BorderSize = 0;

            this.btnClear.FlatStyle =
                System.Windows.Forms.FlatStyle.Flat;

            this.btnClear.Font =
                new System.Drawing.Font(
                    "Segoe UI Semibold",
                    9F,
                    System.Drawing.FontStyle.Bold);

            this.btnClear.ForeColor =
                System.Drawing.Color.White;

            this.btnClear.Location =
                new System.Drawing.Point(785, 47);

            this.btnClear.Size =
                new System.Drawing.Size(100, 34);

            this.btnClear.Text =
                "Clear";

            this.btnClear.UseVisualStyleBackColor = false;

            this.btnClear.Click +=
                new System.EventHandler(this.btnClear_Click);

            // ============================================================
            // SEARCH PANEL
            // ============================================================

            this.pnlSearch.BackColor =
                System.Drawing.Color.FromArgb(249, 250, 251);

            this.pnlSearch.Dock =
                System.Windows.Forms.DockStyle.Top;

            this.pnlSearch.Height = 75;

            this.pnlSearch.Padding =
                new System.Windows.Forms.Padding(25, 15, 25, 10);

            this.pnlSearch.Controls.Add(this.btnRefresh);
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Controls.Add(this.txtSearch);
            this.pnlSearch.Controls.Add(this.lblSearch);

            // SEARCH LABEL

            this.lblSearch.AutoSize = true;

            this.lblSearch.Font =
                new System.Drawing.Font(
                    "Segoe UI Semibold",
                    9.5F,
                    System.Drawing.FontStyle.Bold);

            this.lblSearch.ForeColor =
                System.Drawing.Color.FromArgb(55, 65, 81);

            this.lblSearch.Location =
                new System.Drawing.Point(25, 24);

            this.lblSearch.Text =
                "Search";

            // SEARCH TEXTBOX

            this.txtSearch.BorderStyle =
                System.Windows.Forms.BorderStyle.FixedSingle;

            this.txtSearch.Font =
                new System.Drawing.Font(
                    "Segoe UI",
                    10F);

            this.txtSearch.Location =
                new System.Drawing.Point(85, 19);

            this.txtSearch.Size =
                new System.Drawing.Size(420, 30);

            // SEARCH BUTTON

            this.btnSearch.BackColor =
                System.Drawing.Color.FromArgb(31, 41, 55);

            this.btnSearch.FlatAppearance.BorderSize = 0;

            this.btnSearch.FlatStyle =
                System.Windows.Forms.FlatStyle.Flat;

            this.btnSearch.Font =
                new System.Drawing.Font(
                    "Segoe UI Semibold",
                    9F,
                    System.Drawing.FontStyle.Bold);

            this.btnSearch.ForeColor =
                System.Drawing.Color.White;

            this.btnSearch.Location =
                new System.Drawing.Point(520, 18);

            this.btnSearch.Size =
                new System.Drawing.Size(105, 34);

            this.btnSearch.Text =
                "Search";

            this.btnSearch.UseVisualStyleBackColor = false;

            this.btnSearch.Click +=
                new System.EventHandler(this.btnSearch_Click);

            // REFRESH BUTTON

            this.btnRefresh.BackColor =
                System.Drawing.Color.FromArgb(75, 85, 99);

            this.btnRefresh.FlatAppearance.BorderSize = 0;

            this.btnRefresh.FlatStyle =
                System.Windows.Forms.FlatStyle.Flat;

            this.btnRefresh.Font =
                new System.Drawing.Font(
                    "Segoe UI Semibold",
                    9F,
                    System.Drawing.FontStyle.Bold);

            this.btnRefresh.ForeColor =
                System.Drawing.Color.White;

            this.btnRefresh.Location =
                new System.Drawing.Point(635, 18);

            this.btnRefresh.Size =
                new System.Drawing.Size(105, 34);

            this.btnRefresh.Text =
                "Refresh";

            this.btnRefresh.UseVisualStyleBackColor = false;

            this.btnRefresh.Click +=
                new System.EventHandler(this.btnRefresh_Click);

            // ============================================================
            // GRID PANEL
            // ============================================================

            this.pnlGrid.BackColor =
                System.Drawing.Color.White;

            this.pnlGrid.Dock =
                System.Windows.Forms.DockStyle.Fill;

            this.pnlGrid.Padding =
                new System.Windows.Forms.Padding(25, 15, 25, 25);

            this.pnlGrid.Controls.Add(this.dgvCategories);

            // ============================================================
            // DATAGRIDVIEW
            // ============================================================

            this.dgvCategories.AllowUserToAddRows = false;

            this.dgvCategories.AllowUserToDeleteRows = false;

            this.dgvCategories.AllowUserToResizeRows = false;

            this.dgvCategories.BackgroundColor =
                System.Drawing.Color.White;

            this.dgvCategories.BorderStyle =
                System.Windows.Forms.BorderStyle.None;

            this.dgvCategories.CellBorderStyle =
                System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;

            this.dgvCategories.ColumnHeadersBorderStyle =
                System.Windows.Forms.DataGridViewHeaderBorderStyle.None;

            this.dgvCategories.ColumnHeadersHeight = 42;

            this.dgvCategories.Dock =
                System.Windows.Forms.DockStyle.Fill;

            this.dgvCategories.EnableHeadersVisualStyles = false;

            this.dgvCategories.GridColor =
                System.Drawing.Color.FromArgb(229, 231, 235);

            this.dgvCategories.Location =
                new System.Drawing.Point(25, 15);

            this.dgvCategories.MultiSelect = false;

            this.dgvCategories.ReadOnly = true;

            this.dgvCategories.RowHeadersVisible = false;

            this.dgvCategories.RowTemplate.Height = 38;

            this.dgvCategories.SelectionMode =
                System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            // ============================================================
            // ADD CONTROLS TO FORM
            // ============================================================

            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.pnlSearch);
            this.Controls.Add(this.pnlEditor);
            this.Controls.Add(this.pnlHeader);

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();

            this.pnlEditor.ResumeLayout(false);
            this.pnlEditor.PerformLayout();

            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();

            this.pnlGrid.ResumeLayout(false);

            ((System.ComponentModel.ISupportInitialize)(this.dgvCategories)).EndInit();

            this.ResumeLayout(false);
        }
    }
}