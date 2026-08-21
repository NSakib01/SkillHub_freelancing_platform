using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using SkillHub.Models;
using SkillHub.Services;
using SkillHub.Utilities;

namespace SkillHub.Forms.Common
{
    /// <summary>
    /// Sakib's complete individual account/profile CRUD demonstration:
    /// Create, Read/Search, Update and safe soft-deactivation.
    /// </summary>
    public sealed class FrmAccountManager : Form
    {
        private readonly AuthenticationService _authentication;

        private DataGridView _accountsGrid;
        private TextBox _searchInput;
        private ComboBox _roleInput;
        private TextBox _fullNameInput;
        private TextBox _emailInput;
        private TextBox _phoneInput;
        private TextBox _addressInput;
        private TextBox _passwordInput;
        private TextBox _passwordConfirmationInput;
        private Label _selectedAccountLabel;

        private int? _selectedUserId;
        private bool _suppressSelectionChanged;

        public FrmAccountManager(AuthenticationService authentication)
        {
            AuthorizationService.DemandAdmin();

            if (authentication == null)
            {
                throw new ArgumentNullException(nameof(authentication));
            }

            _authentication = authentication;

            UiFactory.ConfigureForm(
                this,
                "SkillHub | Account and Profile CRUD - Sakib",
                new Size(1130, 720));

            Size = new Size(1390, 850);

            BuildLayout();
            LoadAccounts();
        }

        private void BuildLayout()
        {
            TableLayoutPanel shell = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1,
                Padding = new Padding(24, 20, 24, 20)
            };

            shell.RowStyles.Add(new RowStyle(SizeType.Absolute, 78F));
            shell.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            Panel headingPanel = new Panel { Dock = DockStyle.Fill };
            Label heading = UiFactory.CreateHeading("Account and Profile Management", 22);
            heading.Location = new Point(0, 0);

            Label caption = new Label
            {
                AutoSize = true,
                Text = "Sakib's individual CRUD | Create, read/search, update and soft-deactivate",
                ForeColor = UiFactory.MutedText,
                Location = new Point(3, 41)
            };

            headingPanel.Controls.Add(heading);
            headingPanel.Controls.Add(caption);

            TableLayoutPanel body = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1
            };

            body.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 385F));
            body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            Panel editorPanel = BuildEditorPanel();
            Panel gridPanel = BuildGridPanel();

            body.Controls.Add(editorPanel, 0, 0);
            body.Controls.Add(gridPanel, 1, 0);

            shell.Controls.Add(headingPanel, 0, 0);
            shell.Controls.Add(body, 0, 1);

            Controls.Add(shell);
        }

        private Panel BuildEditorPanel()
        {
            Panel editor = UiFactory.CreateCard(372, 680);
            editor.Dock = DockStyle.Fill;
            editor.AutoScroll = true;
            editor.Padding = new Padding(17, 16, 8, 10);
            editor.Margin = new Padding(0, 0, 14, 0);

            FlowLayoutPanel fields = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };

            Label editorHeading = UiFactory.CreateHeading("Account Editor", 16);
            fields.Controls.Add(editorHeading);

            _selectedAccountLabel = UiFactory.CreateCaption(
                "New account mode: create a Client or Freelancer.");
            _selectedAccountLabel.Width = 325;
            _selectedAccountLabel.Height = 41;
            fields.Controls.Add(_selectedAccountLabel);

            _roleInput = new ComboBox
            {
                Width = 326,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Margin = new Padding(0, 0, 0, 6)
            };
            _roleInput.Items.Add(UserRoles.Client);
            _roleInput.Items.Add(UserRoles.Freelancer);
            _roleInput.SelectedIndex = 0;

            _fullNameInput = UiFactory.CreateTextBox(326);
            _fullNameInput.MaxLength = 120;

            _emailInput = UiFactory.CreateTextBox(326);
            _emailInput.MaxLength = 150;

            _phoneInput = UiFactory.CreateTextBox(326);
            _phoneInput.MaxLength = 20;

            _addressInput = UiFactory.CreateTextBox(326);
            _addressInput.MaxLength = 250;

            _passwordInput = UiFactory.CreateTextBox(326, true);
            _passwordInput.MaxLength = 128;

            _passwordConfirmationInput = UiFactory.CreateTextBox(326, true);
            _passwordConfirmationInput.MaxLength = 128;

            AddField(fields, "Account type", _roleInput);
            AddField(fields, "Full name", _fullNameInput);
            AddField(fields, "Email address", _emailInput);
            AddField(fields, "Phone number", _phoneInput);
            AddField(fields, "Address", _addressInput);
            AddField(fields, "Password (new accounts only)", _passwordInput);
            AddField(fields, "Confirm password", _passwordConfirmationInput);

            FlowLayoutPanel actions = new FlowLayoutPanel
            {
                Width = 335,
                Height = 112,
                WrapContents = true
            };

            Button createButton = UiFactory.CreateButton("Create", true, 145);
            createButton.Click += CreateButtonClick;

            Button updateButton = UiFactory.CreateButton("Update", false, 145);
            updateButton.Click += UpdateButtonClick;

            Button deactivateButton = UiFactory.CreateDangerButton("Deactivate", 145);
            deactivateButton.Click += DeactivateButtonClick;

            Button clearButton = UiFactory.CreateButton("Clear", false, 145);
            clearButton.Click += delegate { ResetEditor(); };

            actions.Controls.Add(createButton);
            actions.Controls.Add(updateButton);
            actions.Controls.Add(deactivateButton);
            actions.Controls.Add(clearButton);
            fields.Controls.Add(actions);

            editor.Controls.Add(fields);
            return editor;
        }

        private Panel BuildGridPanel()
        {
            Panel wrapper = UiFactory.CreateCard(800, 680);
            wrapper.Dock = DockStyle.Fill;
            wrapper.Padding = new Padding(18);

            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3
            };

            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));

            FlowLayoutPanel searchBar = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false
            };

            _searchInput = UiFactory.CreateTextBox(360);
            _searchInput.Margin = new Padding(0, 6, 10, 0);
            _searchInput.MaxLength = 150;
            _searchInput.TextChanged += delegate { LoadAccounts(); };

            Button refreshButton = UiFactory.CreateButton("Refresh", false, 110, 35);
            refreshButton.Click += delegate { LoadAccounts(); };

            searchBar.Controls.Add(_searchInput);
            searchBar.Controls.Add(refreshButton);

            _accountsGrid = UiFactory.CreateReadOnlyGrid();
            _accountsGrid.SelectionChanged += AccountsGridSelectionChanged;

            Label hint = new Label
            {
                Dock = DockStyle.Fill,
                ForeColor = UiFactory.MutedText,
                Text = "Search names, emails, roles or statuses. Select a row to update or deactivate it.",
                TextAlign = ContentAlignment.MiddleLeft
            };

            layout.Controls.Add(searchBar, 0, 0);
            layout.Controls.Add(_accountsGrid, 0, 1);
            layout.Controls.Add(hint, 0, 2);

            wrapper.Controls.Add(layout);
            return wrapper;
        }

        private static void AddField(FlowLayoutPanel fields, string label, Control input)
        {
            fields.Controls.Add(UiFactory.CreateFieldLabel(label));
            fields.Controls.Add(input);
        }

        private void LoadAccounts()
        {
            if (_accountsGrid == null || _searchInput == null)
            {
                return;
            }

            try
            {
                int? selectedBeforeRefresh = _selectedUserId;
                DataTable accounts = _authentication.SearchAccounts(_searchInput.Text);
                _suppressSelectionChanged = true;
                _accountsGrid.DataSource = accounts;

                if (_accountsGrid.Columns.Contains("UserId"))
                {
                    _accountsGrid.Columns["UserId"].HeaderText = "User ID";
                }

                if (_accountsGrid.Columns.Contains("FullName"))
                {
                    _accountsGrid.Columns["FullName"].HeaderText = "Full Name";
                }

                if (_accountsGrid.Columns.Contains("RoleName"))
                {
                    _accountsGrid.Columns["RoleName"].HeaderText = "Role";
                }

                if (_accountsGrid.Columns.Contains("UserType"))
                {
                    _accountsGrid.Columns["UserType"].HeaderText = "Legacy Type";
                }

                _accountsGrid.ClearSelection();
                _selectedUserId = selectedBeforeRefresh;
            }
            catch (Exception exception)
            {
                UiFactory.ShowError(this, exception);
            }
            finally
            {
                _suppressSelectionChanged = false;
            }
        }

        private void AccountsGridSelectionChanged(object sender, EventArgs arguments)
        {
            if (_suppressSelectionChanged
                || _accountsGrid.CurrentRow == null
                || _accountsGrid.SelectedRows.Count == 0
                || _accountsGrid.CurrentRow.DataBoundItem == null)
            {
                return;
            }

            DataGridViewRow selected = _accountsGrid.CurrentRow;
            string roleName = Convert.ToString(selected.Cells["RoleName"].Value);

            _selectedUserId = Convert.ToInt32(selected.Cells["UserId"].Value);

            if (!_roleInput.Items.Contains(roleName))
            {
                _roleInput.Items.Add(roleName);
            }

            _roleInput.SelectedItem = roleName;
            _roleInput.Enabled = false;
            _fullNameInput.Text = Convert.ToString(selected.Cells["FullName"].Value);
            _emailInput.Text = Convert.ToString(selected.Cells["Email"].Value);
            _phoneInput.Text = Convert.ToString(selected.Cells["Phone"].Value);
            _addressInput.Text = Convert.ToString(selected.Cells["Address"].Value);

            _passwordInput.Clear();
            _passwordConfirmationInput.Clear();
            _passwordInput.Enabled = false;
            _passwordConfirmationInput.Enabled = false;

            _selectedAccountLabel.Text =
                "Selected User ID " + _selectedUserId
                + " | " + roleName
                + " | " + Convert.ToString(selected.Cells["Status"].Value);
        }

        private void CreateButtonClick(object sender, EventArgs arguments)
        {
            try
            {
                if (_selectedUserId.HasValue)
                {
                    throw new InvalidOperationException(
                        "Click Clear before creating a new account.");
                }

                int newUserId = _authentication.Register(
                    Convert.ToString(_roleInput.SelectedItem),
                    _fullNameInput.Text,
                    _emailInput.Text,
                    _phoneInput.Text,
                    _addressInput.Text,
                    _passwordInput.Text,
                    _passwordConfirmationInput.Text);

                UiFactory.ShowSuccess(
                    this,
                    "Account created successfully with User ID " + newUserId + ".");

                ResetEditor();
                LoadAccounts();
            }
            catch (Exception exception)
            {
                UiFactory.ShowError(this, exception);
            }
        }

        private void UpdateButtonClick(object sender, EventArgs arguments)
        {
            try
            {
                int userId = RequireSelectedUser();

                _authentication.UpdateAccountProfile(
                    userId,
                    _fullNameInput.Text,
                    _emailInput.Text,
                    _phoneInput.Text,
                    _addressInput.Text);

                UiFactory.ShowSuccess(this, "The selected account was updated successfully.");
                ResetEditor();
                LoadAccounts();
            }
            catch (Exception exception)
            {
                UiFactory.ShowError(this, exception);
            }
        }

        private void DeactivateButtonClick(object sender, EventArgs arguments)
        {
            try
            {
                int userId = RequireSelectedUser();

                DialogResult confirmation = MessageBox.Show(
                    this,
                    "Deactivate User ID " + userId
                    + "? Existing transactions will be preserved.",
                    "Confirm Account Deactivation",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirmation != DialogResult.Yes)
                {
                    return;
                }

                _authentication.DeactivateAccount(userId);

                UiFactory.ShowSuccess(
                    this,
                    "The account was deactivated without deleting its transaction history.");

                ResetEditor();
                LoadAccounts();
            }
            catch (Exception exception)
            {
                UiFactory.ShowError(this, exception);
            }
        }

        private int RequireSelectedUser()
        {
            if (!_selectedUserId.HasValue)
            {
                throw new InvalidOperationException(
                    "Select an account from the grid before performing this action.");
            }

            return _selectedUserId.Value;
        }

        private void ResetEditor()
        {
            _selectedUserId = null;

            if (_roleInput.Items.Contains(UserRoles.Admin))
            {
                _roleInput.Items.Remove(UserRoles.Admin);
            }

            _roleInput.Enabled = true;
            _roleInput.SelectedItem = UserRoles.Client;
            _fullNameInput.Clear();
            _emailInput.Clear();
            _phoneInput.Clear();
            _addressInput.Clear();
            _passwordInput.Clear();
            _passwordConfirmationInput.Clear();
            _passwordInput.Enabled = true;
            _passwordConfirmationInput.Enabled = true;

            _selectedAccountLabel.Text =
                "New account mode: create a Client or Freelancer.";

            if (_accountsGrid != null)
            {
                _accountsGrid.ClearSelection();
            }

            _fullNameInput.Focus();
        }
    }
}
