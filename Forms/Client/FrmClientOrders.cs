using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SkillHub.Models;
using SkillHub.Repositories;
using SkillHub.UI;

namespace SkillHub.Forms.Client
{
    public sealed class FrmClientOrders : Form
    {
        private readonly OrderRepository _orderRepository;

        private ComboBox _statusFilter;
        private FlowLayoutPanel _ordersPanel;
        private Label _countLabel;

        public FrmClientOrders()
        {
            _orderRepository = new OrderRepository();

            InitializeForm();
            BuildInterface();
            LoadOrders();
        }

        private void InitializeForm()
        {
            Text = "SkillHub - My Orders";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(1150, 750);
            MinimumSize = new Size(900, 600);
            BackColor = MarketplaceTheme.PageBackground;
            Font = MarketplaceTheme.Body();
        }

        private void BuildInterface()
        {
            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                BackColor = MarketplaceTheme.White,
                Padding = new Padding(28, 20, 28, 15)
            };

            Label title = new Label
            {
                Text = "My Orders",
                Font = MarketplaceTheme.Heading(24F),
                ForeColor = MarketplaceTheme.DeepCharcoal,
                AutoSize = true,
                Location = new Point(28, 20)
            };

            _countLabel = new Label
            {
                Text = "Loading...",
                Font = MarketplaceTheme.Body(9.5F),
                ForeColor = MarketplaceTheme.MutedText,
                AutoSize = true,
                Location = new Point(30, 60)
            };

            Label filterLabel = new Label
            {
                Text = "Status:",
                Font = MarketplaceTheme.SubHeading(9.5F),
                ForeColor = MarketplaceTheme.DeepCharcoal,
                AutoSize = true,
                Location = new Point(720, 30),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            _statusFilter = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = MarketplaceTheme.Body(9.5F),
                BackColor = MarketplaceTheme.White,
                Width = 190,
                Location = new Point(770, 25),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            _statusFilter.Items.Add("All Orders");
            _statusFilter.Items.Add("Pending Payment");
            _statusFilter.Items.Add("Placed");
            _statusFilter.Items.Add("In Progress");
            _statusFilter.Items.Add("Delivered");
            _statusFilter.Items.Add("Completed");
            _statusFilter.Items.Add("Disputed");
            _statusFilter.Items.Add("Cancelled");
            _statusFilter.Items.Add("Refunded");

            _statusFilter.SelectedIndex = 0;
            _statusFilter.SelectedIndexChanged += StatusFilter_SelectedIndexChanged;

            header.Controls.Add(title);
            header.Controls.Add(_countLabel);
            header.Controls.Add(filterLabel);
            header.Controls.Add(_statusFilter);

            _ordersPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = false,
                FlowDirection = FlowDirection.TopDown,
                Padding = new Padding(28, 25, 28, 30),
                BackColor = MarketplaceTheme.PageBackground
            };

            Controls.Add(_ordersPanel);
            Controls.Add(header);
        }

        private void LoadOrders()
        {
            try
            {
                List<OrderModel> orders =
                    _orderRepository.GetClientOrders();

                RenderOrders(orders);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not load your orders." +
                    Environment.NewLine +
                    Environment.NewLine +
                    ex.Message,
                    "SkillHub",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void StatusFilter_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            try
            {
                string selectedStatus =
                    Convert.ToString(_statusFilter.SelectedItem);

                List<OrderModel> orders;

                if (selectedStatus == "All Orders")
                {
                    orders =
                        _orderRepository.GetClientOrders();
                }
                else
                {
                    orders =
                        _orderRepository.GetClientOrdersByStatus(
                            selectedStatus);
                }

                RenderOrders(orders);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not filter orders." +
                    Environment.NewLine +
                    Environment.NewLine +
                    ex.Message,
                    "SkillHub",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void RenderOrders(List<OrderModel> orders)
        {
            _ordersPanel.SuspendLayout();
            _ordersPanel.Controls.Clear();

            _countLabel.Text =
                orders.Count + " order(s)";

            if (orders.Count == 0)
            {
                Label empty = new Label
                {
                    Text =
                        "No orders found for the selected status.",
                    Font = MarketplaceTheme.Body(11F),
                    ForeColor = MarketplaceTheme.MutedText,
                    AutoSize = true,
                    Padding = new Padding(10)
                };

                _ordersPanel.Controls.Add(empty);
            }

            foreach (OrderModel order in orders)
            {
                _ordersPanel.Controls.Add(
                    CreateOrderCard(order));
            }

            _ordersPanel.ResumeLayout();
        }

        private RoundedPanel CreateOrderCard(OrderModel order)
        {
            RoundedPanel card = new RoundedPanel
            {
                Width = 930,
                Height = 185,
                Margin = new Padding(0, 0, 0, 18),
                BackColor = MarketplaceTheme.White,
                BorderThickness = 0,
                CornerRadius = 15
            };

            Label orderNumber = new Label
            {
                Text = "Order #" + order.OrderId,
                Font = MarketplaceTheme.SubHeading(13F),
                ForeColor = MarketplaceTheme.DeepCharcoal,
                AutoSize = true,
                Location = new Point(22, 20)
            };

            Label service = new Label
            {
                Text = order.ServiceTitle,
                Font = MarketplaceTheme.Body(10.5F),
                ForeColor = MarketplaceTheme.DeepCharcoal,
                AutoEllipsis = true,
                AutoSize = false,
                Location = new Point(22, 52),
                Size = new Size(430, 25)
            };

            Label freelancer = new Label
            {
                Text = "Freelancer: " +
                       order.FreelancerName,
                Font = MarketplaceTheme.Body(9.5F),
                ForeColor = MarketplaceTheme.MutedText,
                AutoEllipsis = true,
                AutoSize = false,
                Location = new Point(22, 82),
                Size = new Size(430, 25)
            };

            Label quantity = new Label
            {
                Text = "Quantity: " +
                       order.Quantity,
                Font = MarketplaceTheme.Body(9.5F),
                ForeColor = MarketplaceTheme.MutedText,
                AutoSize = true,
                Location = new Point(22, 112)
            };

            Label date = new Label
            {
                Text = "Created: " +
                       order.CreatedAt.ToString("dd MMM yyyy"),
                Font = MarketplaceTheme.Body(9.5F),
                ForeColor = MarketplaceTheme.MutedText,
                AutoSize = true,
                Location = new Point(22, 137)
            };

            Label amount = new Label
            {
                Text = "৳ " +
                       order.GrossAmount.ToString("N2"),
                Font = MarketplaceTheme.Heading(16F),
                ForeColor = MarketplaceTheme.Primary,
                AutoSize = true,
                Location = new Point(540, 22)
            };

            Label status = new Label
            {
                Text = order.OrderStatus,
                Font = MarketplaceTheme.ButtonFont(9F),
                ForeColor = MarketplaceTheme.Primary,
                BackColor = MarketplaceTheme.SoftGray,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(155, 32),
                Location = new Point(540, 62)
            };

            card.Controls.Add(orderNumber);
            card.Controls.Add(service);
            card.Controls.Add(freelancer);
            card.Controls.Add(quantity);
            card.Controls.Add(date);
            card.Controls.Add(amount);
            card.Controls.Add(status);

            int nextButtonX = 540;

            if (order.CanApproveCompletion)
            {
                ModernButton approveButton = new ModernButton
                {
                    Text = "Approve & Review",
                    Width = 155,
                    Height = 38,
                    Location = new Point(nextButtonX, 112)
                };

                approveButton.Click += delegate
                {
                    OpenReview(order);
                };

                card.Controls.Add(approveButton);

                nextButtonX += 170;
            }

            if (order.CanFileDispute)
            {
                ModernButton disputeButton = new ModernButton
                {
                    Text = "File Dispute",
                    Width = 125,
                    Height = 38,
                    Location = new Point(nextButtonX, 112)
                };

                disputeButton.IsSecondary = true;

                disputeButton.Click += delegate
                {
                    OpenDispute(order);
                };

                card.Controls.Add(disputeButton);
            }

            return card;
        }

        private void OpenReview(OrderModel order)
        {
            try
            {
                _orderRepository.ApproveCompletion(order.OrderId);

                OrderModel completedOrder =
                    _orderRepository.GetClientOrderById(order.OrderId);

                if (completedOrder == null)
                {
                    throw new InvalidOperationException(
                        "The completed order could not be loaded.");
                }

                using (FrmReview review =
                       new FrmReview(completedOrder))
                {
                    if (review.ShowDialog(this) ==
                        DialogResult.OK)
                    {
                        LoadOrders();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "The order could not be approved." +
                    Environment.NewLine +
                    Environment.NewLine +
                    ex.Message,
                    "SkillHub",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void OpenDispute(OrderModel order)
        {
            using (FrmDispute dispute =
                   new FrmDispute(order))
            {
                if (dispute.ShowDialog(this) ==
                    DialogResult.OK)
                {
                    LoadOrders();
                }
            }
        }
    }
}