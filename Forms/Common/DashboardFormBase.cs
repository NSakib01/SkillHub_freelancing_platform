using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using SkillHub.Data;
using SkillHub.Forms.Freelancer;
using SkillHub.Models;
using SkillHub.Services;
using SkillHub.Utilities;

namespace SkillHub.Forms.Common
{
    [DesignerCategory("Code")]
    public abstract class DashboardFormBase : Form
    {
        private readonly AuthenticationService _authentication;

        private FlowLayoutPanel _mainContent;
        private FlowLayoutPanel _sideContent;
        private Label _identityLabel;

        protected DashboardFormBase(
            AuthenticationService authentication,
            string expectedRole,
            string windowTitle)
        {
            AuthorizationService.DemandRole(expectedRole);

            if (authentication == null)
            {
                throw new ArgumentNullException(nameof(authentication));
            }

            _authentication = authentication;

            UiFactory.ConfigureForm(this, windowTitle, new Size(1080, 700));
            Size = new Size(1280, 820);

            BuildShell();
        }

        protected AuthenticationService Authentication
        {
            get { return _authentication; }
        }

        protected void AddMainCard(string title, string description)
        {
            AddCard(_mainContent, title, description, 570, 118);
        }

        protected void AddSideCard(string title, string description)
        {
            AddCard(_sideContent, title, description, 350, 130);
        }

        protected void AddMainAction(string label, EventHandler handler, int width = 235)
        {
            Button action = UiFactory.CreateButton(label, true, width, 44);
            action.Click += handler;
            _mainContent.Controls.Add(action);
        }

        protected string ReadCount(string statement, int? currentUserId = null)
        {
            try
            {
                DatabaseConnection database = new DatabaseConnection();

                using (SqlConnection connection = database.OpenConnection())
                using (SqlCommand command = new SqlCommand(statement, connection))
                {
                    if (currentUserId.HasValue)
                    {
                        DatabaseConnection.AddParameter(
                            command,
                            "@UserId",
                            SqlDbType.Int,
                            currentUserId.Value);
                    }

                    object result = command.ExecuteScalar();
                    return Convert.ToString(result == DBNull.Value ? 0 : result);
                }
            }
            catch (SqlException)
            {
                return "Unavailable";
            }
            catch (InvalidOperationException)
            {
                return "Unavailable";
            }
        }

        protected void RefreshIdentity()
        {
            if (UserSession.IsAuthenticated)
            {
                _identityLabel.Text =
                    UserSession.FullName
                    + "   |   "
                    + UserSession.RoleName
                    + "   |   User ID "
                    + UserSession.UserId;
            }
        }

        private void BuildShell()
        {
            TableLayoutPanel shell = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1
            };

            shell.RowStyles.Add(new RowStyle(SizeType.Absolute, 118F));
            shell.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            Panel header = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = UiFactory.PrimaryDark,
                Padding = new Padding(34, 19, 22, 15)
            };

            Label brand = new Label
            {
                AutoSize = true,
                Text = "SkillHub",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 23F, FontStyle.Bold),
                Location = new Point(34, 15)
            };

            _identityLabel = new Label
            {
                AutoSize = true,
                ForeColor = Color.FromArgb(222, 233, 246),
                Font = new Font("Segoe UI", 10F),
                Location = new Point(39, 67)
            };

            FlowLayoutPanel actions = new FlowLayoutPanel
            {
                Width = 470,
                Height = 55,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(header.Width - 490, 30),
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false,
                BackColor = UiFactory.PrimaryDark
            };

            header.SizeChanged += delegate
            {
                actions.Left = Math.Max(450, header.ClientSize.Width - actions.Width - 16);
            };

            Button logoutButton = UiFactory.CreateButton("Log Out", false, 106, 38);
            logoutButton.Click += LogoutButtonClick;

            Button passwordButton = UiFactory.CreateButton("Change Password", false, 164, 38);
            passwordButton.Click += PasswordButtonClick;

            Button profileButton = UiFactory.CreateButton("My Profile", false, 116, 38);
            profileButton.Click += ProfileButtonClick;

            actions.Controls.Add(logoutButton);
            actions.Controls.Add(passwordButton);
            actions.Controls.Add(profileButton);

            header.Controls.Add(brand);
            header.Controls.Add(_identityLabel);
            header.Controls.Add(actions);

            TableLayoutPanel body = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(34, 26, 28, 20)
            };

            body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
            body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));

            _mainContent = CreateContentColumn();
            _sideContent = CreateContentColumn();

            _mainContent.Controls.Add(UiFactory.CreateHeading(
                UserSession.CurrentUser.DashboardTitle,
                22));

            Label welcome = UiFactory.CreateCaption(
                "Signed in with an authorized account and a shared SQL Server session.");
            welcome.Width = 580;
            welcome.Height = 40;
            _mainContent.Controls.Add(welcome);

            Label foundationHeading = UiFactory.CreateHeading("Foundation Status", 17);
            _sideContent.Controls.Add(foundationHeading);

            AddSideCard(
                "Current session",
                "User ID: " + UserSession.UserId
                + "\r\nRole: " + UserSession.RoleName
                + "\r\nLegacy user type: " + UserSession.UserType);

            AddSideCard(
                "Shared integration rule",
                "Use UserSession.UserId in SQL parameters; never hardcode a user ID. "
                + "Every module must reuse DatabaseConnection.");

            body.Controls.Add(_mainContent, 0, 0);
            body.Controls.Add(_sideContent, 1, 0);

            shell.Controls.Add(header, 0, 0);
            shell.Controls.Add(body, 0, 1);
            Controls.Add(shell);

            RefreshIdentity();
        }

        private static FlowLayoutPanel CreateContentColumn()
        {
            return new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(0, 0, 14, 0)
            };
        }

        private static void AddCard(
            FlowLayoutPanel parent,
            string title,
            string description,
            int width,
            int height)
        {
            Panel card = UiFactory.CreateCard(width, height);

            Label titleLabel = new Label
            {
                AutoSize = false,
                Width = width - 48,
                Height = 27,
                Text = title,
                Font = new Font("Segoe UI", 11.5F, FontStyle.Bold),
                ForeColor = UiFactory.PrimaryDark,
                Location = new Point(20, 15)
            };

            Label descriptionLabel = new Label
            {
                AutoSize = false,
                Width = width - 48,
                Height = height - 57,
                Text = description,
                ForeColor = UiFactory.MutedText,
                Location = new Point(21, 46)
            };

            card.Controls.Add(titleLabel);
            card.Controls.Add(descriptionLabel);
            parent.Controls.Add(card);
        }

        private void ProfileButtonClick(object sender, EventArgs arguments)
        {
            try
            {
                OpenProfile();

                RefreshIdentity();
            }
            catch (Exception exception)
            {
                UiFactory.ShowError(this, exception);
            }
        }

        private void PasswordButtonClick(object sender, EventArgs arguments)
        {
            try
            {
                using (FrmChangePassword password = new FrmChangePassword(_authentication))
                {
                    password.ShowDialog(this);
                }
            }
            catch (Exception exception)
            {
                UiFactory.ShowError(this, exception);
            }
        }

        protected virtual void OpenProfile()
        {
            using (FrmProfile profile = new FrmProfile(_authentication))
            {
                profile.ShowDialog(this);
            }
        }

        private void LogoutButtonClick(object sender, EventArgs arguments)
        {
            DialogResult confirmation = MessageBox.Show(
                this,
                "Log out of your SkillHub account?",
                "Confirm Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmation == DialogResult.Yes)
            {
                _authentication.Logout();
                Close();
            }
        }
    }
}