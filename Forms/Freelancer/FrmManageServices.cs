using SkillHub.Models;
using SkillHub.Repositories;
using SkillHub.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SkillHub.Forms.Freelancer
{
    [DesignerCategory("Code")]
    public partial class FrmManageServices : Form
    {
        // ============================================================
        // REPOSITORY
        // ============================================================

        private readonly FreelancerServiceRepository _serviceRepository;

        // ============================================================
        // STATE
        // ============================================================

        private int _selectedServiceId = 0;

        // ============================================================
        // CONSTRUCTOR
        // ============================================================

        public FrmManageServices()
        {
            InitializeComponent();

            _serviceRepository =
                new FreelancerServiceRepository();

            if (!IsDesignMode())
            {
                Load += FrmManageServices_Load;
            }
        }

        // ============================================================
        // DESIGN MODE
        // ============================================================

        private bool IsDesignMode()
        {
            return LicenseManager.UsageMode ==
                       LicenseUsageMode.Designtime
                   || DesignMode;
        }

        // ============================================================
        // FORM LOAD
        // ============================================================

        private void FrmManageServices_Load(
            object sender,
            EventArgs e)
        {
            try
            {
                if (!UserSession.IsAuthenticated)
                {
                    MessageBox.Show(
                        "You must be signed in to manage your services.",
                        "Authentication Required",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    Close();
                    return;
                }

                if (UserSession.RoleName != UserRoles.Freelancer)
                {
                    MessageBox.Show(
                        "Only freelancers can access service management.",
                        "Access Denied",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    Close();
                    return;
                }

                LoadCategories();
                LoadServices();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load the service management screen.\n\n"
                    + ex.Message,
                    "Service Management Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // LOAD CATEGORIES
        // ============================================================

        private void LoadCategories()
        {
            try
            {
                cmbCategory.DataSource = null;

                cmbCategory.DataSource =
                    _serviceRepository.GetCategories();

                cmbCategory.DisplayMember =
                    "CategoryName";

                cmbCategory.ValueMember =
                    "CategoryId";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load service categories.\n\n"
                    + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // LOAD SERVICES
        // ============================================================

        private void LoadServices()
        {
            try
            {
                List<Service> services =
                    _serviceRepository.GetByFreelancer(
                        UserSession.UserId);

                dgvServices.DataSource = null;
                dgvServices.DataSource = services;

                lblServiceCount.Text =
                    services.Count + " service(s)";

                ClearEditor(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load your services.\n\n"
                    + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // DATAGRIDVIEW CLICK
        // ============================================================

        private void dgvServices_CellClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            if (dgvServices.Rows[e.RowIndex].DataBoundItem
                is Service service)
            {
                LoadServiceIntoEditor(service);
            }
        }

        // ============================================================
        // LOAD SELECTED SERVICE INTO EDITOR
        // ============================================================

        private void LoadServiceIntoEditor(
            Service service)
        {
            _selectedServiceId =
                service.ServiceId;

            txtTitle.Text =
                service.Title ?? string.Empty;

            txtDescription.Text =
                service.Description ?? string.Empty;

            cmbCategory.SelectedValue =
                service.CategoryId;

            if (service.Price >= nudPrice.Minimum &&
                service.Price <= nudPrice.Maximum)
            {
                nudPrice.Value =
                    service.Price;
            }

            if (service.DeliveryDays >=
                    nudDeliveryDays.Minimum &&
                service.DeliveryDays <=
                    nudDeliveryDays.Maximum)
            {
                nudDeliveryDays.Value =
                    service.DeliveryDays;
            }

            if (service.AvailableSlots >=
                    nudAvailableSlots.Minimum &&
                service.AvailableSlots <=
                    nudAvailableSlots.Maximum)
            {
                nudAvailableSlots.Value =
                    service.AvailableSlots;
            }

            chkActive.Checked =
                service.IsActive;

            lblSelectedService.Text =
                "Selected Service #" +
                service.ServiceId;

            lblMode.Text =
                "Editing Service";

            btnAdd.Enabled = false;
            btnUpdate.Enabled = true;
            btnToggleStatus.Enabled = true;
            btnDelete.Enabled = true;
        }

        // ============================================================
        // ADD SERVICE
        // ============================================================

        private void btnAdd_Click(
            object sender,
            EventArgs e)
        {
            if (!ValidateInput())
            {
                return;
            }

            try
            {
                Service service =
                    BuildServiceFromEditor();

                service.ServiceId = 0;

                _serviceRepository.Add(service);

                MessageBox.Show(
                    "Your service has been published successfully.",
                    "Service Published",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadServices();
                ClearEditor();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to publish the service.\n\n"
                    + ex.Message,
                    "Publish Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // UPDATE SERVICE
        // ============================================================

        private void btnUpdate_Click(
            object sender,
            EventArgs e)
        {
            if (_selectedServiceId == 0)
            {
                MessageBox.Show(
                    "Please select a service first.",
                    "No Service Selected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (!ValidateInput())
            {
                return;
            }

            try
            {
                Service service =
                    BuildServiceFromEditor();

                service.ServiceId =
                    _selectedServiceId;

                _serviceRepository.Update(service);

                MessageBox.Show(
                    "Your service has been updated successfully.",
                    "Service Updated",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadServices();
                ClearEditor();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to update the service.\n\n"
                    + ex.Message,
                    "Update Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // ACTIVATE / DEACTIVATE
        // ============================================================

        private void btnToggleStatus_Click(
            object sender,
            EventArgs e)
        {
            if (_selectedServiceId == 0)
            {
                MessageBox.Show(
                    "Please select a service first.",
                    "No Service Selected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            try
            {
                Service selectedService =
                    GetSelectedService();

                if (selectedService == null)
                {
                    return;
                }

                bool newStatus =
                    !selectedService.IsActive;

                string action =
                    newStatus
                        ? "activate"
                        : "deactivate";

                DialogResult result =
                    MessageBox.Show(
                        "Are you sure you want to "
                        + action
                        + " this service?",
                        "Confirm Status Change",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                {
                    return;
                }

                _serviceRepository.SetActive(
                    selectedService.ServiceId,
                    UserSession.UserId,
                    newStatus);

                MessageBox.Show(
                    newStatus
                        ? "Service activated successfully."
                        : "Service deactivated successfully.",
                    "Status Updated",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadServices();
                ClearEditor();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to change the service status.\n\n"
                    + ex.Message,
                    "Status Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // DELETE SERVICE
        // ============================================================

        private void btnDelete_Click(
            object sender,
            EventArgs e)
        {
            if (_selectedServiceId == 0)
            {
                MessageBox.Show(
                    "Please select a service first.",
                    "No Service Selected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            DialogResult result =
                MessageBox.Show(
                    "Are you sure you want to delete this service?\n\n"
                    + "This operation cannot be undone.",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
            {
                return;
            }

            try
            {
                _serviceRepository.Delete(
                    _selectedServiceId,
                    UserSession.UserId);

                MessageBox.Show(
                    "Service deleted successfully.",
                    "Service Deleted",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadServices();
                ClearEditor();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to delete the service.\n\n"
                    + ex.Message,
                    "Delete Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // CLEAR
        // ============================================================

        private void btnClear_Click(
            object sender,
            EventArgs e)
        {
            ClearEditor();
        }

        private void ClearEditor(
            bool clearGrid = true)
        {
            _selectedServiceId = 0;

            txtTitle.Clear();
            txtDescription.Clear();

            nudPrice.Value = 0;
            nudDeliveryDays.Value = 1;
            nudAvailableSlots.Value = 1;

            chkActive.Checked = true;

            if (cmbCategory.Items.Count > 0)
            {
                cmbCategory.SelectedIndex = 0;
            }

            lblSelectedService.Text =
                "No service selected";

            lblMode.Text =
                "Create New Service";

            btnAdd.Enabled = true;
            btnUpdate.Enabled = false;
            btnToggleStatus.Enabled = false;
            btnDelete.Enabled = false;

            if (clearGrid &&
                dgvServices != null)
            {
                dgvServices.ClearSelection();
            }
        }

        // ============================================================
        // REFRESH
        // ============================================================

        private void btnRefresh_Click(
            object sender,
            EventArgs e)
        {
            LoadCategories();
            LoadServices();
            ClearEditor();
        }

        // ============================================================
        // VALIDATION
        // ============================================================

        private bool ValidateInput()
        {
            string title =
                txtTitle.Text.Trim();

            string description =
                txtDescription.Text.Trim();

            if (string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show(
                    "Please enter a service title.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtTitle.Focus();
                return false;
            }

            if (title.Length < 3)
            {
                MessageBox.Show(
                    "Service title must contain at least 3 characters.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtTitle.Focus();
                return false;
            }

            if (title.Length > 150)
            {
                MessageBox.Show(
                    "Service title cannot exceed 150 characters.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtTitle.Focus();
                return false;
            }

            if (cmbCategory.SelectedValue == null)
            {
                MessageBox.Show(
                    "Please select a service category.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cmbCategory.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                MessageBox.Show(
                    "Please enter a service description.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtDescription.Focus();
                return false;
            }

            if (description.Length > 1500)
            {
                MessageBox.Show(
                    "Service description cannot exceed 1500 characters.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtDescription.Focus();
                return false;
            }

            if (nudPrice.Value < 0)
            {
                MessageBox.Show(
                    "Price cannot be negative.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                nudPrice.Focus();
                return false;
            }

            if (nudDeliveryDays.Value <= 0)
            {
                MessageBox.Show(
                    "Delivery days must be greater than zero.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                nudDeliveryDays.Focus();
                return false;
            }

            if (nudAvailableSlots.Value < 0)
            {
                MessageBox.Show(
                    "Available slots cannot be negative.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                nudAvailableSlots.Focus();
                return false;
            }

            return true;
        }

        // ============================================================
        // BUILD SERVICE MODEL
        // ============================================================

        private Service BuildServiceFromEditor()
        {
            int categoryId =
                Convert.ToInt32(
                    cmbCategory.SelectedValue);

            return new Service
            {
                ServiceId =
                    _selectedServiceId,

                FreelancerId =
                    UserSession.UserId,

                CategoryId =
                    categoryId,

                Title =
                    txtTitle.Text.Trim(),

                Description =
                    txtDescription.Text.Trim(),

                Price =
                    nudPrice.Value,

                DeliveryDays =
                    Convert.ToInt32(
                        nudDeliveryDays.Value),

                AvailableSlots =
                    Convert.ToInt32(
                        nudAvailableSlots.Value),

                IsActive =
                    chkActive.Checked
            };
        }

        // ============================================================
        // GET SELECTED SERVICE
        // ============================================================

        private Service GetSelectedService()
        {
            if (dgvServices.CurrentRow == null)
            {
                return null;
            }

            return dgvServices
                .CurrentRow
                .DataBoundItem as Service;
        }
    }
}