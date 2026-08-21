using System;
using System.Drawing;
using System.Windows.Forms;
using SkillHub.Data;
using SkillHub.Forms.Admin;
using SkillHub.Forms.Client;
using SkillHub.Forms.Freelancer;
using SkillHub.Models;
using SkillHub.Services;
using SkillHub.Utilities;

namespace SkillHub.Forms.Common
{
    public sealed class FrmLogin : Form
    {
        private readonly AuthenticationService _authentication;
        private readonly DatabaseConnection _database;

        private TextBox _emailInput;
        private TextBox _passwordInput;
        private Button _loginButton;
        private Label _connectionStatus;

        public FrmLogin()
        {
            _authentication = new AuthenticationService();
            _database = new DatabaseConnection();

            UiFactory.ConfigureForm(this, "SkillHub | Sign In", new Size(690, 690));
            Size = new Size(920, 760);

            BuildLayout();
        }

        private void BuildLayout()
        {
            TableLayoutPanel outer = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 3
            };

            outer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            outer.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 500F));
            outer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            outer.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            outer.RowStyles.Add(new RowStyle(SizeType.Absolute, 560F));
            outer.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            Panel card = UiFactory.CreateCard(500, 560);
            card.Dock = DockStyle.Fill;
            card.Padding = new Padding(38, 36, 38, 24);

            FlowLayoutPanel content = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = false
            };

            Label brand = UiFactory.CreateHeading("SkillHub", 27);
            Label tagline = UiFactory.CreateCaption(
                "Sign in to the software-development freelance marketplace.");
            tagline.Width = 414;
            tagline.Height = 44;

            _emailInput = UiFactory.CreateTextBox(412);
            _emailInput.MaxLength = 150;

            _passwordInput = UiFactory.CreateTextBox(412, true);
            _passwordInput.MaxLength = 128;

            _loginButton = UiFactory.CreateButton("Sign In", true, 412, 44);
            _loginButton.Click += LoginButtonClick;

            Button registerButton = UiFactory.CreateButton(
                "Create Client or Freelancer Account",
                false,
                412,
                42);
            registerButton.Click += RegisterButtonClick;

            Button testConnectionButton = UiFactory.CreateButton(
                "Test Database Connection",
                false,
                240,
                36);
            testConnectionButton.Click += TestConnectionButtonClick;

            _connectionStatus = new Label
            {
                AutoSize = false,
                Width = 412,
                Height = 40,
                ForeColor = UiFactory.MutedText,
                Text = "Run the shared SQL script before the first sign-in."
            };

            content.Controls.Add(brand);
            content.Controls.Add(tagline);
            content.Controls.Add(UiFactory.CreateFieldLabel("Email address"));
            content.Controls.Add(_emailInput);
            content.Controls.Add(UiFactory.CreateFieldLabel("Password"));
            content.Controls.Add(_passwordInput);
            content.Controls.Add(_loginButton);
            content.Controls.Add(registerButton);
            content.Controls.Add(testConnectionButton);
            content.Controls.Add(_connectionStatus);

            card.Controls.Add(content);
            outer.Controls.Add(card, 1, 1);
            Controls.Add(outer);

            AcceptButton = _loginButton;
        }

        private void LoginButtonClick(object sender, EventArgs arguments)
        {
            try
            {
                _loginButton.Enabled = false;
                Cursor = Cursors.WaitCursor;

                User authenticatedUser = _authentication.Login(
                    _emailInput.Text,
                    _passwordInput.Text);

                OpenRoleDashboard(authenticatedUser);
            }
            catch (Exception exception)
            {
                UserSession.Clear();
                UiFactory.ShowError(this, exception);
            }
            finally
            {
                _passwordInput.Clear();
                _loginButton.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        private void RegisterButtonClick(object sender, EventArgs arguments)
        {
            using (FrmRegister registration = new FrmRegister(_authentication))
            {
                if (registration.ShowDialog(this) == DialogResult.OK)
                {
                    _emailInput.Text = registration.RegisteredEmail;
                    _passwordInput.Focus();
                }
            }
        }

        private void TestConnectionButtonClick(object sender, EventArgs arguments)
        {
            string result;
            bool connected = _database.TryTestConnection(out result);

            _connectionStatus.ForeColor = connected ? UiFactory.Accent : UiFactory.Danger;
            _connectionStatus.Text = result;
        }

        private void OpenRoleDashboard(User authenticatedUser)
        {
            Form dashboard;

            if (authenticatedUser is SkillHub.Models.Admin)
            {
                dashboard = new FrmAdminDashboard(_authentication);
            }
            else if (authenticatedUser is SkillHub.Models.Freelancer)
            {
                dashboard = new FrmFreelancerDashboard(_authentication);
            }
            else if (authenticatedUser is SkillHub.Models.Client)
            {
                dashboard = new FrmClientDashboard(_authentication);
            }
            else
            {
                throw new UnauthorizedAccessException(
                    "No authorized dashboard exists for this account.");
            }

            Hide();

            try
            {
                using (dashboard)
                {
                    dashboard.ShowDialog();
                }
            }
            finally
            {
                _authentication.Logout();
                Show();
                Activate();
            }
        }
    }
}
