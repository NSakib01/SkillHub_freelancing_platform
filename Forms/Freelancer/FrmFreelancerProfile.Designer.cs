namespace SkillHub.Forms.Freelancer
{
    partial class FrmFreelancerProfile
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblFullName;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.Label lblProfessionalTitle;
        private System.Windows.Forms.Label lblBiography;
        private System.Windows.Forms.Label lblSkills;
        private System.Windows.Forms.Label lblVerified;
        private System.Windows.Forms.Label lblRating;

        private System.Windows.Forms.TextBox txtFullName;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.TextBox txtProfessionalTitle;
        private System.Windows.Forms.TextBox txtBiography;
        private System.Windows.Forms.TextBox txtSkills;

        private System.Windows.Forms.Label lblVerifiedValue;
        private System.Windows.Forms.Label lblRatingValue;

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
            this.lblTitle = new System.Windows.Forms.Label();

            this.lblFullName = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblPhone = new System.Windows.Forms.Label();
            this.lblAddress = new System.Windows.Forms.Label();
            this.lblProfessionalTitle = new System.Windows.Forms.Label();
            this.lblBiography = new System.Windows.Forms.Label();
            this.lblSkills = new System.Windows.Forms.Label();
            this.lblVerified = new System.Windows.Forms.Label();
            this.lblRating = new System.Windows.Forms.Label();

            this.txtFullName = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.txtProfessionalTitle = new System.Windows.Forms.TextBox();
            this.txtBiography = new System.Windows.Forms.TextBox();
            this.txtSkills = new System.Windows.Forms.TextBox();

            this.lblVerifiedValue = new System.Windows.Forms.Label();
            this.lblRatingValue = new System.Windows.Forms.Label();

            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();

            this.SuspendLayout();

            // Form
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(850, 650);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SkillHub | Freelancer Profile";
            this.BackColor = System.Drawing.Color.White;

            // Title
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font(
                "Segoe UI",
                20F,
                System.Drawing.FontStyle.Bold);

            this.lblTitle.Location = new System.Drawing.Point(35, 25);
            this.lblTitle.Text = "Freelancer Profile";

            // Full Name
            this.lblFullName.AutoSize = true;
            this.lblFullName.Location = new System.Drawing.Point(40, 90);
            this.lblFullName.Text = "Full Name";

            this.txtFullName.Location = new System.Drawing.Point(200, 87);
            this.txtFullName.Size = new System.Drawing.Size(560, 27);

            // Email
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(40, 135);
            this.lblEmail.Text = "Email";

            this.txtEmail.Location = new System.Drawing.Point(200, 132);
            this.txtEmail.Size = new System.Drawing.Size(560, 27);

            // Phone
            this.lblPhone.AutoSize = true;
            this.lblPhone.Location = new System.Drawing.Point(40, 180);
            this.lblPhone.Text = "Phone";

            this.txtPhone.Location = new System.Drawing.Point(200, 177);
            this.txtPhone.Size = new System.Drawing.Size(560, 27);

            // Address
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new System.Drawing.Point(40, 225);
            this.lblAddress.Text = "Address";

            this.txtAddress.Location = new System.Drawing.Point(200, 222);
            this.txtAddress.Size = new System.Drawing.Size(560, 27);

            // Professional Title
            this.lblProfessionalTitle.AutoSize = true;
            this.lblProfessionalTitle.Location = new System.Drawing.Point(40, 270);
            this.lblProfessionalTitle.Text = "Professional Title";

            this.txtProfessionalTitle.Location =
                new System.Drawing.Point(200, 267);

            this.txtProfessionalTitle.Size =
                new System.Drawing.Size(560, 27);

            // Biography
            this.lblBiography.AutoSize = true;
            this.lblBiography.Location = new System.Drawing.Point(40, 315);
            this.lblBiography.Text = "Biography";

            this.txtBiography.Location =
                new System.Drawing.Point(200, 312);

            this.txtBiography.Multiline = true;
            this.txtBiography.ScrollBars =
                System.Windows.Forms.ScrollBars.Vertical;

            this.txtBiography.Size =
                new System.Drawing.Size(560, 80);

            // Skills
            this.lblSkills.AutoSize = true;
            this.lblSkills.Location = new System.Drawing.Point(40, 420);
            this.lblSkills.Text = "Skills";

            this.txtSkills.Location =
                new System.Drawing.Point(200, 417);

            this.txtSkills.Multiline = true;
            this.txtSkills.Size =
                new System.Drawing.Size(560, 55);

            // Verified
            this.lblVerified.AutoSize = true;
            this.lblVerified.Location = new System.Drawing.Point(40, 495);
            this.lblVerified.Text = "Verification";

            this.lblVerifiedValue.AutoSize = true;
            this.lblVerifiedValue.Location =
                new System.Drawing.Point(200, 495);

            // Rating
            this.lblRating.AutoSize = true;
            this.lblRating.Location = new System.Drawing.Point(40, 525);
            this.lblRating.Text = "Average Rating";

            this.lblRatingValue.AutoSize = true;
            this.lblRatingValue.Location =
                new System.Drawing.Point(200, 525);

            // Save
            this.btnSave.Location =
                new System.Drawing.Point(560, 565);

            this.btnSave.Size =
                new System.Drawing.Size(95, 35);

            this.btnSave.Text = "Save";

            this.btnSave.UseVisualStyleBackColor = true;

            this.btnSave.Click +=
                new System.EventHandler(this.btnSave_Click);

            // Cancel
            this.btnCancel.Location =
                new System.Drawing.Point(665, 565);

            this.btnCancel.Size =
                new System.Drawing.Size(95, 35);

            this.btnCancel.Text = "Reload";

            this.btnCancel.UseVisualStyleBackColor = true;

            this.btnCancel.Click +=
                new System.EventHandler(this.btnCancel_Click);

            // Add controls
            this.Controls.Add(this.lblTitle);

            this.Controls.Add(this.lblFullName);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.lblPhone);
            this.Controls.Add(this.lblAddress);
            this.Controls.Add(this.lblProfessionalTitle);
            this.Controls.Add(this.lblBiography);
            this.Controls.Add(this.lblSkills);
            this.Controls.Add(this.lblVerified);
            this.Controls.Add(this.lblRating);

            this.Controls.Add(this.txtFullName);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.txtProfessionalTitle);
            this.Controls.Add(this.txtBiography);
            this.Controls.Add(this.txtSkills);

            this.Controls.Add(this.lblVerifiedValue);
            this.Controls.Add(this.lblRatingValue);

            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}