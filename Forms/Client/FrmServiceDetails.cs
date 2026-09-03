using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using SkillHub.Models;
using SkillHub.Repositories;
using SkillHub.UI;

namespace SkillHub.Forms.Client
{
    public sealed class FrmServiceDetails : Form
    {
        private readonly int _serviceId;
        private readonly ServiceRepository _serviceRepository;
        private readonly CartRepository _cartRepository;

        private PictureBox _serviceImage;
        private PictureBox _freelancerAvatar;
        private Label _freelancerName;
        private Label _professionalTitle;
        private Label _freelancerRating;
        private Label _freelancerBiography;
        private Label _skills;
        private Label _category;
        private Label _serviceTitle;
        private TextBox _description;
        private Label _price;
        private Label _delivery;
        private Label _slots;
        private Label _availability;
        private ModernButton _addToCartButton;

        public FrmServiceDetails(int serviceId)
        {
            if (serviceId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(serviceId));
            }

            _serviceId = serviceId;
            _serviceRepository = new ServiceRepository();
            _cartRepository = new CartRepository();

            InitializeForm();
            BuildInterface();
            LoadService();
        }

        private void InitializeForm()
        {
            Text = "SkillHub | Service Details";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(1200, 820);
            MinimumSize = new Size(1180, 720);
            BackColor = MarketplaceTheme.PageBackground;
            Font = MarketplaceTheme.Body();
        }

        private void BuildInterface()
        {
            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 76,
                BackColor = MarketplaceTheme.Navy
            };

            Label brand = new Label
            {
                Text = "SkillHub  /  Service Details",
                AutoSize = true,
                Font = MarketplaceTheme.Heading(19F),
                ForeColor = Color.White,
                Location = new Point(28, 21)
            };

            ModernButton closeButton = new ModernButton
            {
                Text = "Back to Services",
                Width = 155,
                Height = 40,
                IsSecondary = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            closeButton.Location = new Point(header.ClientSize.Width - 184, 18);
            closeButton.Click += delegate { Close(); };
            header.Resize += delegate
            {
                closeButton.Left = header.ClientSize.Width - closeButton.Width - 28;
            };

            header.Controls.Add(brand);
            header.Controls.Add(closeButton);

            Panel page = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = MarketplaceTheme.PageBackground
            };

            RoundedPanel freelancerCard = new RoundedPanel
            {
                Location = new Point(28, 24),
                Size = new Size(480, 660),
                BackColor = Color.White,
                BorderThickness = 0,
                CornerRadius = 18
            };

            _serviceImage = new PictureBox
            {
                Location = new Point(0, 0),
                Size = new Size(480, 285),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(229, 236, 247)
            };

            _freelancerAvatar = new PictureBox
            {
                Location = new Point(26, 307),
                Size = new Size(72, 72),
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            _freelancerName = new Label
            {
                AutoSize = false,
                Location = new Point(112, 309),
                Size = new Size(338, 28),
                Font = MarketplaceTheme.Heading(14F),
                ForeColor = MarketplaceTheme.DeepCharcoal,
                AutoEllipsis = true
            };

            _professionalTitle = new Label
            {
                AutoSize = false,
                Location = new Point(114, 340),
                Size = new Size(335, 22),
                Font = MarketplaceTheme.Body(9.5F),
                ForeColor = MarketplaceTheme.MutedText,
                AutoEllipsis = true
            };

            _freelancerRating = new Label
            {
                AutoSize = true,
                Location = new Point(114, 365),
                Font = MarketplaceTheme.SubHeading(9F),
                ForeColor = MarketplaceTheme.Warning
            };

            Label aboutHeading = new Label
            {
                Text = "About the freelancer",
                AutoSize = true,
                Location = new Point(28, 406),
                Font = MarketplaceTheme.SubHeading(11F),
                ForeColor = MarketplaceTheme.DeepCharcoal
            };

            _freelancerBiography = new Label
            {
                AutoSize = false,
                Location = new Point(28, 438),
                Size = new Size(422, 92),
                Font = MarketplaceTheme.Body(9.5F),
                ForeColor = MarketplaceTheme.MutedText
            };

            Label skillsHeading = new Label
            {
                Text = "Skills",
                AutoSize = true,
                Location = new Point(28, 548),
                Font = MarketplaceTheme.SubHeading(11F),
                ForeColor = MarketplaceTheme.DeepCharcoal
            };

            _skills = new Label
            {
                AutoSize = false,
                Location = new Point(28, 578),
                Size = new Size(422, 55),
                Font = MarketplaceTheme.Body(9.5F),
                ForeColor = MarketplaceTheme.Primary,
                BackColor = Color.FromArgb(235, 243, 255),
                Padding = new Padding(10, 8, 10, 8)
            };

            freelancerCard.Controls.Add(_serviceImage);
            freelancerCard.Controls.Add(_freelancerAvatar);
            freelancerCard.Controls.Add(_freelancerName);
            freelancerCard.Controls.Add(_professionalTitle);
            freelancerCard.Controls.Add(_freelancerRating);
            freelancerCard.Controls.Add(aboutHeading);
            freelancerCard.Controls.Add(_freelancerBiography);
            freelancerCard.Controls.Add(skillsHeading);
            freelancerCard.Controls.Add(_skills);

            RoundedPanel detailsCard = new RoundedPanel
            {
                Location = new Point(532, 24),
                Size = new Size(620, 660),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                BackColor = Color.White,
                BorderThickness = 0,
                CornerRadius = 18
            };

            _category = new Label
            {
                AutoSize = true,
                Location = new Point(32, 28),
                Font = MarketplaceTheme.SubHeading(9F),
                ForeColor = MarketplaceTheme.Primary,
                BackColor = Color.FromArgb(232, 241, 255),
                Padding = new Padding(10, 5, 10, 5)
            };

            _serviceTitle = new Label
            {
                AutoSize = false,
                Location = new Point(32, 73),
                Size = new Size(552, 78),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Font = MarketplaceTheme.Heading(22F),
                ForeColor = MarketplaceTheme.DeepCharcoal
            };

            _availability = new Label
            {
                AutoSize = true,
                Location = new Point(35, 158),
                Font = MarketplaceTheme.SubHeading(9.5F)
            };

            Label descriptionHeading = new Label
            {
                Text = "What this service includes",
                AutoSize = true,
                Location = new Point(32, 197),
                Font = MarketplaceTheme.SubHeading(12F),
                ForeColor = MarketplaceTheme.DeepCharcoal
            };

            _description = new TextBox
            {
                Location = new Point(32, 229),
                Size = new Size(552, 190),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                BorderStyle = BorderStyle.None,
                BackColor = Color.White,
                ForeColor = MarketplaceTheme.MutedText,
                Font = MarketplaceTheme.Body(10.5F)
            };

            RoundedPanel deliveryCard = CreateInformationCard("Delivery", 32, 442);
            _delivery = CreateInformationValue(deliveryCard);

            RoundedPanel slotsCard = CreateInformationCard("Availability", 216, 442);
            _slots = CreateInformationValue(slotsCard);

            RoundedPanel protectedCard = CreateInformationCard("Checkout", 400, 442);
            Label checkoutValue = CreateInformationValue(protectedCard);
            checkoutValue.Text = "Simulated & recorded";

            Label startingAt = new Label
            {
                Text = "Service price",
                AutoSize = true,
                Location = new Point(34, 551),
                Font = MarketplaceTheme.Body(9F),
                ForeColor = MarketplaceTheme.MutedText
            };

            _price = new Label
            {
                AutoSize = true,
                Location = new Point(32, 575),
                Font = MarketplaceTheme.Heading(22F),
                ForeColor = MarketplaceTheme.Primary
            };

            _addToCartButton = new ModernButton
            {
                Text = "Add to Cart",
                Width = 210,
                Height = 48,
                Location = new Point(374, 574),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            _addToCartButton.Click += delegate { AddToCart(); };

            detailsCard.Resize += delegate
            {
                _serviceTitle.Width = detailsCard.ClientSize.Width - 64;
                _description.Width = detailsCard.ClientSize.Width - 64;
                _addToCartButton.Left = detailsCard.ClientSize.Width - _addToCartButton.Width - 36;
            };

            detailsCard.Controls.Add(_category);
            detailsCard.Controls.Add(_serviceTitle);
            detailsCard.Controls.Add(_availability);
            detailsCard.Controls.Add(descriptionHeading);
            detailsCard.Controls.Add(_description);
            detailsCard.Controls.Add(deliveryCard);
            detailsCard.Controls.Add(slotsCard);
            detailsCard.Controls.Add(protectedCard);
            detailsCard.Controls.Add(startingAt);
            detailsCard.Controls.Add(_price);
            detailsCard.Controls.Add(_addToCartButton);

            page.Resize += delegate
            {
                detailsCard.Width = Math.Max(510, page.ClientSize.Width - detailsCard.Left - 30);
            };

            page.Controls.Add(freelancerCard);
            page.Controls.Add(detailsCard);
            Controls.Add(page);
            Controls.Add(header);
        }

        private static RoundedPanel CreateInformationCard(string heading, int left, int top)
        {
            RoundedPanel card = new RoundedPanel
            {
                Location = new Point(left, top),
                Size = new Size(166, 78),
                BackColor = MarketplaceTheme.SoftGray,
                BorderColor = MarketplaceTheme.Border,
                BorderThickness = 1,
                CornerRadius = 12
            };

            card.Controls.Add(new Label
            {
                Text = heading,
                AutoSize = true,
                Location = new Point(13, 12),
                Font = MarketplaceTheme.Body(8.5F),
                ForeColor = MarketplaceTheme.MutedText
            });

            return card;
        }

        private static Label CreateInformationValue(Control parent)
        {
            Label value = new Label
            {
                AutoSize = false,
                Location = new Point(13, 38),
                Size = new Size(142, 25),
                Font = MarketplaceTheme.SubHeading(9.5F),
                ForeColor = MarketplaceTheme.DeepCharcoal,
                AutoEllipsis = true
            };
            parent.Controls.Add(value);
            return value;
        }

        private void LoadService()
        {
            try
            {
                ServiceCatalogItem service = _serviceRepository.GetServiceById(_serviceId);

                if (service == null)
                {
                    MessageBox.Show(this, "This service is no longer available.", "SkillHub", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                    return;
                }

                _serviceImage.Image = ImageAssetHelper.LoadServiceImage(
                    service.ServiceImagePath,
                    service.Title,
                    service.CategoryName,
                    _serviceImage.Size);
                _freelancerAvatar.Image = ImageAssetHelper.LoadAvatar(
                    service.FreelancerProfileImagePath,
                    service.FreelancerName,
                    72);
                _freelancerName.Text = service.FreelancerName + (service.IsVerified ? "  ✓ Verified" : string.Empty);
                _freelancerName.ForeColor = service.IsVerified ? MarketplaceTheme.Primary : MarketplaceTheme.DeepCharcoal;
                _professionalTitle.Text = service.ProfessionalTitle;
                _freelancerRating.Text = "★ " + service.AverageRating.ToString("0.0") + " average freelancer rating";
                _freelancerBiography.Text = string.IsNullOrWhiteSpace(service.FreelancerBiography)
                    ? "This freelancer has not added a biography yet."
                    : service.FreelancerBiography;
                _skills.Text = string.IsNullOrWhiteSpace(service.FreelancerSkills)
                    ? "Skills not listed"
                    : service.FreelancerSkills;
                _category.Text = service.CategoryName;
                _serviceTitle.Text = service.Title;
                _description.Text = service.Description;
                _price.Text = "৳ " + service.Price.ToString("N2");
                _delivery.Text = service.DeliveryDays + " day(s)";
                _slots.Text = service.AvailableSlots + " slot(s)";
                _availability.Text = service.IsAvailable ? "● Available to order" : "● Currently unavailable";
                _availability.ForeColor = service.IsAvailable ? MarketplaceTheme.Success : MarketplaceTheme.Danger;

                if (!service.IsAvailable)
                {
                    _addToCartButton.SetUnavailable("Unavailable");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    this,
                    "Could not load the service details.\r\n\r\n" + exception.Message,
                    "SkillHub",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Close();
            }
        }

        private void AddToCart()
        {
            try
            {
                _cartRepository.AddItem(_serviceId, 1);
                DialogResult result = MessageBox.Show(
                    this,
                    "The service was added to your cart.\r\n\r\nOpen the cart now?",
                    "Added to Cart",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    using (FrmCart cart = new FrmCart())
                    {
                        cart.ShowDialog(this);
                    }
                }
            }
            catch (SqlException exception)
            {
                MessageBox.Show(this, exception.Message, "Cart Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, exception.Message, "SkillHub", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_serviceImage != null && _serviceImage.Image != null)
                {
                    _serviceImage.Image.Dispose();
                }

                if (_freelancerAvatar != null && _freelancerAvatar.Image != null)
                {
                    _freelancerAvatar.Image.Dispose();
                }
            }

            base.Dispose(disposing);
        }
    }
}
