using SkillHub.Data;
using SkillHub.Forms.Common;
using SkillHub.Models;
using SkillHub.Services;
using SkillHub.Utilities;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SkillHub.Forms.Admin
{
    public partial class FrmManageCategories : Form
    {
        private readonly AuthenticationService Authentication;
        private readonly DatabaseConnection Database;

        private int selectedCategoryId = 0;

        public FrmManageCategories(AuthenticationService authentication)
        {
            InitializeComponent();

            Authentication = authentication;
            Database = new DatabaseConnection();

            AuthorizationService.DemandRole(UserRoles.Admin);

            ConfigureGrid();
            LoadCategories();

            UiFactory.AddBackToDashboardButton(this);
        }

        private void ConfigureGrid()
        {
            dgvCategories.AutoGenerateColumns = true;
            dgvCategories.ReadOnly = true;
            dgvCategories.AllowUserToAddRows = false;
            dgvCategories.AllowUserToDeleteRows = false;
            dgvCategories.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCategories.MultiSelect = false;
            dgvCategories.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvCategories.CellClick += dgvCategories_CellClick;
        }

        private void LoadCategories()
        {
            try
            {
                using (SqlConnection connection = Database.OpenConnection())
                using (SqlCommand command = new SqlCommand(
                    "SELECT CategoryId, CategoryName, IsActive, CreatedAt " +
                    "FROM dbo.Categories " +
                    "ORDER BY CategoryId DESC;",
                    connection))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    dgvCategories.DataSource = table;

                    if (dgvCategories.Columns.Contains("CategoryId"))
                    {
                        dgvCategories.Columns["CategoryId"].HeaderText = "ID";
                    }

                    if (dgvCategories.Columns.Contains("CategoryName"))
                    {
                        dgvCategories.Columns["CategoryName"].HeaderText = "Category Name";
                    }

                    if (dgvCategories.Columns.Contains("IsActive"))
                    {
                        dgvCategories.Columns["IsActive"].HeaderText = "Active";
                    }

                    if (dgvCategories.Columns.Contains("CreatedAt"))
                    {
                        dgvCategories.Columns["CreatedAt"].HeaderText = "Created At";
                    }
                }
            }
            catch (Exception exception)
            {
                UiFactory.ShowError(this, exception);
            }
        }

        private void SearchCategories()
        {
            try
            {
                string searchText = txtSearch.Text.Trim();

                using (SqlConnection connection = Database.OpenConnection())
                using (SqlCommand command = new SqlCommand(
                    "SELECT CategoryId, CategoryName, IsActive, CreatedAt " +
                    "FROM dbo.Categories " +
                    "WHERE CategoryName LIKE @Search " +
                    "ORDER BY CategoryId DESC;",
                    connection))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DatabaseConnection.AddParameter(
                        command,
                        "@Search",
                        SqlDbType.NVarChar,
                        "%" + searchText + "%",
                        150);

                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    dgvCategories.DataSource = table;
                }
            }
            catch (Exception exception)
            {
                UiFactory.ShowError(this, exception);
            }
        }

        private void AddCategory()
        {
            string categoryName = txtCategoryName.Text.Trim();

            if (string.IsNullOrWhiteSpace(categoryName))
            {
                MessageBox.Show(
                    "Please enter a category name.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtCategoryName.Focus();
                return;
            }

            try
            {
                using (SqlConnection connection = Database.OpenConnection())
                using (SqlCommand command = new SqlCommand(
                    "IF EXISTS " +
                    "(SELECT 1 FROM dbo.Categories WHERE CategoryName = @CategoryName) " +
                    "BEGIN " +
                    "    SELECT 1; " +
                    "END " +
                    "ELSE " +
                    "BEGIN " +
                    "    INSERT INTO dbo.Categories (CategoryName, IsActive, CreatedAt) " +
                    "    VALUES (@CategoryName, 1, GETDATE()); " +
                    "    SELECT 0; " +
                    "END;",
                    connection))
                {
                    DatabaseConnection.AddParameter(
                        command,
                        "@CategoryName",
                        SqlDbType.NVarChar,
                        categoryName,
                        150);

                    int result = Convert.ToInt32(command.ExecuteScalar());

                    if (result == 1)
                    {
                        MessageBox.Show(
                            "This category already exists.",
                            "Duplicate Category",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        return;
                    }
                }

                MessageBox.Show(
                    "Category added successfully.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                ClearForm();
                LoadCategories();
            }
            catch (Exception exception)
            {
                UiFactory.ShowError(this, exception);
            }
        }

        private void UpdateCategory()
        {
            if (selectedCategoryId <= 0)
            {
                MessageBox.Show(
                    "Please select a category from the table first.",
                    "Select Category",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            string categoryName = txtCategoryName.Text.Trim();

            if (string.IsNullOrWhiteSpace(categoryName))
            {
                MessageBox.Show(
                    "Please enter a category name.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtCategoryName.Focus();
                return;
            }

            try
            {
                using (SqlConnection connection = Database.OpenConnection())
                using (SqlCommand command = new SqlCommand(
                    "IF EXISTS " +
                    "(SELECT 1 FROM dbo.Categories " +
                    " WHERE CategoryName = @CategoryName " +
                    " AND CategoryId <> @CategoryId) " +
                    "BEGIN " +
                    "    SELECT 1; " +
                    "END " +
                    "ELSE " +
                    "BEGIN " +
                    "    UPDATE dbo.Categories " +
                    "    SET CategoryName = @CategoryName " +
                    "    WHERE CategoryId = @CategoryId; " +
                    "    SELECT 0; " +
                    "END;",
                    connection))
                {
                    DatabaseConnection.AddParameter(
                        command,
                        "@CategoryName",
                        SqlDbType.NVarChar,
                        categoryName,
                        150);

                    DatabaseConnection.AddParameter(
                        command,
                        "@CategoryId",
                        SqlDbType.Int,
                        selectedCategoryId);

                    int result = Convert.ToInt32(command.ExecuteScalar());

                    if (result == 1)
                    {
                        MessageBox.Show(
                            "Another category already uses this name.",
                            "Duplicate Category",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        return;
                    }
                }

                MessageBox.Show(
                    "Category updated successfully.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                ClearForm();
                LoadCategories();
            }
            catch (Exception exception)
            {
                UiFactory.ShowError(this, exception);
            }
        }

        private void DeactivateCategory()
        {
            if (selectedCategoryId <= 0)
            {
                MessageBox.Show(
                    "Please select a category first.",
                    "Select Category",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            DialogResult confirmation = MessageBox.Show(
                "Are you sure you want to deactivate this category?",
                "Confirm Deactivation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmation != DialogResult.Yes)
            {
                return;
            }

            try
            {
                using (SqlConnection connection = Database.OpenConnection())
                using (SqlCommand command = new SqlCommand(
                    "UPDATE dbo.Categories " +
                    "SET IsActive = 0 " +
                    "WHERE CategoryId = @CategoryId;",
                    connection))
                {
                    DatabaseConnection.AddParameter(
                        command,
                        "@CategoryId",
                        SqlDbType.Int,
                        selectedCategoryId);

                    command.ExecuteNonQuery();
                }

                MessageBox.Show(
                    "Category deactivated successfully.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                ClearForm();
                LoadCategories();
            }
            catch (Exception exception)
            {
                UiFactory.ShowError(this, exception);
            }
        }

        private void ClearForm()
        {
            selectedCategoryId = 0;

            txtCategoryName.Clear();
            txtSearch.Clear();

            dgvCategories.ClearSelection();

            txtCategoryName.Focus();
        }

        private void dgvCategories_CellClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            DataGridViewRow row = dgvCategories.Rows[e.RowIndex];

            if (row.Cells["CategoryId"].Value != null)
            {
                selectedCategoryId =
                    Convert.ToInt32(row.Cells["CategoryId"].Value);
            }

            if (row.Cells["CategoryName"].Value != null)
            {
                txtCategoryName.Text =
                    Convert.ToString(row.Cells["CategoryName"].Value);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddCategory();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateCategory();
        }

        private void btnDeactivate_Click(object sender, EventArgs e)
        {
            DeactivateCategory();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchCategories();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadCategories();
        }
    }
}