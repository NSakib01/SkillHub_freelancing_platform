using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SkillHub.Models;
using SkillHub.Repositories;
using SkillHub.UI;

namespace SkillHub.Forms.Client
{
    public sealed class FrmCart : Form
    {
        private readonly CartRepository _cartRepository;

        private FlowLayoutPanel _itemsPanel;
        private Label _totalLabel;
        private Label _countLabel;

        public FrmCart()
        {
            _cartRepository = new CartRepository();

            InitializeForm();
            BuildInterface();
            LoadCart();
        }

        private void InitializeForm()
        {
            Text = "SkillHub - My Cart";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(1100, 720);
            MinimumSize = new Size(850, 600);
            BackColor = MarketplaceTheme.PageBackground;
            Font = MarketplaceTheme.Body();
        }

        private void BuildInterface()
        {
            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 82,
                BackColor = MarketplaceTheme.White
            };

            Label title = new Label
            {
                Text = "My Cart",
                Font = MarketplaceTheme.Heading(24F),
                ForeColor = MarketplaceTheme.DeepCharcoal,
                AutoSize = true,
                Location = new Point(30, 25)
            };

            _countLabel = new Label
            {
                Text = "",
                Font = MarketplaceTheme.Body(10F),
                ForeColor = MarketplaceTheme.MutedText,
                AutoSize = true,
                Location = new Point(160, 34)
            };

            header.Controls.Add(title);
            header.Controls.Add(_countLabel);

            Panel bottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 105,
                BackColor = MarketplaceTheme.White,
                Padding = new Padding(28, 18, 28, 18)
            };

            _totalLabel = new Label
            {
                Text = "Total: ৳ 0.00",
                Font = MarketplaceTheme.Heading(18F),
                ForeColor = MarketplaceTheme.DeepCharcoal,
                AutoSize = true,
                Location = new Point(30, 25)
            };

            ModernButton clearButton = new ModernButton
            {
                Text = "Clear Cart",
                Width = 125,
                Height = 42
            };

            clearButton.IsSecondary = true;

            clearButton.Location = new Point(
                bottom.ClientSize.Width - 300,
                30);

            clearButton.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Right;

            clearButton.Click += ClearButton_Click;

            ModernButton checkoutButton = new ModernButton
            {
                Text = "Proceed to Checkout",
                Width = 180,
                Height = 42
            };

            checkoutButton.Location = new Point(
                bottom.ClientSize.Width - 205,
                30);

            checkoutButton.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Right;

            checkoutButton.Click += CheckoutButton_Click;

            bottom.Controls.Add(_totalLabel);
            bottom.Controls.Add(clearButton);
            bottom.Controls.Add(checkoutButton);

            _itemsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = false,
                FlowDirection = FlowDirection.TopDown,
                Padding = new Padding(28, 25, 28, 25),
                BackColor = MarketplaceTheme.PageBackground
            };

            Controls.Add(_itemsPanel);
            Controls.Add(bottom);
            Controls.Add(header);
        }

        private void LoadCart()
        {
            try
            {
                List<CartItem> items =
                    _cartRepository.GetCartItems();

                RenderCart(items);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not load your cart." +
                    Environment.NewLine +
                    Environment.NewLine +
                    ex.Message,
                    "SkillHub",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void RenderCart(List<CartItem> items)
        {
            _itemsPanel.SuspendLayout();
            _itemsPanel.Controls.Clear();

            decimal total = 0m;
            int quantity = 0;

            foreach (CartItem item in items)
            {
                total += item.Subtotal;
                quantity += item.Quantity;

                _itemsPanel.Controls.Add(
                    CreateCartItemCard(item));
            }

            _countLabel.Text =
                quantity + " item(s)";

            _totalLabel.Text =
                "Total: ৳ " + total.ToString("N2");

            if (items.Count == 0)
            {
                Label empty = new Label
                {
                    Text = "Your cart is empty.\r\nBrowse services and add something you like.",
                    Font = MarketplaceTheme.Body(12F),
                    ForeColor = MarketplaceTheme.MutedText,
                    AutoSize = true,
                    Padding = new Padding(10)
                };

                _itemsPanel.Controls.Add(empty);
            }

            _itemsPanel.ResumeLayout();
        }

        private RoundedPanel CreateCartItemCard(CartItem item)
        {
            RoundedPanel card = new RoundedPanel
            {
                Width = 900,
                Height = 135,
                Margin = new Padding(0, 0, 0, 15),
                BackColor = MarketplaceTheme.White,
                BorderThickness = 0,
                CornerRadius = 15
            };

            Label serviceTitle = new Label
            {
                Text = item.ServiceTitle,
                Font = MarketplaceTheme.SubHeading(13F),
                ForeColor = MarketplaceTheme.DeepCharcoal,
                AutoEllipsis = true,
                AutoSize = false,
                Location = new Point(22, 20),
                Size = new Size(400, 28)
            };

            Label freelancer = new Label
            {
                Text = "Freelancer: " + item.FreelancerName,
                Font = MarketplaceTheme.Body(9.5F),
                ForeColor = MarketplaceTheme.MutedText,
                AutoEllipsis = true,
                AutoSize = false,
                Location = new Point(22, 52),
                Size = new Size(400, 24)
            };

            Label price = new Label
            {
                Text = "৳ " + item.UnitPrice.ToString("N2") + " each",
                Font = MarketplaceTheme.Body(10F),
                ForeColor = MarketplaceTheme.MutedText,
                AutoSize = true,
                Location = new Point(22, 82)
            };

            Label subtotal = new Label
            {
                Text = "৳ " + item.Subtotal.ToString("N2"),
                Font = MarketplaceTheme.Heading(15F),
                ForeColor = MarketplaceTheme.Primary,
                AutoSize = true,
                Location = new Point(610, 25)
            };

            Label quantityLabel = new Label
            {
                Text = item.Quantity.ToString(),
                Font = MarketplaceTheme.SubHeading(11F),
                ForeColor = MarketplaceTheme.DeepCharcoal,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(45, 35),
                Location = new Point(650, 70)
            };

            ModernButton minusButton = new ModernButton
            {
                Text = "−",
                Width = 38,
                Height = 35,
                Location = new Point(605, 70)
            };

            ModernButton plusButton = new ModernButton
            {
                Text = "+",
                Width = 38,
                Height = 35,
                Location = new Point(702, 70)
            };

            ModernButton removeButton = new ModernButton
            {
                Text = "Remove",
                Width = 105,
                Height = 35,
                Location = new Point(770, 70)
            };

            removeButton.IsSecondary = true;

            int cartItemId = item.CartItemId;
            int currentQuantity = item.Quantity;

            minusButton.Click += delegate
            {
                if (currentQuantity > 1)
                {
                    try
                    {
                        _cartRepository.UpdateQuantity(
                            cartItemId,
                            currentQuantity - 1);

                        LoadCart();
                    }
                    catch (Exception ex)
                    {
                        ShowCartError(ex);
                    }
                }
            };

            plusButton.Click += delegate
            {
                try
                {
                    _cartRepository.UpdateQuantity(
                        cartItemId,
                        currentQuantity + 1);

                    LoadCart();
                }
                catch (Exception ex)
                {
                    ShowCartError(ex);
                }
            };

            removeButton.Click += delegate
            {
                try
                {
                    _cartRepository.RemoveItem(cartItemId);
                    LoadCart();
                }
                catch (Exception ex)
                {
                    ShowCartError(ex);
                }
            };

            card.Controls.Add(serviceTitle);
            card.Controls.Add(freelancer);
            card.Controls.Add(price);
            card.Controls.Add(subtotal);
            card.Controls.Add(minusButton);
            card.Controls.Add(quantityLabel);
            card.Controls.Add(plusButton);
            card.Controls.Add(removeButton);

            return card;
        }

        private void ClearButton_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                List<CartItem> items =
                    _cartRepository.GetCartItems();

                if (items.Count == 0)
                {
                    MessageBox.Show(
                        "Your cart is already empty.",
                        "SkillHub",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    return;
                }

                DialogResult result = MessageBox.Show(
                    "Are you sure you want to clear your cart?",
                    "Clear Cart",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                {
                    return;
                }

                _cartRepository.ClearCart();

                LoadCart();
            }
            catch (Exception ex)
            {
                ShowCartError(ex);
            }
        }

        private void CheckoutButton_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                List<CartItem> items =
                    _cartRepository.GetCartItems();

                if (items.Count == 0)
                {
                    MessageBox.Show(
                        "Your cart is empty.",
                        "Checkout",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    return;
                }

                using (FrmCheckout checkout =
                       new FrmCheckout())
                {
                    checkout.ShowDialog(this);
                }

                LoadCart();
            }
            catch (Exception ex)
            {
                ShowCartError(ex);
            }
        }

        private void ShowCartError(Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Cart Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
    }
}