using SkillHub.Data;
using SkillHub.Forms.Common;
using SkillHub.Services;
using SkillHub.Utilities;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace SkillHub.Forms.Admin
{
    /// <summary>
    /// Super-admin platform configuration.
    /// </summary>
    public sealed class FrmPlatformSettings : Form
    {
        private readonly DatabaseConnection _database;

        private DataGridView _settingsGrid;

        private TextBox _keyInput;
        private TextBox _valueInput;
        private TextBox _descriptionInput;

        private Button _updateButton;

        public FrmPlatformSettings()
        {
            AuthorizationService.DemandAdmin();

            _database = new DatabaseConnection();

            Text = "SkillHub | Platform Settings";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(1100, 700);
            MinimumSize = new Size(900, 600);

            BuildLayout();
            LoadSettings();

            UiFactory.AddBackToDashboardButton(this);
        }

        private void BuildLayout()
        {
            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };

            Label heading = new Label
            {
                AutoSize = true,
                Text = "Platform Settings",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Location = new Point(20, 20)
            };

            Label caption = new Label
            {
                AutoSize = true,
                Text = "Manage configurable SkillHub platform settings and commission percentage.",
                ForeColor = Color.Gray,
                Location = new Point(23, 58)
            };

            _settingsGrid = new DataGridView
            {
                Location = new Point(20, 100),
                Width = 1020,
                Height = 330,
                Anchor = AnchorStyles.Top
                    | AnchorStyles.Bottom
                    | AnchorStyles.Left
                    | AnchorStyles.Right,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode =
                    DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };

            _settingsGrid.SelectionChanged += delegate
            {
                LoadSelectedSetting();
            };

            GroupBox editor = new GroupBox
            {
                Text = "Setting Editor",
                Location = new Point(20, 445),
                Width = 1020,
                Height = 150,
                Anchor = AnchorStyles.Left
                    | AnchorStyles.Right
                    | AnchorStyles.Bottom
            };

            Label keyLabel = new Label
            {
                AutoSize = true,
                Text = "Setting Key",
                Location = new Point(15, 25)
            };

            _keyInput = new TextBox
            {
                Width = 230,
                Location = new Point(15, 48),
                ReadOnly = true
            };

            Label valueLabel = new Label
            {
                AutoSize = true,
                Text = "Setting Value",
                Location = new Point(260, 25)
            };

            _valueInput = new TextBox
            {
                Width = 230,
                Location = new Point(260, 48)
            };

            Label descriptionLabel = new Label
            {
                AutoSize = true,
                Text = "Description",
                Location = new Point(505, 25)
            };

            _descriptionInput = new TextBox
            {
                Width = 300,
                Location = new Point(505, 48),
                ReadOnly = true
            };

            _updateButton = new Button
            {
                Text = "Update",
                Width = 100,
                Height = 32,
                Location = new Point(820, 46)
            };

            _updateButton.Click += delegate
            {
                UpdateSetting();
            };

            Button clearButton = new Button
            {
                Text = "Clear",
                Width = 80,
                Height = 32,
                Location = new Point(925, 46)
            };

            clearButton.Click += delegate
            {
                ClearEditor();
            };

            editor.Controls.Add(keyLabel);
            editor.Controls.Add(_keyInput);
            editor.Controls.Add(valueLabel);
            editor.Controls.Add(_valueInput);
            editor.Controls.Add(descriptionLabel);
            editor.Controls.Add(_descriptionInput);
            editor.Controls.Add(_updateButton);
            editor.Controls.Add(clearButton);

            mainPanel.Controls.Add(heading);
            mainPanel.Controls.Add(caption);
            mainPanel.Controls.Add(_settingsGrid);
            mainPanel.Controls.Add(editor);

            Controls.Add(mainPanel);
        }

        private void LoadSettings()
        {
            try
            {
                const string query = @"
SELECT
    SettingKey,
    SettingValue,
    Description,
    UpdatedAt,
    UpdatedBy
FROM dbo.PlatformSettings
ORDER BY SettingKey;
";

                DataTable table = new DataTable();

                using (SqlConnection connection =
                    _database.OpenConnection())
                using (SqlCommand command =
                    new SqlCommand(query, connection))
                using (SqlDataAdapter adapter =
                    new SqlDataAdapter(command))
                {
                    adapter.Fill(table);
                }

                _settingsGrid.DataSource = table;
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "Unable to load platform settings.\n\n"
                    + exception.Message,
                    "Settings Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LoadSelectedSetting()
        {
            if (_settingsGrid.CurrentRow == null)
            {
                return;
            }

            DataGridViewRow row =
                _settingsGrid.CurrentRow;

            if (row.Cells["SettingKey"].Value == null)
            {
                return;
            }

            _keyInput.Text =
                row.Cells["SettingKey"].Value.ToString();

            _valueInput.Text =
                row.Cells["SettingValue"].Value.ToString();

            _descriptionInput.Text =
                row.Cells["Description"].Value == DBNull.Value
                    ? ""
                    : row.Cells["Description"].Value.ToString();
        }

        private void UpdateSetting()
        {
            string key = _keyInput.Text.Trim();
            string value = _valueInput.Text.Trim();

            if (key == "")
            {
                MessageBox.Show(
                    "Please select a setting.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (value == "")
            {
                MessageBox.Show(
                    "Setting value cannot be empty.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (key == "CommissionPercent")
            {
                decimal commission;

                if (!decimal.TryParse(
                    value,
                    out commission))
                {
                    MessageBox.Show(
                        "Commission percentage must be a valid number.",
                        "Validation",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                if (commission < 0 || commission > 100)
                {
                    MessageBox.Show(
                        "Commission percentage must be between 0 and 100.",
                        "Validation",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                value = commission.ToString("0.00");
            }

            DialogResult confirmation =
                MessageBox.Show(
                    "Update platform setting '" + key + "'?",
                    "Confirm Update",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

            if (confirmation != DialogResult.Yes)
            {
                return;
            }

            const string query = @"
UPDATE dbo.PlatformSettings
SET
    SettingValue = @SettingValue,
    UpdatedAt = SYSDATETIME(),
    UpdatedBy = @UpdatedBy
WHERE SettingKey = @SettingKey;
";

            try
            {
                using (SqlConnection connection =
                    _database.OpenConnection())
                using (SqlCommand command =
                    new SqlCommand(query, connection))
                {
                    DatabaseConnection.AddParameter(
                        command,
                        "@SettingValue",
                        SqlDbType.NVarChar,
                        value,
                        250);

                    DatabaseConnection.AddParameter(
                        command,
                        "@UpdatedBy",
                        SqlDbType.Int,
                        UserSession.UserId);

                    DatabaseConnection.AddParameter(
                        command,
                        "@SettingKey",
                        SqlDbType.NVarChar,
                        key,
                        80);

                    command.ExecuteNonQuery();
                }

                MessageBox.Show(
                    "Platform setting updated successfully.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadSettings();
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "Unable to update platform setting.\n\n"
                    + exception.Message,
                    "Settings Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ClearEditor()
        {
            _keyInput.Clear();
            _valueInput.Clear();
            _descriptionInput.Clear();

            _settingsGrid.ClearSelection();
        }
    }
}