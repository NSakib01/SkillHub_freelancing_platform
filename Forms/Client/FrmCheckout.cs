using System;
using System.Drawing;
using System.Windows.Forms;
using SkillHub.Services;
using SkillHub.UI;

namespace SkillHub.Forms.Client
{
    public sealed class FrmCheckout : Form
    {
        private Label _totalLabel;
        private Label _statusLabel;
        private ModernButton _placeOrderButton;

        public FrmCheckout()
        {
            InitializeForm();
            BuildInterface();
            LoadCartSummary();
        }

        private void InitializeForm()
        {
            Text = "SkillHub - Checkout";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(650, 500);
            MinimumSize = new Size(550, 420);
            BackColor = MarketplaceTheme.PageBackground;
            Font = MarketplaceTheme.Body();
        }

        private void BuildInterface()
        {
            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = MarketplaceTheme.White
            };

            Label title = new Label
            {
                Text = "Checkout",
                Font = MarketplaceTheme.Heading(24F),
                ForeColor = MarketplaceTheme.DeepCharcoal,
                AutoSize = true,
                Location = new Point(30, 24)
            };

            header.Controls.Add(title);

            RoundedPanel card = new RoundedPanel
            {
                Location = new Point(30, 105),
                Size = new Size(570, 260),
                BackColor = MarketplaceTheme.White,
                BorderThickness = 0,
                CornerRadius = 16,
                Anchor =
                    AnchorStyles.Top |
                    AnchorStyles.Left |
                    AnchorStyles.Right
            };

            Label informationTitle = new Label
            {
                Text = "Order Summary",
                Font = MarketplaceTheme.SubHeading(15F),
                ForeColor = MarketplaceTheme.DeepCharcoal,
                AutoSize = true,
                Location = new Point(25, 25)
            };

            Label information = new Label
            {
                Text =
                    "Your cart will be processed securely.\r\n\r\n" +
                    "If your cart contains services from multiple freelancers,\r\n" +
                    "SkillHub will automatically create separate orders.\r\n\r\n" +
                    "Payment will be recorded for each order.",
                Font = MarketplaceTheme.Body(10.5F),
                ForeColor = MarketplaceTheme.MutedText,
                AutoSize = true,
                Location = new Point(25, 65)
            };

            _totalLabel = new Label
            {
                Text = "Total: ৳ 0.00",
                Font = MarketplaceTheme.Heading(19F),
                ForeColor = MarketplaceTheme.Primary,
                AutoSize = true,
                Location = new Point(25, 180)
            };

            card.Controls.Add(informationTitle);
            card.Controls.Add(information);
            card.Controls.Add(_totalLabel);

            _statusLabel = new Label
            {
                Text = "",
                Font = MarketplaceTheme.Body(10F),
                ForeColor = MarketplaceTheme.MutedText,
                AutoSize = false,
                Location = new Point(30, 380),
                Size = new Size(570, 40),
                Anchor =
                    AnchorStyles.Left |
                    AnchorStyles.Right |
                    AnchorStyles.Bottom
            };

            _placeOrderButton = new ModernButton
            {
                Text = "Place Order",
                Width = 180,
                Height = 45,
                Anchor =
                    AnchorStyles.Bottom |
                    AnchorStyles.Right
            };

            _placeOrderButton.Location = new Point(
                ClientSize.Width - 210,
                ClientSize.Height - 65);

            _placeOrderButton.Click += PlaceOrderButton_Click;

            Controls.Add(_placeOrderButton);
            Controls.Add(_statusLabel);
            Controls.Add(card);
            Controls.Add(header);
        }

        private void LoadCartSummary()
        {
            _totalLabel.Text =
                "Total will be calculated at checkout.";
        }

        private void PlaceOrderButton_Click(
            object sender,
            EventArgs e)
        {
            DialogResult confirmation =
                MessageBox.Show(
                    "Are you sure you want to place this order?",
                    "Confirm Checkout",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

            if (confirmation != DialogResult.Yes)
            {
                return;
            }

            try
            {
                _placeOrderButton.Enabled = false;

                _statusLabel.Text =
                    "Processing your order...";

                Cursor = Cursors.WaitCursor;

                CheckoutService checkoutService =
                    new CheckoutService();

                CheckoutResult result =
                    checkoutService.Checkout();

                Cursor = Cursors.Default;

                _totalLabel.Text =
                    "Total: ৳ " +
                    result.TotalAmount.ToString("N2");

                _statusLabel.ForeColor =
                    MarketplaceTheme.Primary;

                _statusLabel.Text =
                    result.Message;

                MessageBox.Show(
                    "Checkout completed successfully." +
                    Environment.NewLine +
                    Environment.NewLine +
                    "Orders created: " +
                    result.OrderIds.Count +
                    Environment.NewLine +
                    "Total paid: ৳ " +
                    result.TotalAmount.ToString("N2"),
                    "Order Placed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;

                _placeOrderButton.Enabled = true;

                _statusLabel.ForeColor =
                    MarketplaceTheme.Danger;

                _statusLabel.Text =
                    "Checkout could not be completed.";

                MessageBox.Show(
                    "Checkout failed." +
                    Environment.NewLine +
                    Environment.NewLine +
                    ex.Message,
                    "Checkout Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}