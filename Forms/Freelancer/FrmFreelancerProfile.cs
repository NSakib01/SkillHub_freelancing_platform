using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SkillHub.Forms.Freelancer
{
    public partial class FrmFreelancerProfile : Form
    {
        private readonly int freelancerId;

        private readonly string connectionString =
    ConfigurationManager.ConnectionStrings["SkillHubConnection"].ConnectionString;

        public FrmFreelancerProfile(int freelancerId)
        {
            InitializeComponent();

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
    }
}