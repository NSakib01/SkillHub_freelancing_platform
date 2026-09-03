using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SkillHub.Data;
using SkillHub.Forms.Freelancer;
using SkillHub.Models;
using SkillHub.Services;
using SkillHub.Utilities;
using SkillHub.UI;

namespace SkillHub.Forms.Common
{
    [DesignerCategory("Code")]
    public abstract class DashboardFormBase : Form
    {
        private readonly AuthenticationService _authentication;

        private FlowLayoutPanel _mainContent;
        private FlowLayoutPanel _sideContent;
        private Label _identityLabel;
        private PictureBox _avatarPicture;

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

            UiFactory.ConfigureForm(
                this,
                windowTitle,
                new Size(1080, 700));

            Size = new Size(1280, 820);

            BuildShell();
        }

        protected AuthenticationService Authentication
        {
            get { return _authentication; }
        }

        // ============================================================
        // MAIN CARD
        // ============================================================

        protected void AddMainCard(
            string title,
            string description)
        {
            AddCard(
                _mainContent,
                title,
                description,
                570,
                118);
        }

        // ============================================================
        // MAIN CARD WITH ACTION
        // ============================================================

        protected void AddMainCardWithAction(
            string title,
            string description,
            string actionText,
            EventHandler actionHandler)
        {
            Panel card =
                UiFactory.CreateCard(570, 128);

            Panel accentBar = new Panel
            {
                BackColor = MarketplaceTheme.Accent,
                Location = new Point(0, 18),
                Size = new Size(5, 92)
            };

            Label titleLabel = new Label
            {
                AutoSize = false,
                Width = 350,
                Height = 27,
                Text = title,
                Font = new Font(
                    "Segoe UI",
                    11.5F,
                    FontStyle.Bold),
                ForeColor = UiFactory.PrimaryDark,
                Location = new Point(28, 17)
            };

            Label descriptionLabel = new Label
            {
                AutoSize = false,
                Width = 345,
                Height = 52,
                Text = description,
                ForeColor = UiFactory.MutedText,
                Location = new Point(29, 49)
            };

            Button actionButton =
                UiFactory.CreateButton(
                    actionText,
                    true,
                    145,
                    40);

            actionButton.Location =
                new Point(397, 43);

            actionButton.Click += actionHandler;

            card.Controls.Add(accentBar);
            card.Controls.Add(titleLabel);
            card.Controls.Add(descriptionLabel);
            card.Controls.Add(actionButton);

            _mainContent.Controls.Add(card);
        }

        // ============================================================
        // SIDE CARD
        // ============================================================

        protected void AddSideCard(
            string title,
            string description)
        {
            AddCard(
                _sideContent,
                title,
                description,
                350,
                130);
        }

        // ============================================================
        // MAIN ACTION
        // ============================================================

        protected void AddMainAction(
            string label,
            EventHandler handler,
            int width = 235)
        {
            Button action =
                UiFactory.CreateButton(
                    label,
                    true,
                    width,
                    44);

            action.Click += handler;

            _mainContent.Controls.Add(action);
        }

        // ============================================================
        // READ COUNT
        // ============================================================

        protected string ReadCount(
            string statement,
            int? currentUserId = null)
        {
            try
            {
                DatabaseConnection database =
                    new DatabaseConnection();

                using (SqlConnection connection =
                       database.OpenConnection())
                using (SqlCommand command =
                       new SqlCommand(statement, connection))
                {
                    if (currentUserId.HasValue)
                    {
                        DatabaseConnection.AddParameter(
                            command,
                            "@UserId",
                            SqlDbType.Int,
                            currentUserId.Value);
                    }

                    object result =
                        command.ExecuteScalar();

                    if (result == null ||
                        result == DBNull.Value)
                    {
                        return "0";
                    }

                    return Convert.ToString(result);
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

        // ============================================================
        // READ VALUE
        // ============================================================

        protected decimal? ReadDecimal(
            string statement,
            int? currentUserId = null)
        {
            try
            {
                DatabaseConnection database =
                    new DatabaseConnection();

                using (SqlConnection connection =
                       database.OpenConnection())
                using (SqlCommand command =
                       new SqlCommand(statement, connection))
                {
                    if (currentUserId.HasValue)
                    {
                        DatabaseConnection.AddParameter(
                            command,
                            "@UserId",
                            SqlDbType.Int,
                            currentUserId.Value);
                    }

                    object result =
                        command.ExecuteScalar();

                    if (result == null ||
                        result == DBNull.Value)
                    {
                        return 0m;
                    }

                    return Convert.ToDecimal(result);
                }
            }
            catch (SqlException)
            {
                return null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
            catch (FormatException)
            {
                return null;
            }
            catch (InvalidCastException)
            {
                return null;
            }
        }

        // ============================================================
        // IDENTITY
        // ============================================================

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

                if (_avatarPicture != null)
                {
                    Image previousImage = _avatarPicture.Image;
                    _avatarPicture.Image = ImageAssetHelper.LoadAvatar(
                        UserSession.CurrentUser.ProfileImagePath,
                        UserSession.FullName,
                        _avatarPicture.Width);

                    if (previousImage != null)
                    {
                        previousImage.Dispose();
                    }
                }
            }
        }

        // ============================================================
        // BUILD SHELL
        // ============================================================

        private void BuildShell()
        {
            TableLayoutPanel shell =
                new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    RowCount = 2,
                    ColumnCount = 1
                };

            shell.RowStyles.Add(
                new RowStyle(
                    SizeType.Absolute,
                    132F));

            shell.RowStyles.Add(
                new RowStyle(
                    SizeType.Percent,
                    100F));

            // ========================================================
            // HEADER
            // ========================================================

            Panel header =
                new Panel
                {
                    Dock = DockStyle.Fill,
                    BackColor = MarketplaceTheme.Navy,
                    Padding = new Padding(
                        34,
                        19,
                        22,
                        15)
                };

            _avatarPicture = new PictureBox
                {
                    Size = new Size(68, 68),
                    Location = new Point(32, 27),
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    BackColor = Color.Transparent,
                    Image = ImageAssetHelper.LoadAvatar(
                        UserSession.CurrentUser.ProfileImagePath,
                        UserSession.FullName,
                        68)
                };

            Label brand =
                new Label
                {
                    AutoSize = true,
                    Text = "SkillHub",
                    ForeColor = Color.White,
                    Font = new Font(
                        "Segoe UI",
                        23F,
                        FontStyle.Bold),
                    Location = new Point(118, 21),
                    BackColor = Color.Transparent
                };

            _identityLabel =
                new Label
                {
                    AutoSize = true,
                    ForeColor =
                        Color.FromArgb(
                            222,
                            233,
                            246),
                    Font =
                        new Font(
                            "Segoe UI",
                            10F),
                    Location =
                        new Point(121, 70),
                    BackColor = Color.Transparent
                };

            FlowLayoutPanel actions =
                new FlowLayoutPanel
                {
                    Width = 470,
                    Height = 55,
                    Anchor =
                        AnchorStyles.Top |
                        AnchorStyles.Right,
                    Location =
                        new Point(
                            header.Width - 490,
                            30),
                    FlowDirection =
                        FlowDirection.RightToLeft,
                    WrapContents = false,
                    BackColor =
                        Color.Transparent
                };

            header.SizeChanged += delegate
            {
                actions.Left =
                    Math.Max(
                            590,
                        header.ClientSize.Width
                        - actions.Width
                        - 16);
            };

            Button logoutButton =
                UiFactory.CreateButton(
                    "Log Out",
                    false,
                    106,
                    38);

            logoutButton.Click +=
                LogoutButtonClick;

            Button passwordButton =
                UiFactory.CreateButton(
                    "Change Password",
                    false,
                    164,
                    38);

            passwordButton.Click +=
                PasswordButtonClick;

            Button profileButton =
                UiFactory.CreateButton(
                    "My Profile",
                    false,
                    116,
                    38);

            profileButton.Click +=
                ProfileButtonClick;

            actions.Controls.Add(logoutButton);
            actions.Controls.Add(passwordButton);
            actions.Controls.Add(profileButton);

            header.Paint += delegate(object sender, PaintEventArgs arguments)
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    header.ClientRectangle,
                    MarketplaceTheme.Navy,
                    UiFactory.Primary,
                    15F))
                {
                    arguments.Graphics.FillRectangle(brush, header.ClientRectangle);
                }
            };

            header.Controls.Add(_avatarPicture);
            header.Controls.Add(brand);
            header.Controls.Add(_identityLabel);
            header.Controls.Add(actions);

            // ========================================================
            // BODY
            // ========================================================

            TableLayoutPanel body =
                new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 2,
                    RowCount = 1,
                    Padding =
                        new Padding(
                            34,
                            26,
                            28,
                            20)
                };

            body.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent,
                    65F));

            body.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Percent,
                    35F));

            _mainContent =
                CreateContentColumn();

            _sideContent =
                CreateContentColumn();

            _mainContent.Controls.Add(
                UiFactory.CreateHeading(
                    UserSession.CurrentUser.DashboardTitle,
                    22));

            Label welcome =
                UiFactory.CreateCaption(
                    "Welcome back, " + UserSession.FullName + ". Your SkillHub workspace is ready.");

            welcome.Width = 580;
            welcome.Height = 40;

            _mainContent.Controls.Add(welcome);

            Label foundationHeading =
                UiFactory.CreateHeading(
                    "Quick information",
                    17);

            _sideContent.Controls.Add(
                foundationHeading);

            AddSideCard(
                "Your account",
                UserSession.RoleName
                + " access is active. Use My Profile to keep your public information current.");

            AddSideCard(
                "Secure workspace",
                "Your services, orders and account actions are linked to the currently signed-in profile.");

            body.Controls.Add(
                _mainContent,
                0,
                0);

            body.Controls.Add(
                _sideContent,
                1,
                0);

            shell.Controls.Add(
                header,
                0,
                0);

            shell.Controls.Add(
                body,
                0,
                1);

            Controls.Add(shell);

            RefreshIdentity();
        }

        // ============================================================
        // CONTENT COLUMN
        // ============================================================

        private static FlowLayoutPanel CreateContentColumn()
        {
            return new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection =
                    FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding =
                    new Padding(
                        0,
                        0,
                        14,
                        0)
            };
        }

        // ============================================================
        // GENERIC CARD
        // ============================================================

        private static void AddCard(
            FlowLayoutPanel parent,
            string title,
            string description,
            int width,
            int height)
        {
            Panel card =
                UiFactory.CreateCard(
                    width,
                    height);

            Panel accentDot = new Panel
            {
                BackColor = MarketplaceTheme.Accent,
                Location = new Point(20, 21),
                Size = new Size(9, 9)
            };

            Label titleLabel =
                new Label
                {
                    AutoSize = false,
                    Width = width - 62,
                    Height = 27,
                    Text = title,
                    Font =
                        new Font(
                            "Segoe UI",
                            11.5F,
                            FontStyle.Bold),
                    ForeColor =
                        UiFactory.PrimaryDark,
                    Location =
                        new Point(37, 14)
                };

            Label descriptionLabel =
                new Label
                {
                    AutoSize = false,
                    Width = width - 48,
                    Height = height - 57,
                    Text = description,
                    ForeColor =
                        UiFactory.MutedText,
                    Location =
                        new Point(22, 48)
                };

            card.Controls.Add(accentDot);
            card.Controls.Add(titleLabel);
            card.Controls.Add(descriptionLabel);

            parent.Controls.Add(card);
        }

        // ============================================================
        // PROFILE
        // ============================================================

        private void ProfileButtonClick(
            object sender,
            EventArgs arguments)
        {
            try
            {
                OpenProfile();

                RefreshIdentity();
            }
            catch (Exception exception)
            {
                UiFactory.ShowError(
                    this,
                    exception);
            }
        }

        protected virtual void OpenProfile()
        {
            using (FrmProfile profile =
                   new FrmProfile(_authentication))
            {
                profile.ShowDialog(this);
            }
        }

        // ============================================================
        // PASSWORD
        // ============================================================

        private void PasswordButtonClick(
            object sender,
            EventArgs arguments)
        {
            try
            {
                using (FrmChangePassword password =
                       new FrmChangePassword(
                           _authentication))
                {
                    password.ShowDialog(this);
                }
            }
            catch (Exception exception)
            {
                UiFactory.ShowError(
                    this,
                    exception);
            }
        }

        // ============================================================
        // LOGOUT
        // ============================================================

        private void LogoutButtonClick(
            object sender,
            EventArgs arguments)
        {
            DialogResult confirmation =
                MessageBox.Show(
                    this,
                    "Log out of your SkillHub account?",
                    "Confirm Logout",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

            if (confirmation ==
                DialogResult.Yes)
            {
                _authentication.Logout();
                Close();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _avatarPicture != null && _avatarPicture.Image != null)
            {
                _avatarPicture.Image.Dispose();
                _avatarPicture.Image = null;
            }

            base.Dispose(disposing);
        }
    }
}
