using System;
using System.Drawing;
using System.Windows.Forms;
using SkillHub.Services;
using SkillHub.Utilities;

namespace SkillHub.Forms.Common
{
    public sealed class FrmProfile : Form
    {
        private readonly AuthenticationService _authentication;

        private TextBox _fullNameInput;
        private TextBox _emailInput;
        private TextBox _phoneInput;
        private TextBox _addressInput;

        public FrmProfile(AuthenticationService authentication)
        {
            AuthorizationService.DemandAuthenticated();

            if (authentication == null)
            {
                throw new ArgumentNullException(nameof(authentication));
            }

            _authentication = authentication;

            UiFactory.ConfigureForm(
                this,
                "SkillHub | My Account Profile",
                new Size(520, 570));

            Size = new Size(550, 650);
            MaximizeBox = false;

            BuildLayout();
        }

        private void BuildLayout()
        {
            Panel page = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(32, 25, 18, 20)
            };

            FlowLayoutPanel content = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };

            content.Controls.Add(UiFactory.CreateHeading("My Account Profile", 23));

            Label accountIdentity = UiFactory.CreateCaption(
                "User ID: " + UserSession.UserId
                + "    |    Role: " + UserSession.RoleName
                + "    |    Legacy type: " + UserSession.UserType);
            accountIdentity.Width = 440;
            accountIdentity.Height = 48;
            content.Controls.Add(accountIdentity);

            _fullNameInput = UiFactory.CreateTextBox(435);
            _fullNameInput.MaxLength = 120;
            _fullNameInput.Text = UserSession.CurrentUser.FullName;

            _emailInput = UiFactory.CreateTextBox(435);
            _emailInput.MaxLength = 150;
            _emailInput.Text = UserSession.CurrentUser.Email;

            _phoneInput = UiFactory.CreateTextBox(435);
            _phoneInput.MaxLength = 20;
            _phoneInput.Text = UserSession.CurrentUser.Phone ?? string.Empty;

            _addressInput = UiFactory.CreateTextBox(435);
            _addressInput.MaxLength = 250;
            _addressInput.Text = UserSession.CurrentUser.Address ?? string.Empty;

            AddField(content, "Full name", _fullNameInput);
            AddField(content, "Email address", _emailInput);
            AddField(content, "Phone number", _phoneInput);
            AddField(content, "Address", _addressInput);

            FlowLayoutPanel actions = new FlowLayoutPanel
            {
                Width = 440,
                Height = 100,
                WrapContents = true
            };

            Button saveButton = UiFactory.CreateButton("Save Profile", true, 165);
            saveButton.Click += SaveButtonClick;

            Button passwordButton = UiFactory.CreateButton("Change Password", false, 185);
            passwordButton.Click += PasswordButtonClick;

            actions.Controls.Add(saveButton);
            actions.Controls.Add(passwordButton);
            content.Controls.Add(actions);

            page.Controls.Add(content);
            Controls.Add(page);

            AcceptButton = saveButton;
        }

        private static void AddField(FlowLayoutPanel content, string label, Control input)
        {
            content.Controls.Add(UiFactory.CreateFieldLabel(label));
            content.Controls.Add(input);
        }

        private void SaveButtonClick(object sender, EventArgs arguments)
        {
            try
            {
                _authentication.UpdateMyProfile(
                    _fullNameInput.Text,
                    _emailInput.Text,
                    _phoneInput.Text,
                    _addressInput.Text);

                UiFactory.ShowSuccess(this, "Your account profile was updated successfully.");
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception exception)
            {
                UiFactory.ShowError(this, exception);
            }
        }

        private void PasswordButtonClick(object sender, EventArgs arguments)
        {
            using (FrmChangePassword passwordForm = new FrmChangePassword(_authentication))
            {
                passwordForm.ShowDialog(this);
            }
        }
    }
}
