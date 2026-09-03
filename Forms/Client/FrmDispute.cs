using System;
using System.Drawing;
using System.Windows.Forms;
using SkillHub.Models;
using SkillHub.Repositories;
using SkillHub.UI;

namespace SkillHub.Forms.Client
{
    public sealed class FrmDispute : Form
    {
        private readonly OrderModel _order;
        private readonly DisputeRepository _disputeRepository;

        private TextBox _reasonInput;

        public FrmDispute(OrderModel order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            _order = order;
            _disputeRepository = new DisputeRepository();

            InitializeForm();
            BuildInterface();
        }

        private void InitializeForm()
        {
            Text = "SkillHub - File Dispute";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(620, 500);
            MinimumSize = new Size(540, 440);
            BackColor = MarketplaceTheme.PageBackground;
            Font = MarketplaceTheme.Body();
        }

        private void BuildInterface()
        {
            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 85,
                BackColor = MarketplaceTheme.White
            };

            Label title = new Label
            {
                Text = "File a Dispute",
                Font = MarketplaceTheme.Heading(22F),
                ForeColor = MarketplaceTheme.DeepCharcoal,
                AutoSize = true,
                Location = new Point(28, 22)
            };

            header.Controls.Add(title);

            RoundedPanel card = new RoundedPanel
            {
                Location = new Point(30, 110),
                Size = new Size(540, 285),
                BackColor = MarketplaceTheme.White,
                BorderThickness = 0,
                CornerRadius = 15
            };

            Label orderLabel = new Label
            {
                Text = "Order #" + _order.OrderId,
                Font = MarketplaceTheme.SubHeading(13F),
                ForeColor = MarketplaceTheme.DeepCharcoal,
                AutoSize = true,
                Location = new Point(25, 22)
            };

            Label serviceLabel = new Label
            {
                Text = "Service: " + _order.ServiceTitle,
                Font = MarketplaceTheme.Body(10F),
                ForeColor = MarketplaceTheme.MutedText,
                AutoEllipsis = true,
                AutoSize = false,
                Location = new Point(25, 53),
                Size = new Size(485, 25)
            };

            Label instruction = new Label
            {
                Text = "Please explain the issue you are experiencing.",
                Font = MarketplaceTheme.SubHeading(10F),
                ForeColor = MarketplaceTheme.DeepCharcoal,
                AutoSize = true,
                Location = new Point(25, 90)
            };

            _reasonInput = new TextBox
            {
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Font = MarketplaceTheme.Body(10F),
                Location = new Point(25, 120),
                Size = new Size(485, 105),
                MaxLength = 2000
            };

            Label hint = new Label
            {
                Text = "Maximum 2000 characters.",
                Font = MarketplaceTheme.Body(9F),
                ForeColor = MarketplaceTheme.MutedText,
                AutoSize = true,
                Location = new Point(25, 238)
            };

            card.Controls.Add(orderLabel);
            card.Controls.Add(serviceLabel);
            card.Controls.Add(instruction);
            card.Controls.Add(_reasonInput);
            card.Controls.Add(hint);

            ModernButton cancelButton = new ModernButton
            {
                Text = "Cancel",
                Width = 110,
                Height = 42,
                Location = new Point(330, 415)
            };

            cancelButton.IsSecondary = true;

            cancelButton.Click += delegate
            {
                DialogResult = DialogResult.Cancel;
                Close();
            };

            ModernButton submitButton = new ModernButton
            {
                Text = "Submit Dispute",
                Width = 145,
                Height = 42,
                Location = new Point(450, 415)
            };

            submitButton.Click += SubmitButton_Click;

            Controls.Add(submitButton);
            Controls.Add(cancelButton);
            Controls.Add(card);
            Controls.Add(header);
        }

        private void SubmitButton_Click(
            object sender,
            EventArgs e)
        {
            string reason =
                _reasonInput.Text == null
                    ? string.Empty
                    : _reasonInput.Text.Trim();

            if (reason.Length == 0)
            {
                MessageBox.Show(
                    "Please explain the reason for the dispute.",
                    "SkillHub",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                _reasonInput.Focus();
                return;
            }

            try
            {
                DisputeModel dispute = new DisputeModel
                {
                    OrderId = _order.OrderId,
                    OpenedBy = _order.ClientId,
                    Reason = reason,
                    Status = "Open"
                };

                _disputeRepository.AddDispute(dispute);

                MessageBox.Show(
                    "Your dispute has been submitted successfully.",
                    "SkillHub",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "The dispute could not be submitted." +
                    Environment.NewLine +
                    Environment.NewLine +
                    ex.Message,
                    "Dispute Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}