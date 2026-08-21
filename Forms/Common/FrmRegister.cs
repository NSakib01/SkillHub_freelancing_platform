using System;
using System.Drawing;
using System.Windows.Forms;
using SkillHub.Models;
using SkillHub.Services;

namespace SkillHub.Forms.Common
{
    public sealed class FrmRegister : Form
    {
        private readonly AuthenticationService _authentication;

        private ComboBox _roleInput;
        private TextBox _fullNameInput;
        private TextBox _emailInput;
        private TextBox _phoneInput;
        private TextBox _addressInput;
        private TextBox _passwordInput;
        private TextBox _passwordConfirmationInput;

        public FrmRegister(AuthenticationService authentication)
        {
            if (authentication == null)
            {
                throw new ArgumentNullException(nameof(authentication));
            }

            _authentication = authentication;

            UiFactory.ConfigureForm(
                this,
                "SkillHub | Create Account",
                new Size(530, 680));

            Size = new Size(560, 790);
            MaximizeBox = false;

            BuildLayout();
        }

        public string RegisteredEmail { get; private set; }

        private void BuildLayout()
        {
            Panel page = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(34, 24, 20, 16),
                AutoScroll = true
            };

            FlowLayoutPanel content = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = UiFactory.PageBackground
            };

            Label heading = UiFactory.CreateHeading("Create Your Account", 23);
            Label caption = UiFactory.CreateCaption(
                "Clients and freelancers can register. Platform administrator accounts are seeded privately.");
            caption.Width = 450;

            _roleInput = new ComboBox
            {
                Width = 445,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Margin = new Padding(0, 0, 0, 8)
            };
            _roleInput.Items.Add(UserRoles.Client);
            _roleInput.Items.Add(UserRoles.Freelancer);
            _roleInput.SelectedIndex = 0;

            _fullNameInput = UiFactory.CreateTextBox(445);
            _fullNameInput.MaxLength = 120;

            _emailInput = UiFactory.CreateTextBox(445);
            _emailInput.MaxLength = 150;

            _phoneInput = UiFactory.CreateTextBox(445);
            _phoneInput.MaxLength = 20;

            _addressInput = UiFactory.CreateTextBox(445);
            _addressInput.MaxLength = 250;

            _passwordInput = UiFactory.CreateTextBox(445, true);
            _passwordInput.MaxLength = 128;

            _passwordConfirmationInput = UiFactory.CreateTextBox(445, true);
            _passwordConfirmationInput.MaxLength = 128;

            content.Controls.Add(heading);
            content.Controls.Add(caption);
            AddLabeledControl(content, "Account type", _roleInput);
            AddLabeledControl(content, "Full name", _fullNameInput);
            AddLabeledControl(content, "Email address", _emailInput);
            AddLabeledControl(content, "Phone number (optional)", _phoneInput);
            AddLabeledControl(content, "Address (optional)", _addressInput);
            AddLabeledControl(content, "Password", _passwordInput);
            AddLabeledControl(content, "Confirm password", _passwordConfirmationInput);

            Label passwordHint = UiFactory.CreateCaption(
                "Use at least 8 characters with uppercase, lowercase, a number and a symbol.");
            passwordHint.Width = 445;
            passwordHint.Height = 39;
            content.Controls.Add(passwordHint);

            FlowLayoutPanel actions = new FlowLayoutPanel
            {
                Width = 450,
                Height = 55,
                WrapContents = false
            };

            Button createButton = UiFactory.CreateButton("Create Account", true, 190);
            createButton.Click += CreateButtonClick;

            Button cancelButton = UiFactory.CreateButton("Cancel", false, 120);
            cancelButton.Click += delegate { Close(); };

            actions.Controls.Add(createButton);
            actions.Controls.Add(cancelButton);
            content.Controls.Add(actions);

            page.Controls.Add(content);
            Controls.Add(page);

            AcceptButton = createButton;
        }

        private static void AddLabeledControl(
            FlowLayoutPanel content,
            string label,
            Control input)
        {
            content.Controls.Add(UiFactory.CreateFieldLabel(label));
            content.Controls.Add(input);
        }

        private void CreateButtonClick(object sender, EventArgs arguments)
        {
            try
            {
                int userId = _authentication.Register(
                    Convert.ToString(_roleInput.SelectedItem),
                    _fullNameInput.Text,
                    _emailInput.Text,
                    _phoneInput.Text,
                    _addressInput.Text,
                    _passwordInput.Text,
                    _passwordConfirmationInput.Text);

                RegisteredEmail = _emailInput.Text.Trim();

                UiFactory.ShowSuccess(
                    this,
                    "Account created successfully. Your database user ID is "
                    + userId + ". You can sign in now.");

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
