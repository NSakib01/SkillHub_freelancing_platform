using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace SkillHub.Forms.Common
{
    internal static class UiFactory
    {
        public static readonly Color PageBackground = Color.FromArgb(244, 247, 252);
        public static readonly Color CardBackground = Color.White;
        public static readonly Color Primary = Color.FromArgb(29, 78, 137);
        public static readonly Color PrimaryDark = Color.FromArgb(22, 55, 96);
        public static readonly Color Accent = Color.FromArgb(20, 145, 120);
        public static readonly Color Text = Color.FromArgb(32, 41, 55);
        public static readonly Color MutedText = Color.FromArgb(94, 108, 128);
        public static readonly Color Border = Color.FromArgb(222, 228, 238);
        public static readonly Color Danger = Color.FromArgb(183, 53, 65);

        public static void ConfigureForm(Form form, string title, Size minimumSize)
        {
            form.Text = title;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimumSize = minimumSize;
            form.BackColor = PageBackground;
            form.Font = new Font(
                "Segoe UI",
                10F,
                FontStyle.Regular,
                GraphicsUnit.Point);
            form.AutoScaleMode = AutoScaleMode.Font;
        }

        public static Label CreateHeading(string text, int size = 22)
        {
            return new Label
            {
                AutoSize = true,
                Font = new Font(
                    "Segoe UI",
                    size,
                    FontStyle.Bold),
                ForeColor = Text,
                Text = text,
                Margin = new Padding(0, 0, 0, 8)
            };
        }

        public static Label CreateCaption(string text)
        {
            return new Label
            {
                AutoSize = false,
                Height = 48,
                Width = 420,
                ForeColor = MutedText,
                Text = text,
                Margin = new Padding(0, 0, 0, 8)
            };
        }

        public static Label CreateFieldLabel(string text)
        {
            return new Label
            {
                AutoSize = true,
                Font = new Font(
                    "Segoe UI",
                    9.5F,
                    FontStyle.Bold),
                ForeColor = Text,
                Text = text,
                Margin = new Padding(0, 8, 0, 4)
            };
        }

        public static TextBox CreateTextBox(
            int width,
            bool password = false)
        {
            return new TextBox
            {
                Width = width,
                Height = 35,
                Font = new Font(
                    "Segoe UI",
                    11F,
                    FontStyle.Regular),
                BorderStyle = BorderStyle.FixedSingle,
                UseSystemPasswordChar = password,
                Margin = new Padding(0, 0, 0, 10)
            };
        }

        public static Button CreateButton(
            string text,
            bool primary = true,
            int width = 160,
            int height = 40)
        {
            Color backColor =
                primary ? Primary : Color.White;

            Color foreColor =
                primary ? Color.White : PrimaryDark;

            Button button = new Button
            {
                Text = text,
                Width = width,
                Height = height,
                BackColor = backColor,
                ForeColor = foreColor,
                FlatStyle = FlatStyle.Flat,
                Font = new Font(
                    "Segoe UI",
                    9.5F,
                    FontStyle.Bold),
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 6, 12, 6),
                UseVisualStyleBackColor = false
            };

            button.FlatAppearance.BorderSize =
                primary ? 0 : 1;

            button.FlatAppearance.BorderColor = Border;

            return button;
        }

        public static Button CreateDangerButton(
            string text,
            int width = 160)
        {
            Button button =
                CreateButton(text, true, width);

            button.BackColor = Danger;

            return button;
        }

        public static void AddBackToDashboardButton(Form form)
        {
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            Button backButton = CreateButton(
                "← Back to Dashboard",
                false,
                180,
                38);

            backButton.Anchor =
                AnchorStyles.Bottom |
                AnchorStyles.Right;

            backButton.Click += delegate
            {
                form.Close();
            };

            backButton.Location = new Point(
                form.ClientSize.Width
                - backButton.Width
                - 20,
                form.ClientSize.Height
                - backButton.Height
                - 20);

            form.Controls.Add(backButton);
            backButton.BringToFront();
        }

        public static Panel CreateCard(
            int width,
            int height)
        {
            Panel panel = new Panel
            {
                Width = width,
                Height = height,
                BackColor = CardBackground,
                Padding = new Padding(24),
                Margin = new Padding(0, 0, 0, 18)
            };

            panel.Paint += delegate (
                object sender,
                PaintEventArgs arguments)
            {
                using (Pen pen = new Pen(Border))
                {
                    Rectangle border = new Rectangle(
                        0,
                        0,
                        panel.ClientSize.Width - 1,
                        panel.ClientSize.Height - 1);

                    arguments.Graphics.DrawRectangle(
                        pen,
                        border);
                }
            };

            return panel;
        }

        public static DataGridView CreateReadOnlyGrid()
        {
            DataGridView grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.DisplayedCells,
                SelectionMode =
                    DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                EnableHeadersVisualStyles = false,
                GridColor = Border,
                ColumnHeadersHeight = 42,
                RowTemplate = { Height = 36 }
            };

            grid.ColumnHeadersDefaultCellStyle.BackColor =
                PrimaryDark;

            grid.ColumnHeadersDefaultCellStyle.ForeColor =
                Color.White;

            grid.ColumnHeadersDefaultCellStyle.Font =
                new Font(
                    "Segoe UI",
                    9F,
                    FontStyle.Bold);

            grid.DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(224, 236, 248);

            grid.DefaultCellStyle.SelectionForeColor =
                Text;

            grid.DefaultCellStyle.ForeColor =
                Text;

            grid.AlternatingRowsDefaultCellStyle.BackColor =
                Color.FromArgb(247, 249, 252);

            return grid;
        }

        public static void ShowError(
            IWin32Window owner,
            Exception exception)
        {
            string message;

            if (exception is SqlException)
            {
                message =
                    "A database operation could not be completed. "
                    + "Verify that SkillHubDB was created "
                    + "and the connection is available.";
            }
            else if (exception is ConfigurationErrorsException)
            {
                message =
                    "The application configuration is missing "
                    + "or invalid. Check the SkillHubConnection "
                    + "entry in App.config.";
            }
            else
            {
                message = exception.Message;
            }

            MessageBox.Show(
                owner,
                message,
                "SkillHub",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        public static void ShowSuccess(
            IWin32Window owner,
            string message)
        {
            MessageBox.Show(
                owner,
                message,
                "SkillHub",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}