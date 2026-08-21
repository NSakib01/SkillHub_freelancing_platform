using System;
using System.Drawing;
using System.Windows.Forms;
using SkillHub.Services;

namespace SkillHub.Forms.Common
{
    public sealed class FrmChangePassword : Form
    {
        private readonly AuthenticationService _authentication;

        private TextBox _currentPasswordInput;
        private TextBox _newPasswordInput;
        private TextBox _passwordConfirmationInput;

        public FrmChangePassword(AuthenticationService authentication)
        {
            AuthorizationService.DemandAuthenticated();

            if (authentication == null)
            {
                throw new ArgumentNullException(nameof(authentication));
            }

            _authentication = authentication;

            UiFactory.ConfigureForm(
                this,
                "SkillHub | Change Password",
                new Size(510, 440));

            Size = new Size(530, 490);
            MaximizeBox = false;

            BuildLayout();
        }

        private void BuildLayout()
        {
            FlowLayoutPanel content = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(32, 23, 15, 15)
            };

            content.Controls.Add(UiFactory.CreateHeading("Change Password", 22));

            Label caption = UiFactory.CreateCaption(
                "Your current password is required before a new password can be saved.");
            caption.Width = 420;
            content.Controls.Add(caption);

            _currentPasswordInput = UiFactory.CreateTextBox(420, true);
            _currentPasswordInput.MaxLength = 128;

            _newPasswordInput = UiFactory.CreateTextBox(420, true);
            _newPasswordInput.MaxLength = 128;

            _passwordConfirmationInput = UiFactory.CreateTextBox(420, true);
            _passwordConfirmationInput.MaxLength = 128;

            AddField(content, "Current password", _currentPasswordInput);
            AddField(content, "New password", _newPasswordInput);
            AddField(content, "Confirm new password", _passwordConfirmationInput);

            Button saveButton = UiFactory.CreateButton("Update Password", true, 205);
            saveButton.Click += SaveButtonClick;
            content.Controls.Add(saveButton);

            Controls.Add(content);
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
                _authentication.ChangePassword(
                    _currentPasswordInput.Text,
                    _newPasswordInput.Text,
                    _passwordConfirmationInput.Text);

                UiFactory.ShowSuccess(this, "Your password was changed successfully.");
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception exception)
            {
                UiFactory.ShowError(this, exception);
            }
        }
    }
}
