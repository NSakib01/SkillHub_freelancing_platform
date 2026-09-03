using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using SkillHub.Repositories;
using SkillHub.UI;
using SkillHub.Utilities;

namespace SkillHub.Forms.Freelancer
{
    public partial class FrmFreelancerProfile : Form
    {
        private readonly int freelancerId;

        private readonly string connectionString =
    ConfigurationManager.ConnectionStrings["SkillHubConnection"].ConnectionString;

        private PictureBox profileImagePreview;
        private Button chooseImageButton;
        private Button removeImageButton;
        private string profileImagePath;
        private string pendingProfileImageSource;

        public FrmFreelancerProfile(int freelancerId)
        {
            InitializeComponent();

            ApplyModernLayout();

            this.freelancerId = freelancerId;

            LoadProfile();
        }

        private void LoadProfile()
        {
            const string query = @"
                SELECT
                    u.FullName,
                    u.Email,
                    u.Phone,
                    u.Address,
                    fp.ProfessionalTitle,
                    fp.Biography,
                    fp.Skills,
                    u.ProfileImagePath,
                    fp.IsVerified,
                    fp.AverageRating
                FROM dbo.Users u
                INNER JOIN dbo.FreelancerProfiles fp
                    ON fp.UserId = u.UserId
                INNER JOIN dbo.Roles r
                    ON r.RoleId = u.RoleId
                WHERE u.UserId = @UserId
                  AND r.RoleName = N'Freelancer';";

            try
            {
                using (SqlConnection connection =
                    new SqlConnection(connectionString))
                {
                    using (SqlCommand command =
                        new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue(
                            "@UserId",
                            freelancerId);

                        connection.Open();

                        using (SqlDataReader reader =
                            command.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                MessageBox.Show(
                                    "Freelancer profile was not found.",
                                    "Profile",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);

                                return;
                            }

                            txtFullName.Text =
                                reader["FullName"] == DBNull.Value
                                    ? ""
                                    : reader["FullName"].ToString();

                            txtEmail.Text =
                                reader["Email"] == DBNull.Value
                                    ? ""
                                    : reader["Email"].ToString();

                            txtPhone.Text =
                                reader["Phone"] == DBNull.Value
                                    ? ""
                                    : reader["Phone"].ToString();

                            txtAddress.Text =
                                reader["Address"] == DBNull.Value
                                    ? ""
                                    : reader["Address"].ToString();

                            txtProfessionalTitle.Text =
                                reader["ProfessionalTitle"] == DBNull.Value
                                    ? ""
                                    : reader["ProfessionalTitle"].ToString();

                            txtBiography.Text =
                                reader["Biography"] == DBNull.Value
                                    ? ""
                                    : reader["Biography"].ToString();

                            txtSkills.Text =
                                reader["Skills"] == DBNull.Value
                                    ? ""
                                    : reader["Skills"].ToString();

                            profileImagePath =
                                reader["ProfileImagePath"] == DBNull.Value
                                    ? string.Empty
                                    : reader["ProfileImagePath"].ToString();

                            pendingProfileImageSource = null;
                            ShowProfileImagePreview();

                            bool isVerified =
                                reader["IsVerified"] != DBNull.Value &&
                                Convert.ToBoolean(reader["IsVerified"]);

                            decimal rating =
                                reader["AverageRating"] == DBNull.Value
                                    ? 0
                                    : Convert.ToDecimal(
                                        reader["AverageRating"]);

                            lblVerifiedValue.Text =
                                isVerified
                                    ? "Verified"
                                    : "Not Verified";

                            lblRatingValue.Text =
                                rating.ToString("0.00");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load freelancer profile.\n\n" +
                    ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateProfile())
            {
                return;
            }

            const string updateUsers = @"
                UPDATE dbo.Users
                SET
                    FullName = @FullName,
                    Email = @Email,
                    Phone = @Phone,
                    Address = @Address,
                    ProfileImagePath = @ProfileImagePath,
                    UpdatedAt = SYSDATETIME()
                WHERE UserId = @UserId;";

            const string updateProfile = @"
                UPDATE dbo.FreelancerProfiles
                SET
                    ProfessionalTitle = @ProfessionalTitle,
                    Biography = @Biography,
                    Skills = @Skills,
                    UpdatedAt = SYSDATETIME()
                WHERE UserId = @UserId;";

            try
            {
                string savedImagePath = PrepareProfileImagePath();

                using (SqlConnection connection =
                    new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlTransaction transaction =
                        connection.BeginTransaction())
                    {
                        try
                        {
                            using (SqlCommand command =
                                new SqlCommand(
                                    updateUsers,
                                    connection,
                                    transaction))
                            {
                                command.Parameters.AddWithValue(
                                    "@FullName",
                                    txtFullName.Text.Trim());

                                command.Parameters.AddWithValue(
                                    "@Email",
                                    txtEmail.Text.Trim());

                                command.Parameters.AddWithValue(
                                    "@Phone",
                                    string.IsNullOrWhiteSpace(txtPhone.Text)
                                        ? (object)DBNull.Value
                                        : txtPhone.Text.Trim());

                                command.Parameters.AddWithValue(
                                    "@Address",
                                    string.IsNullOrWhiteSpace(txtAddress.Text)
                                        ? (object)DBNull.Value
                                        : txtAddress.Text.Trim());

                                command.Parameters.Add(
                                    "@ProfileImagePath",
                                    System.Data.SqlDbType.NVarChar,
                                    300).Value =
                                    string.IsNullOrWhiteSpace(savedImagePath)
                                        ? (object)DBNull.Value
                                        : savedImagePath;

                                command.Parameters.AddWithValue(
                                    "@UserId",
                                    freelancerId);

                                command.ExecuteNonQuery();
                            }

                            using (SqlCommand command =
                                new SqlCommand(
                                    updateProfile,
                                    connection,
                                    transaction))
                            {
                                command.Parameters.AddWithValue(
                                    "@ProfessionalTitle",
                                    string.IsNullOrWhiteSpace(
                                        txtProfessionalTitle.Text)
                                        ? (object)DBNull.Value
                                        : txtProfessionalTitle.Text.Trim());

                                command.Parameters.AddWithValue(
                                    "@Biography",
                                    string.IsNullOrWhiteSpace(
                                        txtBiography.Text)
                                        ? (object)DBNull.Value
                                        : txtBiography.Text.Trim());

                                command.Parameters.AddWithValue(
                                    "@Skills",
                                    string.IsNullOrWhiteSpace(
                                        txtSkills.Text)
                                        ? (object)DBNull.Value
                                        : txtSkills.Text.Trim());

                                command.Parameters.AddWithValue(
                                    "@UserId",
                                    freelancerId);

                                command.ExecuteNonQuery();
                            }

                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }

                MessageBox.Show(
                    "Profile updated successfully.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                if (UserSession.IsAuthenticated && UserSession.UserId == freelancerId)
                {
                    UserSession.Refresh(new UserRepository().GetById(freelancerId));
                }

                LoadProfile();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    MessageBox.Show(
                        "This email address is already being used by another account.",
                        "Duplicate Email",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show(
                        "Unable to update profile.\n\n" +
                        ex.Message,
                        "Database Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to update profile.\n\n" +
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private bool ValidateProfile()
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show(
                    "Full name is required.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtFullName.Focus();
                return false;
            }

            if (txtFullName.Text.Trim().Length < 2)
            {
                MessageBox.Show(
                    "Full name must contain at least 2 characters.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtFullName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show(
                    "Email is required.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtEmail.Focus();
                return false;
            }

            if (!txtEmail.Text.Contains("@") ||
                !txtEmail.Text.Contains("."))
            {
                MessageBox.Show(
                    "Please enter a valid email address.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtEmail.Focus();
                return false;
            }

            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            LoadProfile();
        }

        private void ApplyModernLayout()
        {
            ClientSize = new Size(1080, 680);
            MinimumSize = new Size(1100, 720);
            BackColor = MarketplaceTheme.PageBackground;
            Font = MarketplaceTheme.Body();

            lblTitle.Location = new Point(270, 27);
            lblTitle.ForeColor = MarketplaceTheme.DeepCharcoal;
            lblTitle.Text = "Build Your Freelancer Profile";

            Label[] fieldLabels =
            {
                lblFullName, lblEmail, lblPhone, lblAddress,
                lblProfessionalTitle, lblBiography, lblSkills
            };

            foreach (Label label in fieldLabels)
            {
                label.Left = 270;
                label.Font = MarketplaceTheme.SubHeading(9.5F);
                label.ForeColor = MarketplaceTheme.MutedText;
            }

            TextBox[] fields =
            {
                txtFullName, txtEmail, txtPhone, txtAddress,
                txtProfessionalTitle, txtBiography, txtSkills
            };

            foreach (TextBox field in fields)
            {
                field.Left = 430;
                field.Width = 600;
                field.Font = MarketplaceTheme.Body(10F);
                field.BackColor = Color.White;
                field.BorderStyle = BorderStyle.FixedSingle;
            }

            RoundedPanel visualCard = new RoundedPanel
            {
                Location = new Point(30, 88),
                Size = new Size(210, 500),
                BackColor = Color.White,
                CornerRadius = 18,
                BorderThickness = 0
            };

            profileImagePreview = new PictureBox
            {
                Location = new Point(29, 28),
                Size = new Size(152, 152),
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            chooseImageButton = new Button
            {
                Text = "Choose Photo",
                Location = new Point(29, 198),
                Size = new Size(152, 38),
                BackColor = MarketplaceTheme.Primary,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            chooseImageButton.FlatAppearance.BorderSize = 0;
            chooseImageButton.Click += ChooseImageButtonClick;

            removeImageButton = new Button
            {
                Text = "Remove Photo",
                Location = new Point(29, 245),
                Size = new Size(152, 34),
                BackColor = Color.White,
                ForeColor = MarketplaceTheme.MutedText,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            removeImageButton.FlatAppearance.BorderColor = MarketplaceTheme.Border;
            removeImageButton.Click += RemoveImageButtonClick;

            lblVerified.Location = new Point(29, 313);
            lblVerified.Font = MarketplaceTheme.Body(8.5F);
            lblVerified.ForeColor = MarketplaceTheme.MutedText;
            lblVerifiedValue.Location = new Point(29, 338);
            lblVerifiedValue.Font = MarketplaceTheme.SubHeading(10F);
            lblVerifiedValue.ForeColor = MarketplaceTheme.Success;

            lblRating.Location = new Point(29, 386);
            lblRating.Font = MarketplaceTheme.Body(8.5F);
            lblRating.ForeColor = MarketplaceTheme.MutedText;
            lblRatingValue.Location = new Point(29, 411);
            lblRatingValue.Font = MarketplaceTheme.Heading(16F);
            lblRatingValue.ForeColor = MarketplaceTheme.Warning;

            visualCard.Controls.Add(profileImagePreview);
            visualCard.Controls.Add(chooseImageButton);
            visualCard.Controls.Add(removeImageButton);
            visualCard.Controls.Add(lblVerified);
            visualCard.Controls.Add(lblVerifiedValue);
            visualCard.Controls.Add(lblRating);
            visualCard.Controls.Add(lblRatingValue);
            Controls.Add(visualCard);

            btnSave.Location = new Point(820, 585);
            btnSave.Size = new Size(100, 40);
            btnSave.BackColor = MarketplaceTheme.Primary;
            btnSave.ForeColor = Color.White;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Font = MarketplaceTheme.SubHeading(9.5F);
            btnSave.Cursor = Cursors.Hand;

            btnCancel.Location = new Point(930, 585);
            btnCancel.Size = new Size(100, 40);
            btnCancel.BackColor = Color.White;
            btnCancel.ForeColor = MarketplaceTheme.Primary;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.FlatAppearance.BorderColor = MarketplaceTheme.Border;
            btnCancel.Font = MarketplaceTheme.SubHeading(9.5F);
            btnCancel.Cursor = Cursors.Hand;

            ShowProfileImagePreview();
        }

        private void ChooseImageButtonClick(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "Choose your profile photo";
                dialog.Filter = "Image files|*.png;*.jpg;*.jpeg;*.bmp|All files|*.*";
                dialog.CheckFileExists = true;

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    pendingProfileImageSource = dialog.FileName;
                    ShowProfileImagePreview();
                }
            }
        }

        private void RemoveImageButtonClick(object sender, EventArgs e)
        {
            profileImagePath = null;
            pendingProfileImageSource = null;
            ShowProfileImagePreview();
        }

        private string PrepareProfileImagePath()
        {
            if (!string.IsNullOrWhiteSpace(pendingProfileImageSource))
            {
                profileImagePath = ImageAssetHelper.ImportUserImage(
                    pendingProfileImageSource,
                    "freelancer-" + freelancerId);
                pendingProfileImageSource = null;
            }

            return profileImagePath;
        }

        private void ShowProfileImagePreview()
        {
            if (profileImagePreview == null)
            {
                return;
            }

            if (profileImagePreview.Image != null)
            {
                profileImagePreview.Image.Dispose();
            }

            string previewPath = !string.IsNullOrWhiteSpace(pendingProfileImageSource)
                ? pendingProfileImageSource
                : profileImagePath;

            profileImagePreview.Image = ImageAssetHelper.LoadAvatar(
                previewPath,
                txtFullName == null ? "SkillHub Freelancer" : txtFullName.Text,
                profileImagePreview.Width);
        }
    }
}
