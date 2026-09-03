using SkillHub.Models;
using SkillHub.Repositories;
using SkillHub.Utilities;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace SkillHub.Forms.Freelancer
{
    [DesignerCategory("Code")]
    public partial class FrmServiceEditor : Form
    {
        private readonly FreelancerServiceRepository _repository;
        private readonly int _freelancerId;
        private readonly int? _serviceId;
        private readonly bool _isEditMode;
        private string _existingImagePath;

        public FrmServiceEditor(int freelancerId)
        {
            InitializeComponent();

            _repository = new FreelancerServiceRepository();
            _freelancerId = freelancerId;
            _serviceId = null;
            _isEditMode = false;

            ConfigureForm();
            LoadCategories();
        }

        public FrmServiceEditor(int freelancerId, int serviceId)
        {
            InitializeComponent();

            _repository = new FreelancerServiceRepository();
            _freelancerId = freelancerId;
            _serviceId = serviceId;
            _isEditMode = true;

            ConfigureForm();
            LoadCategories();
            LoadService();
        }

        // ============================================================
        // FORM SETUP
        // ============================================================

        private void ConfigureForm()
        {
            Text = _isEditMode
                ? "SkillHub | Edit Service"
                : "SkillHub | Create Service";

            lblHeader.Text = _isEditMode
                ? "Edit Service"
                : "Create New Service";

            lblSubtitle.Text = _isEditMode
                ? "Update your existing software-service listing."
                : "Publish a new software-service listing.";

            btnSave.Text = _isEditMode
                ? "Save Changes"
                : "Publish Service";

            nudPrice.Minimum = 0;
            nudPrice.Maximum = 100000000;
            nudPrice.DecimalPlaces = 2;
            nudPrice.Increment = 100;

            nudDeliveryDays.Minimum = 1;
            nudDeliveryDays.Maximum = 365;

            nudAvailableSlots.Minimum = 0;
            nudAvailableSlots.Maximum = 100000;

            txtTitle.MaxLength = 150;
            txtDescription.MaxLength = 1500;

            cmbCategory.DropDownStyle =
                ComboBoxStyle.DropDownList;

            AcceptButton = btnSave;
            CancelButton = btnCancel;
        }

        // ============================================================
        // LOAD CATEGORIES
        // ============================================================

        private void LoadCategories()
        {
            try
            {
                DataTable categories =
                    _repository.GetCategories();

                cmbCategory.DataSource = null;

                cmbCategory.DisplayMember =
                    "CategoryName";

                cmbCategory.ValueMember =
                    "CategoryId";

                cmbCategory.DataSource =
                    categories;

                if (categories.Rows.Count > 0)
                {
                    cmbCategory.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load service categories.\n\n"
                    + ex.Message,
                    "SkillHub | Categories",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // LOAD EXISTING SERVICE
        // ============================================================

        private void LoadService()
        {
            if (!_serviceId.HasValue)
                return;

            try
            {
                var services =
                    _repository.GetByFreelancer(
                        _freelancerId);

                Service service =
                    services.Find(
                        s => s.ServiceId ==
                             _serviceId.Value);

                if (service == null)
                {
                    MessageBox.Show(
                        "The selected service could not be found.",
                        "SkillHub | Service",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    DialogResult =
                        DialogResult.Cancel;

                    Close();

                    return;
                }

                txtTitle.Text =
                    service.Title;

                txtDescription.Text =
                    service.Description;

                _existingImagePath =
                    service.ImagePath;

                nudPrice.Value =
                    service.Price;

                nudDeliveryDays.Value =
                    service.DeliveryDays;

                nudAvailableSlots.Value =
                    service.AvailableSlots;

                cmbCategory.SelectedValue =
                    service.CategoryId;

                lblServiceStatus.Text =
                    service.IsActive
                        ? "Currently Active"
                        : "Currently Inactive";

                lblServiceStatus.ForeColor =
                    service.IsActive
                        ? Color.SeaGreen
                        : Color.Firebrick;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load the service.\n\n"
                    + ex.Message,
                    "SkillHub | Service",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // VALIDATION
        // ============================================================

        private bool ValidateService()
        {
            string title =
                txtTitle.Text.Trim();

            string description =
                txtDescription.Text.Trim();

            if (title.Length < 3)
            {
                MessageBox.Show(
                    "Service title must contain at least 3 characters.",
                    "SkillHub | Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtTitle.Focus();

                return false;
            }

            if (description.Length < 10)
            {
                MessageBox.Show(
                    "Please provide a more detailed service description.",
                    "SkillHub | Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtDescription.Focus();

                return false;
            }

            if (cmbCategory.SelectedValue == null)
            {
                MessageBox.Show(
                    "Please select a service category.",
                    "SkillHub | Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cmbCategory.Focus();

                return false;
            }

            if (nudPrice.Value < 0)
            {
                MessageBox.Show(
                    "Price cannot be negative.",
                    "SkillHub | Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                nudPrice.Focus();

                return false;
            }

            if (nudDeliveryDays.Value <= 0)
            {
                MessageBox.Show(
                    "Delivery days must be greater than zero.",
                    "SkillHub | Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                nudDeliveryDays.Focus();

                return false;
            }

            if (nudAvailableSlots.Value < 0)
            {
                MessageBox.Show(
                    "Available slots cannot be negative.",
                    "SkillHub | Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                nudAvailableSlots.Focus();

                return false;
            }

            return true;
        }

        // ============================================================
        // BUILD MODEL
        // ============================================================

        private Service BuildService()
        {
            return new Service
            {
                ServiceId =
                    _serviceId ?? 0,

                FreelancerId =
                    _freelancerId,

                CategoryId =
                    Convert.ToInt32(
                        cmbCategory.SelectedValue),

                Title =
                    txtTitle.Text.Trim(),

                Description =
                    txtDescription.Text.Trim(),

                ImagePath =
                    _existingImagePath,

                Price =
                    nudPrice.Value,

                DeliveryDays =
                    Convert.ToInt32(
                        nudDeliveryDays.Value),

                AvailableSlots =
                    Convert.ToInt32(
                        nudAvailableSlots.Value)
            };
        }

        // ============================================================
        // SAVE
        // ============================================================

        private void btnSave_Click(
            object sender,
            EventArgs e)
        {
            if (!ValidateService())
                return;

            try
            {
                Service service =
                    BuildService();

                if (_isEditMode)
                {
                    _repository.Update(service);

                    MessageBox.Show(
                        "Service updated successfully.",
                        "SkillHub | Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    _repository.Add(service);

                    MessageBox.Show(
                        "Service published successfully.",
                        "SkillHub | Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                DialogResult =
                    DialogResult.OK;

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to save the service.\n\n"
                    + ex.Message,
                    "SkillHub | Save Service",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // CANCEL
        // ============================================================

        private void btnCancel_Click(
            object sender,
            EventArgs e)
        {
            DialogResult =
                DialogResult.Cancel;

            Close();
        }
    }
}
