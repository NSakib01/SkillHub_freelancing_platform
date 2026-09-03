using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SkillHub.UI
{
    /// <summary>
    /// Flat, rounded, premium-styled button built entirely in code
    /// (FlatStyle.Flat, zero border size, GDI+ rounded corners).
    /// </summary>
    public class ModernButton : Button
    {
        private int _cornerRadius = 10;
        private Color _normalBackColor = MarketplaceTheme.Primary;
        private Color _hoverBackColor = MarketplaceTheme.PrimaryHover;

        public ModernButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);

            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            Font = MarketplaceTheme.ButtonFont();
            ForeColor = MarketplaceTheme.White;
            BackColor = _normalBackColor;
            Cursor = Cursors.Hand;
            Height = 42;
            Width = 160;
            Padding = new Padding(20, 0, 20, 0);
            UseVisualStyleBackColor = false;

            MouseEnter += ModernButtonMouseEnter;
            MouseLeave += ModernButtonMouseLeave;
        }

        public int CornerRadius
        {
            get { return _cornerRadius; }
            set
            {
                _cornerRadius = value;
                UpdateRegion();
                Invalidate();
            }
        }

        public bool IsSecondary
        {
            set
            {
                if (value)
                {
                    _normalBackColor = MarketplaceTheme.White;
                    _hoverBackColor = MarketplaceTheme.SoftGray;
                    ForeColor = MarketplaceTheme.Primary;
                    FlatAppearance.BorderSize = 1;
                    FlatAppearance.BorderColor = MarketplaceTheme.Primary;
                }
                else
                {
                    _normalBackColor = MarketplaceTheme.Primary;
                    _hoverBackColor = MarketplaceTheme.PrimaryHover;
                    ForeColor = MarketplaceTheme.White;
                    FlatAppearance.BorderSize = 0;
                }

                BackColor = _normalBackColor;
            }
        }

        public void SetUnavailable(string label)
        {
            Enabled = false;
            Text = label;
            BackColor = MarketplaceTheme.Disabled;
            ForeColor = MarketplaceTheme.MutedText;
            Cursor = Cursors.No;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateRegion();
        }

        private void ModernButtonMouseEnter(object sender, EventArgs e)
        {
            if (Enabled)
            {
                BackColor = _hoverBackColor;
            }
        }

        private void ModernButtonMouseLeave(object sender, EventArgs e)
        {
            if (Enabled)
            {
                BackColor = _normalBackColor;
            }
        }

        private void UpdateRegion()
        {
            if (Width <= 0 || Height <= 0)
            {
                return;
            }

            using (GraphicsPath path = RoundedPanel.CreateRoundedPath(
                new Rectangle(0, 0, Width, Height),
                _cornerRadius))
            {
                Region = new Region(path);
            }
        }
    }
}
