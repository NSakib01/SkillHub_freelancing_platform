using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SkillHub.UI
{
    /// <summary>
    /// A programmatically drawn panel with rounded corners and an optional
    /// border, used to build the premium marketplace card look without any
    /// Visual Studio Designer support.
    /// </summary>
    public class RoundedPanel : Panel
    {
        private int _cornerRadius = 14;
        private Color _borderColor = MarketplaceTheme.Border;
        private int _borderThickness = 1;

        public RoundedPanel()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint
                | ControlStyles.UserPaint
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw,
                true);

            BackColor = MarketplaceTheme.CardBackground;
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

        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                Invalidate();
            }
        }

        public int BorderThickness
        {
            get { return _borderThickness; }
            set
            {
                _borderThickness = value;
                Invalidate();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateRegion();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath path = CreateRoundedPath(
                new Rectangle(0, 0, Width - 1, Height - 1),
                _cornerRadius))
            using (SolidBrush background = new SolidBrush(BackColor))
            using (Pen border = new Pen(_borderColor, _borderThickness))
            {
                e.Graphics.FillPath(background, path);

                if (_borderThickness > 0)
                {
                    e.Graphics.DrawPath(border, path);
                }
            }
        }

        private void UpdateRegion()
        {
            if (Width <= 0 || Height <= 0)
            {
                return;
            }

            using (GraphicsPath path = CreateRoundedPath(
                new Rectangle(0, 0, Width, Height),
                _cornerRadius))
            {
                Region = new Region(path);
            }
        }

        internal static GraphicsPath CreateRoundedPath(Rectangle bounds, int radius)
        {
            GraphicsPath path = new GraphicsPath();

            if (radius <= 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            int diameter = radius * 2;
            Rectangle arc = new Rectangle(bounds.Location, new Size(diameter, diameter));

            path.AddArc(arc, 180, 90);

            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }
    }
}