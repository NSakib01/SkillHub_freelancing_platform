using System;
using System.Drawing;
using System.Windows.Forms;
using SkillHub.Models;
using SkillHub.Repositories;
using SkillHub.UI;

namespace SkillHub.Forms.Client
{
    public sealed class FrmReview : Form
    {
        private readonly OrderModel _order;
        private readonly ReviewRepository _reviewRepository;

        private NumericUpDown _ratingInput;
        private TextBox _commentInput;

        public FrmReview(OrderModel order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            _order = order;
            _reviewRepository = new ReviewRepository();

            InitializeForm();
            BuildInterface();
        }

        private void InitializeForm()
        {
            Text = "SkillHub - Review Service";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(600, 500);
            MinimumSize = new Size(520, 440);
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
                Text = "Review Your Service",
                Font = MarketplaceTheme.Heading(22F),
                ForeColor = MarketplaceTheme.DeepCharcoal,
                AutoSize = true,
                Location = new Point(28, 22)
            };

            header.Controls.Add(title);

            RoundedPanel card = new RoundedPanel
            {
                Location = new Point(30, 110),
                Size = new Size(520, 285),
                BackColor = MarketplaceTheme.White,
                BorderThickness = 0,
                CornerRadius = 15
            };

            Label serviceLabel = new Label
            {
                Text = "Service: " + _order.ServiceTitle,
                Font = MarketplaceTheme.SubHeading(12F),
                ForeColor = MarketplaceTheme.DeepCharcoal,
                AutoEllipsis = true,
                AutoSize = false,
                Location = new Point(25, 22),
                Size = new Size(465, 28)
            };

            Label freelancerLabel = new Label
            {
                Text = "Freelancer: " + _order.FreelancerName,
                Font = MarketplaceTheme.Body(10F),
                ForeColor = MarketplaceTheme.MutedText,
                AutoEllipsis = true,
                AutoSize = false,
                Location = new Point(25, 53),
                Size = new Size(465, 25)
            };

            Label ratingLabel = new Label
            {
                Text = "Rating (1 - 5)",
                Font = MarketplaceTheme.SubHeading(10F),
                ForeColor = MarketplaceTheme.DeepCharcoal,
                AutoSize = true,
                Location = new Point(25, 92)
            };

            _ratingInput = new NumericUpDown
            {
                Minimum = 1,
                Maximum = 5,
                Value = 5,
                Width = 80,
                Height = 35,
                Font = MarketplaceTheme.SubHeading(11F),
                Location = new Point(25, 118)
            };

            Label commentLabel = new Label
            {
                Text = "Comment",
                Font = MarketplaceTheme.SubHeading(10F),
                ForeColor = MarketplaceTheme.DeepCharcoal,
                AutoSize = true,
                Location = new Point(125, 92)
            };

            _commentInput = new TextBox
            {
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Font = MarketplaceTheme.Body(10F),
                Location = new Point(125, 118),
                Size = new Size(365, 90),
                MaxLength = 1000
            };

            Label hint = new Label
            {
                Text = "Share your experience with this service.",
                Font = MarketplaceTheme.Body(9F),
                ForeColor = MarketplaceTheme.MutedText,
                AutoSize = true,
                Location = new Point(25, 225)
            };

            card.Controls.Add(serviceLabel);
            card.Controls.Add(freelancerLabel);
            card.Controls.Add(ratingLabel);
            card.Controls.Add(_ratingInput);
            card.Controls.Add(commentLabel);
            card.Controls.Add(_commentInput);
            card.Controls.Add(hint);

            ModernButton cancelButton = new ModernButton
            {
                Text = "Cancel",
                Width = 110,
                Height = 42,
                Location = new Point(300, 415)
            };

            cancelButton.IsSecondary = true;
            cancelButton.Click += delegate
            {
                DialogResult = DialogResult.Cancel;
                Close();
            };

            ModernButton submitButton = new ModernButton
            {
                Text = "Submit Review",
                Width = 145,
                Height = 42,
                Location = new Point(420, 415)
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
            string comment =
                _commentInput.Text == null
                    ? string.Empty
                    : _commentInput.Text.Trim();

            try
            {
                ReviewModel review = new ReviewModel
                {
                    OrderId = _order.OrderId,
                    ClientId = _order.ClientId,
                    FreelancerId = _order.FreelancerId,
                    Rating = Convert.ToByte(_ratingInput.Value),
                    Comment = comment
                };

                _reviewRepository.AddReview(review);

                MessageBox.Show(
                    "Your review has been submitted successfully.",
                    "SkillHub",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "The review could not be submitted." +
                    Environment.NewLine +
                    Environment.NewLine +
                    ex.Message,
                    "Review Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}