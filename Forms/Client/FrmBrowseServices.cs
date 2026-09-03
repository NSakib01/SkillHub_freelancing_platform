using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
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
        private readonly List<ServiceCatalogItem> _allServices;

        private TextBox _searchBox;
        private ComboBox _categoryBox;
        private ComboBox _sortBox;
        private CheckBox _availableOnlyBox;
        private FlowLayoutPanel _servicesPanel;
        private Label _resultLabel;

        public FrmBrowseServices()
        {
            _serviceRepository = new ServiceRepository();
            _cartRepository = new CartRepository();
            _allServices = new List<ServiceCatalogItem>();

            InitializeForm();
            BuildInterface();
            LoadServices();
        }

        private void InitializeForm()
        {
            Text = "SkillHub | Explore Services";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(1320, 860);
            MinimumSize = new Size(1200, 700);
            BackColor = MarketplaceTheme.PageBackground;
            Font = MarketplaceTheme.Body();
        }

        private void BuildInterface()
        {
            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 78,
                BackColor = MarketplaceTheme.Navy,
                Padding = new Padding(30, 16, 30, 12)
            };

            Label brand = new Label
            {
                Text = "SkillHub",
                Font = MarketplaceTheme.Heading(22F),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(30, 20)
            };

            ModernButton closeButton = new ModernButton
            {
                Text = "Back",
                Width = 96,
                Height = 40,
                IsSecondary = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            closeButton.Location = new Point(header.ClientSize.Width - 250, 18);
            closeButton.Click += delegate { Close(); };

            ModernButton cartButton = new ModernButton
            {
                Text = "View Cart",
                Width = 130,
                Height = 40,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            cartButton.Location = new Point(header.ClientSize.Width - 144, 18);
            cartButton.Click += delegate { OpenCart(); };

            header.Resize += delegate
            {
                cartButton.Left = header.ClientSize.Width - cartButton.Width - 28;
                closeButton.Left = cartButton.Left - closeButton.Width - 12;
            };

            header.Controls.Add(brand);
            header.Controls.Add(closeButton);
            header.Controls.Add(cartButton);

            Panel filterHost = new Panel
            {
                Dock = DockStyle.Top,
                Height = 205,
                BackColor = MarketplaceTheme.PageBackground,
                Padding = new Padding(28, 18, 28, 14)
            };

            RoundedPanel filterCard = new RoundedPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                BorderThickness = 0,
                CornerRadius = 18
            };

            Label heroTitle = new Label
            {
                Text = "Find the right expert for your project",
                Font = MarketplaceTheme.Heading(25F),
                ForeColor = MarketplaceTheme.DeepCharcoal,
                AutoSize = true,
                Location = new Point(30, 22)
            };

            Label heroSubtitle = new Label
            {
                Text = "Search services, compare freelancers and open any card for full details.",
                Font = MarketplaceTheme.Body(10.5F),
                ForeColor = MarketplaceTheme.MutedText,
                AutoSize = true,
                Location = new Point(32, 62)
            };

            Label searchLabel = CreateFilterLabel("Search", 32, 101);
            _searchBox = new TextBox
            {
                Font = MarketplaceTheme.Body(10.5F),
                Location = new Point(32, 124),
                Size = new Size(415, 31),
                BorderStyle = BorderStyle.FixedSingle
            };
            _searchBox.TextChanged += delegate { ApplyFilters(); };

            Label categoryLabel = CreateFilterLabel("Category", 470, 101);
            _categoryBox = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = MarketplaceTheme.Body(10F),
                Location = new Point(470, 124),
                Size = new Size(245, 31)
            };
            _categoryBox.SelectedIndexChanged += delegate { ApplyFilters(); };

            Label sortLabel = CreateFilterLabel("Sort by", 738, 101);
            _sortBox = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = MarketplaceTheme.Body(10F),
                Location = new Point(738, 124),
                Size = new Size(220, 31)
            };
            _sortBox.Items.AddRange(new object[]
            {
                "Recommended",
                "Rating: High to Low",
                "Price: Low to High",
                "Price: High to Low",
                "Delivery: Fastest",
                "Newest"
            });
            _sortBox.SelectedIndex = 0;
            _sortBox.SelectedIndexChanged += delegate { ApplyFilters(); };

            _availableOnlyBox = new CheckBox
            {
                Text = "Available now",
                Checked = true,
                AutoSize = true,
                Font = MarketplaceTheme.SubHeading(9.5F),
                ForeColor = MarketplaceTheme.DeepCharcoal,
                Location = new Point(988, 128)
            };
            _availableOnlyBox.CheckedChanged += delegate { ApplyFilters(); };

            filterCard.Controls.Add(heroTitle);
            filterCard.Controls.Add(heroSubtitle);
            filterCard.Controls.Add(searchLabel);
            filterCard.Controls.Add(_searchBox);
            filterCard.Controls.Add(categoryLabel);
            filterCard.Controls.Add(_categoryBox);
            filterCard.Controls.Add(sortLabel);
            filterCard.Controls.Add(_sortBox);
            filterCard.Controls.Add(_availableOnlyBox);
            filterHost.Controls.Add(filterCard);

            Panel contentHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 56,
                BackColor = MarketplaceTheme.PageBackground,
                Padding = new Padding(30, 10, 30, 8)
            };

            Label servicesTitle = new Label
            {
                Text = "Explore services",
                Font = MarketplaceTheme.SubHeading(16F),
                ForeColor = MarketplaceTheme.DeepCharcoal,
                AutoSize = true,
                Location = new Point(30, 13)
            };

            _resultLabel = new Label
            {
                Text = "Loading…",
                Font = MarketplaceTheme.Body(9.5F),
                ForeColor = MarketplaceTheme.MutedText,
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            contentHeader.Resize += delegate
            {
                _resultLabel.Location = new Point(
                    contentHeader.ClientSize.Width - _resultLabel.Width - 30,
                    17);
            };

            contentHeader.Controls.Add(servicesTitle);
            contentHeader.Controls.Add(_resultLabel);

            _servicesPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(30, 10, 24, 30),
                BackColor = MarketplaceTheme.PageBackground
            };

            Controls.Add(_servicesPanel);
            Controls.Add(contentHeader);
            Controls.Add(filterHost);
            Controls.Add(header);
        }

        private static Label CreateFilterLabel(string text, int left, int top)
        {
            return new Label
            {
                Text = text,
                AutoSize = true,
                Font = MarketplaceTheme.SubHeading(9F),
                ForeColor = MarketplaceTheme.MutedText,
                Location = new Point(left, top)
            };
        }

        private void LoadServices()
        {
            try
            {
                _allServices.Clear();
                _allServices.AddRange(_serviceRepository.GetActiveServices());
                LoadCategoryFilter();
                ApplyFilters();
            }
            catch (Exception exception)
            {
                _resultLabel.Text = "Unable to load services";
                MessageBox.Show(
                    this,
                    "Could not load the service marketplace.\r\n\r\n" + exception.Message,
                    "SkillHub",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LoadCategoryFilter()
        {
            string previousSelection = Convert.ToString(_categoryBox.SelectedItem);
            _categoryBox.Items.Clear();
            _categoryBox.Items.Add("All categories");

            foreach (string category in _allServices
                .Select(service => service.CategoryName)
                .Where(categoryName => !string.IsNullOrWhiteSpace(categoryName))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(categoryName => categoryName))
            {
                _categoryBox.Items.Add(category);
            }

            int previousIndex = _categoryBox.Items.IndexOf(previousSelection);
            _categoryBox.SelectedIndex = previousIndex >= 0 ? previousIndex : 0;
        }

        private void ApplyFilters()
        {
            if (_servicesPanel == null || _allServices == null)
            {
                return;
            }

            string search = (_searchBox.Text ?? string.Empty).Trim();
            string category = Convert.ToString(_categoryBox.SelectedItem);
            IEnumerable<ServiceCatalogItem> query = _allServices;

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(service =>
                    Contains(service.Title, search)
                    || Contains(service.Description, search)
                    || Contains(service.FreelancerName, search)
                    || Contains(service.ProfessionalTitle, search)
                    || Contains(service.FreelancerSkills, search)
                    || Contains(service.CategoryName, search));
            }

            if (!string.IsNullOrWhiteSpace(category)
                && !string.Equals(category, "All categories", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(service => string.Equals(
                    service.CategoryName,
                    category,
                    StringComparison.OrdinalIgnoreCase));
            }

            if (_availableOnlyBox.Checked)
            {
                query = query.Where(service => service.IsAvailable);
            }

            switch (Convert.ToString(_sortBox.SelectedItem))
            {
                case "Rating: High to Low":
                    query = query.OrderByDescending(service => service.AverageRating)
                        .ThenBy(service => service.Price);
                    break;
                case "Price: Low to High":
                    query = query.OrderBy(service => service.Price)
                        .ThenByDescending(service => service.AverageRating);
                    break;
                case "Price: High to Low":
                    query = query.OrderByDescending(service => service.Price)
                        .ThenByDescending(service => service.AverageRating);
                    break;
                case "Delivery: Fastest":
                    query = query.OrderBy(service => service.DeliveryDays)
                        .ThenByDescending(service => service.AverageRating);
                    break;
                case "Newest":
                    query = query.OrderByDescending(service => service.CreatedAt);
                    break;
                default:
                    query = query.OrderByDescending(service => service.IsVerified)
                        .ThenByDescending(service => service.AverageRating)
                        .ThenByDescending(service => service.CreatedAt);
                    break;
            }

            RenderServices(query.ToList());
        }

        private static bool Contains(string source, string search)
        {
            return !string.IsNullOrWhiteSpace(source)
                && source.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void RenderServices(List<ServiceCatalogItem> services)
        {
            _servicesPanel.SuspendLayout();
            DisposePictureBoxImages(_servicesPanel);
            _servicesPanel.Controls.Clear();
            _resultLabel.Text = services.Count + " service(s) found";

            if (services.Count == 0)
            {
                RoundedPanel emptyCard = new RoundedPanel
                {
                    Width = 720,
                    Height = 150,
                    Margin = new Padding(0, 10, 0, 0),
                    BackColor = Color.White,
                    CornerRadius = 16,
                    BorderThickness = 0
                };
                emptyCard.Controls.Add(new Label
                {
                    Text = "No services match these filters",
                    Font = MarketplaceTheme.Heading(17F),
                    ForeColor = MarketplaceTheme.DeepCharcoal,
                    AutoSize = true,
                    Location = new Point(28, 30)
                });
                emptyCard.Controls.Add(new Label
                {
                    Text = "Try a different keyword, category or sorting option.",
                    Font = MarketplaceTheme.Body(10F),
                    ForeColor = MarketplaceTheme.MutedText,
                    AutoSize = true,
                    Location = new Point(30, 72)
                });
                _servicesPanel.Controls.Add(emptyCard);
            }
            else
            {
                foreach (ServiceCatalogItem service in services)
                {
                    _servicesPanel.Controls.Add(CreateServiceCard(service));
                }
            }

            _servicesPanel.ResumeLayout();
        }

        private RoundedPanel CreateServiceCard(ServiceCatalogItem service)
        {
            RoundedPanel card = new RoundedPanel
            {
                Width = 350,
                Height = 465,
                Margin = new Padding(0, 0, 20, 20),
                BackColor = Color.White,
                BorderColor = MarketplaceTheme.Border,
                BorderThickness = 1,
                CornerRadius = 16
            };

            PictureBox serviceImage = new PictureBox
            {
                Location = new Point(0, 0),
                Size = new Size(350, 170),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(232, 238, 247),
                Image = ImageAssetHelper.LoadServiceImage(
                    service.ServiceImagePath,
                    service.Title,
                    service.CategoryName,
                    new Size(350, 170)),
                Cursor = Cursors.Hand
            };

            PictureBox avatar = new PictureBox
            {
                Location = new Point(18, 184),
                Size = new Size(48, 48),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = ImageAssetHelper.LoadAvatar(
                    service.FreelancerProfileImagePath,
                    service.FreelancerName,
                    48)
            };

            Label freelancer = new Label
            {
                Text = service.FreelancerName + (service.IsVerified ? "  ✓" : string.Empty),
                Font = MarketplaceTheme.SubHeading(10F),
                ForeColor = service.IsVerified ? MarketplaceTheme.Primary : MarketplaceTheme.DeepCharcoal,
                AutoEllipsis = true,
                AutoSize = false,
                Location = new Point(76, 185),
                Size = new Size(250, 23)
            };

            Label professionalTitle = new Label
            {
                Text = service.ProfessionalTitle,
                Font = MarketplaceTheme.Body(8.5F),
                ForeColor = MarketplaceTheme.MutedText,
                AutoEllipsis = true,
                AutoSize = false,
                Location = new Point(76, 208),
                Size = new Size(250, 22)
            };

            Label category = new Label
            {
                Text = service.CategoryName,
                Font = MarketplaceTheme.SubHeading(8.5F),
                ForeColor = MarketplaceTheme.Primary,
                BackColor = Color.FromArgb(232, 241, 255),
                AutoEllipsis = true,
                TextAlign = ContentAlignment.MiddleLeft,
                Location = new Point(18, 244),
                Size = new Size(314, 25),
                Padding = new Padding(8, 0, 8, 0)
            };

            Label title = new Label
            {
                Text = service.Title,
                Font = MarketplaceTheme.SubHeading(12F),
                ForeColor = MarketplaceTheme.DeepCharcoal,
                AutoEllipsis = true,
                AutoSize = false,
                Location = new Point(18, 279),
                Size = new Size(314, 52),
                Cursor = Cursors.Hand
            };

            Label rating = new Label
            {
                Text = "★ " + service.AverageRating.ToString("0.0") + "   •   "
                    + service.DeliveryDays + " day delivery",
                Font = MarketplaceTheme.Body(9F),
                ForeColor = MarketplaceTheme.Warning,
                AutoSize = true,
                Location = new Point(18, 337)
            };

            Label price = new Label
            {
                Text = "৳ " + service.Price.ToString("N0"),
                Font = MarketplaceTheme.Heading(16F),
                ForeColor = MarketplaceTheme.Primary,
                AutoSize = true,
                Location = new Point(18, 365)
            };

            Label slots = new Label
            {
                Text = service.AvailableSlots + " slot(s)",
                Font = MarketplaceTheme.Body(8.5F),
                ForeColor = service.IsAvailable ? MarketplaceTheme.Success : MarketplaceTheme.Danger,
                AutoSize = true,
                Location = new Point(244, 372)
            };

            ModernButton detailsButton = new ModernButton
            {
                Text = "View Details",
                Width = 150,
                Height = 42,
                Location = new Point(18, 411),
                IsSecondary = true
            };

            ModernButton cartButton = new ModernButton
            {
                Width = 154,
                Height = 42,
                Location = new Point(178, 411)
            };

            int serviceId = service.ServiceId;
            EventHandler openDetails = delegate { OpenServiceDetails(serviceId); };
            serviceImage.Click += openDetails;
            title.Click += openDetails;
            detailsButton.Click += openDetails;

            if (service.IsAvailable)
            {
                cartButton.Text = "Add to Cart";
                cartButton.Click += delegate { AddServiceToCart(serviceId); };
            }
            else
            {
                cartButton.SetUnavailable("Unavailable");
            }

            card.Controls.Add(serviceImage);
            card.Controls.Add(avatar);
            card.Controls.Add(freelancer);
            card.Controls.Add(professionalTitle);
            card.Controls.Add(category);
            card.Controls.Add(title);
            card.Controls.Add(rating);
            card.Controls.Add(price);
            card.Controls.Add(slots);
            card.Controls.Add(detailsButton);
            card.Controls.Add(cartButton);
            return card;
        }

        private void OpenServiceDetails(int serviceId)
        {
            using (FrmServiceDetails details = new FrmServiceDetails(serviceId))
            {
                details.ShowDialog(this);
            }
        }

        private void AddServiceToCart(int serviceId)
        {
            try
            {
                _cartRepository.AddItem(serviceId, 1);
                DialogResult result = MessageBox.Show(
                    this,
                    "The service was added to your cart.\r\n\r\nOpen the cart now?",
                    "Added to Cart",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    OpenCart();
                }
            }
            catch (SqlException exception)
            {
                MessageBox.Show(this, exception.Message, "Cart Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    this,
                    "Could not add the service to your cart.\r\n\r\n" + exception.Message,
                    "SkillHub",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void OpenCart()
        {
            using (FrmCart cart = new FrmCart())
            {
                cart.ShowDialog(this);
            }
        }

        private static void DisposePictureBoxImages(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                DisposePictureBoxImages(control);
                PictureBox picture = control as PictureBox;
                if (picture != null && picture.Image != null)
                {
                    picture.Image.Dispose();
                    picture.Image = null;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _servicesPanel != null)
            {
                DisposePictureBoxImages(_servicesPanel);
            }

            base.Dispose(disposing);
        }
    }
}
