using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using SkillHub.Models;
using SkillHub.Repositories;
using SkillHub.UI;

namespace SkillHub.Forms.Client
{
    public sealed class FrmBrowseServices : Form
    {
        private readonly ServiceRepository _serviceRepository;
        private readonly CartRepository _cartRepository;

        private TextBox _searchBox;
        private FlowLayoutPanel _servicesPanel;
        private Label _resultLabel;

        public FrmBrowseServices()
        {
            _serviceRepository = new ServiceRepository();
            _cartRepository = new CartRepository();

            InitializeForm();
            BuildInterface();
            LoadServices();
        }

        private void InitializeForm()
        {
            Text = "SkillHub - Browse Services";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(1200, 780);
            MinimumSize = new Size(950, 650);
            BackColor = MarketplaceTheme.PageBackground;
            Font = MarketplaceTheme.Body();
        }

        private void BuildInterface()
        {
            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 82,
                BackColor = MarketplaceTheme.White,
                Padding = new Padding(28, 18, 28, 12)
            };

            Label logo = new Label
            {
                Text = "SkillHub",
                Font = MarketplaceTheme.Heading(22F),
                ForeColor = MarketplaceTheme.Primary,
                AutoSize = true,
                Location = new Point(28, 24)
            };

            ModernButton cartButton = new ModernButton
            {
                Text = "View Cart",
                Width = 130,
                Height = 40
            };

            cartButton.Location = new Point(
                header.ClientSize.Width - 158,
                21);

            cartButton.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Right;

            cartButton.Click += CartButton_Click;

            header.Controls.Add(logo);
            header.Controls.Add(cartButton);

            Panel heroHost = new Panel
            {
                Dock = DockStyle.Top,
                Height = 215,
                BackColor = MarketplaceTheme.PageBackground,
                Padding = new Padding(24, 16, 24, 14)
            };

            RoundedPanel hero = new RoundedPanel
            {
                Dock = DockStyle.Fill,
                BackColor = MarketplaceTheme.White,
                BorderThickness = 0,
                CornerRadius = 18,
                Padding = new Padding(32)
            };

            Label heroTitle = new Label
            {
                Text = "Are you looking for Freelancers?",
                Font = MarketplaceTheme.Heading(30F),
                ForeColor = MarketplaceTheme.DeepCharcoal,
                AutoSize = true,
                Location = new Point(32, 25)
            };

            Label heroSubtitle = new Label
            {
                Text = "Find talented freelancers and professional services for your next project.",
                Font = MarketplaceTheme.Body(11F),
                ForeColor = MarketplaceTheme.MutedText,
                AutoSize = true,
                Location = new Point(35, 72)
            };

            _searchBox = new TextBox
            {
                Font = MarketplaceTheme.Body(11F),
                ForeColor = MarketplaceTheme.DeepCharcoal,
                BackColor = MarketplaceTheme.SoftGray,
                BorderStyle = BorderStyle.None,
                Location = new Point(35, 112),
                Height = 40,
                Anchor =
                    AnchorStyles.Top |
                    AnchorStyles.Left |
                    AnchorStyles.Right
            };

            _searchBox.Width = hero.ClientSize.Width - 70;

            _searchBox.TextChanged += SearchBox_TextChanged;

            hero.Resize += delegate
            {
                _searchBox.Width = hero.ClientSize.Width - 70;
            };

            hero.Controls.Add(heroTitle);
            hero.Controls.Add(heroSubtitle);
            hero.Controls.Add(_searchBox);

            heroHost.Controls.Add(hero);

            Panel contentHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 55,
                BackColor = MarketplaceTheme.PageBackground,
                Padding = new Padding(28, 12, 28, 8)
            };

            Label servicesTitle = new Label
            {
                Text = "Available Services",
                Font = MarketplaceTheme.SubHeading(15F),
                ForeColor = MarketplaceTheme.DeepCharcoal,
                AutoSize = true,
                Location = new Point(28, 14)
            };

            _resultLabel = new Label
            {
                Text = "Loading...",
                Font = MarketplaceTheme.Body(9.5F),
                ForeColor = MarketplaceTheme.MutedText,
                AutoSize = true
            };

            contentHeader.Controls.Add(servicesTitle);
            contentHeader.Controls.Add(_resultLabel);

            contentHeader.Resize += delegate
            {
                _resultLabel.Location = new Point(
                    contentHeader.ClientSize.Width -
                    _resultLabel.Width -
                    28,
                    17);
            };

            _servicesPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(28, 10, 28, 30),
                BackColor = MarketplaceTheme.PageBackground
            };

            Controls.Add(_servicesPanel);
            Controls.Add(contentHeader);
            Controls.Add(heroHost);
            Controls.Add(header);
        }

        private void LoadServices()
        {
            try
            {
                List<ServiceCatalogItem> services =
                    _serviceRepository.GetActiveServices();

                RenderServices(services);
            }
            catch (Exception ex)
            {
                _resultLabel.Text = "Unable to load services.";

                MessageBox.Show(
                    "Could not load services." +
                    Environment.NewLine +
                    Environment.NewLine +
                    ex.Message,
                    "SkillHub",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            string searchText = _searchBox.Text.Trim();

            try
            {
                List<ServiceCatalogItem> services;

                if (string.IsNullOrWhiteSpace(searchText))
                {
                    services =
                        _serviceRepository.GetActiveServices();
                }
                else
                {
                    services =
                        _serviceRepository.SearchActiveServices(searchText);
                }

                RenderServices(services);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not search services." +
                    Environment.NewLine +
                    Environment.NewLine +
                    ex.Message,
                    "SkillHub",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void RenderServices(
            List<ServiceCatalogItem> services)
        {
            _servicesPanel.SuspendLayout();

            _servicesPanel.Controls.Clear();

            _resultLabel.Text =
                services.Count + " service(s) found";

            foreach (ServiceCatalogItem service in services)
            {
                _servicesPanel.Controls.Add(
                    CreateServiceCard(service));
            }

            _servicesPanel.ResumeLayout();
        }

        private RoundedPanel CreateServiceCard(
            ServiceCatalogItem service)
        {
            RoundedPanel card = new RoundedPanel
            {
                Width = 330,
                Height = 245,
                Margin = new Padding(0, 0, 18, 18),
                BackColor = MarketplaceTheme.White,
                BorderThickness = 0,
                CornerRadius = 15,
                Padding = new Padding(20)
            };

            Label title = new Label
            {
                Text = service.Title,
                Font = MarketplaceTheme.SubHeading(13F),
                ForeColor = MarketplaceTheme.DeepCharcoal,
                AutoEllipsis = true,
                AutoSize = false,
                Location = new Point(20, 18),
                Size = new Size(285, 40)
            };

            Label freelancer = new Label
            {
                Text = "Freelancer: " + service.FreelancerName,
                Font = MarketplaceTheme.Body(9.5F),
                ForeColor = MarketplaceTheme.MutedText,
                AutoEllipsis = true,
                AutoSize = false,
                Location = new Point(20, 63),
                Size = new Size(285, 25)
            };

            Label price = new Label
            {
                Text = "৳ " + service.Price.ToString("N2"),
                Font = MarketplaceTheme.Heading(17F),
                ForeColor = MarketplaceTheme.Primary,
                AutoSize = true,
                Location = new Point(20, 94)
            };

            Label delivery = new Label
            {
                Text = service.DeliveryDays + " day(s) delivery",
                Font = MarketplaceTheme.Body(9F),
                ForeColor = MarketplaceTheme.MutedText,
                AutoSize = true,
                Location = new Point(20, 128)
            };

            Label slots = new Label
            {
                Text = service.AvailableSlots + " slot(s) available",
                Font = MarketplaceTheme.Body(9F),
                ForeColor =
                    service.AvailableSlots > 0
                        ? MarketplaceTheme.MutedText
                        : MarketplaceTheme.Danger,
                AutoSize = true,
                Location = new Point(20, 151)
            };

            ModernButton button = new ModernButton
            {
                Width = 285,
                Height = 42,
                Location = new Point(20, 184)
            };

            if (!service.IsActive ||
                service.AvailableSlots <= 0)
            {
                button.SetUnavailable("Unavailable");
            }
            else
            {
                button.Text = "Add to Cart";

                int serviceId = service.ServiceId;

                button.Click += delegate
                {
                    AddServiceToCart(serviceId);
                };
            }

            card.Controls.Add(title);
            card.Controls.Add(freelancer);
            card.Controls.Add(price);
            card.Controls.Add(delivery);
            card.Controls.Add(slots);
            card.Controls.Add(button);

            return card;
        }

        private void AddServiceToCart(int serviceId)
        {
            try
            {
                _cartRepository.AddItem(serviceId, 1);

                DialogResult result = MessageBox.Show(
                    "The service was added to your cart." +
                    Environment.NewLine +
                    Environment.NewLine +
                    "Would you like to open the cart now?",
                    "Added to Cart",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    OpenCart();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Cart Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not add the service to your cart." +
                    Environment.NewLine +
                    Environment.NewLine +
                    ex.Message,
                    "SkillHub",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void CartButton_Click(
            object sender,
            EventArgs e)
        {
            OpenCart();
        }

        private void OpenCart()
        {
            using (FrmCart form = new FrmCart())
            {
                form.ShowDialog(this);
            }
        }
    }
}