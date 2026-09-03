using System.Drawing;

namespace SkillHub.UI
{
    /// <summary>
    /// Premium marketplace palette and typography for the Client module only.
    /// Kept separate from Forms.Common.UiFactory so Admin and Freelancer
    /// screens are never affected by this styling.
    /// </summary>
    public static class MarketplaceTheme
    {
        public static readonly Color White = Color.FromArgb(255, 255, 255);
        public static readonly Color VibrantBlue = Color.FromArgb(29, 112, 236);
        public static readonly Color DeepCharcoal = Color.FromArgb(30, 34, 41);
        public static readonly Color SoftGray = Color.FromArgb(248, 250, 252);

        public static readonly Color PageBackground = SoftGray;
        public static readonly Color CardBackground = White;
        public static readonly Color Primary = VibrantBlue;
        public static readonly Color PrimaryHover = Color.FromArgb(18, 96, 210);
        public static readonly Color PrimaryText = DeepCharcoal;
        public static readonly Color MutedText = Color.FromArgb(100, 110, 125);
        public static readonly Color Border = Color.FromArgb(228, 233, 240);
        public static readonly Color Danger = Color.FromArgb(200, 60, 70);
        public static readonly Color Disabled = Color.FromArgb(210, 214, 220);

        public static Font Heading(float size = 22F)
        {
            return new Font("Segoe UI", size, FontStyle.Bold);
        }

        public static Font SubHeading(float size = 13F)
        {
            return new Font("Segoe UI", size, FontStyle.Bold);
        }

        public static Font Body(float size = 10F)
        {
            return new Font("Segoe UI", size, FontStyle.Regular);
        }

        public static Font ButtonFont(float size = 9.5F)
        {
            return new Font("Segoe UI", size, FontStyle.Bold);
        }
    }
}