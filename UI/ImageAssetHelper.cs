using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace SkillHub.UI
{
    /// <summary>
    /// Loads portable project images without locking their files and creates
    /// polished fallbacks when an optional image is missing.
    /// </summary>
    public static class ImageAssetHelper
    {
        public static Image LoadAvatar(string storedPath, string displayName, int size)
        {
            Image source = LoadFromStoredPath(storedPath);

            if (source == null)
            {
                source = CreateAvatarFallback(displayName, size);
            }

            try
            {
                return CreateCircularImage(source, size);
            }
            finally
            {
                source.Dispose();
            }
        }

        public static Image LoadServiceImage(
            string storedPath,
            string title,
            string category,
            Size size)
        {
            Image source = LoadFromStoredPath(storedPath);

            if (source != null)
            {
                try
                {
                    return CreateCoverImage(source, size);
                }
                finally
                {
                    source.Dispose();
                }
            }

            return CreateServiceFallback(title, category, size);
        }

        public static string ImportUserImage(string sourcePath, string filePrefix)
        {
            if (string.IsNullOrWhiteSpace(sourcePath) || !File.Exists(sourcePath))
            {
                throw new FileNotFoundException("The selected image could not be found.", sourcePath);
            }

            string extension = Path.GetExtension(sourcePath).ToLowerInvariant();

            if (extension != ".png" && extension != ".jpg" &&
                extension != ".jpeg" && extension != ".bmp")
            {
                throw new InvalidOperationException(
                    "Choose a PNG, JPG, JPEG or BMP image file.");
            }

            string relativeDirectory = Path.Combine("Assets", "UserUploads");
            string targetDirectory = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                relativeDirectory);

            Directory.CreateDirectory(targetDirectory);

            string safePrefix = MakeSafeFileName(filePrefix);
            string fileName = safePrefix + "-" + DateTime.Now.ToString("yyyyMMdd-HHmmss")
                + "-" + Guid.NewGuid().ToString("N").Substring(0, 8) + extension;

            string targetPath = Path.Combine(targetDirectory, fileName);
            File.Copy(sourcePath, targetPath, false);

            return Path.Combine(relativeDirectory, fileName);
        }

        public static string ResolveAssetPath(string storedPath)
        {
            if (string.IsNullOrWhiteSpace(storedPath))
            {
                return null;
            }

            string normalized = storedPath
                .Replace('/', Path.DirectorySeparatorChar)
                .Replace('\\', Path.DirectorySeparatorChar);

            if (Path.IsPathRooted(normalized))
            {
                return normalized;
            }

            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, normalized);
        }

        private static Image LoadFromStoredPath(string storedPath)
        {
            string path = ResolveAssetPath(storedPath);

            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                return null;
            }

            try
            {
                using (FileStream stream = new FileStream(
                    path,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.ReadWrite))
                using (Image original = Image.FromStream(stream))
                {
                    return new Bitmap(original);
                }
            }
            catch (ArgumentException)
            {
                return null;
            }
            catch (IOException)
            {
                return null;
            }
        }

        private static Image CreateCircularImage(Image source, int size)
        {
            Bitmap result = new Bitmap(size, size);

            using (Graphics graphics = Graphics.FromImage(result))
            using (GraphicsPath path = new GraphicsPath())
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.Clear(Color.Transparent);

                path.AddEllipse(1, 1, size - 2, size - 2);
                graphics.SetClip(path);

                Rectangle sourceRectangle = GetSquareCrop(source);
                graphics.DrawImage(
                    source,
                    new Rectangle(0, 0, size, size),
                    sourceRectangle,
                    GraphicsUnit.Pixel);
            }

            return result;
        }

        private static Rectangle GetSquareCrop(Image image)
        {
            int side = Math.Min(image.Width, image.Height);
            return new Rectangle(
                (image.Width - side) / 2,
                (image.Height - side) / 2,
                side,
                side);
        }

        private static Image CreateCoverImage(Image source, Size requestedSize)
        {
            int width = Math.Max(240, requestedSize.Width);
            int height = Math.Max(130, requestedSize.Height);
            Bitmap result = new Bitmap(width, height);

            float sourceRatio = (float)source.Width / source.Height;
            float targetRatio = (float)width / height;
            Rectangle sourceRectangle;

            if (sourceRatio > targetRatio)
            {
                int cropWidth = Convert.ToInt32(source.Height * targetRatio);
                sourceRectangle = new Rectangle(
                    (source.Width - cropWidth) / 2,
                    0,
                    cropWidth,
                    source.Height);
            }
            else
            {
                int cropHeight = Convert.ToInt32(source.Width / targetRatio);
                sourceRectangle = new Rectangle(
                    0,
                    (source.Height - cropHeight) / 2,
                    source.Width,
                    cropHeight);
            }

            using (Graphics graphics = Graphics.FromImage(result))
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.DrawImage(
                    source,
                    new Rectangle(0, 0, width, height),
                    sourceRectangle,
                    GraphicsUnit.Pixel);
            }

            return result;
        }

        private static Image CreateAvatarFallback(string displayName, int size)
        {
            Bitmap bitmap = new Bitmap(size, size);
            string initials = GetInitials(displayName);

            using (Graphics graphics = Graphics.FromImage(bitmap))
            using (LinearGradientBrush background = new LinearGradientBrush(
                new Rectangle(0, 0, size, size),
                MarketplaceTheme.Primary,
                MarketplaceTheme.Accent,
                45F))
            using (Font font = new Font("Segoe UI", Math.Max(13F, size * 0.27F), FontStyle.Bold))
            using (StringFormat format = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            })
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.FillRectangle(background, 0, 0, size, size);
                graphics.DrawString(initials, font, Brushes.White, new RectangleF(0, 0, size, size), format);
            }

            return bitmap;
        }

        private static Image CreateServiceFallback(
            string title,
            string category,
            Size size)
        {
            int width = Math.Max(240, size.Width);
            int height = Math.Max(130, size.Height);
            Bitmap bitmap = new Bitmap(width, height);

            using (Graphics graphics = Graphics.FromImage(bitmap))
            using (LinearGradientBrush background = new LinearGradientBrush(
                new Rectangle(0, 0, width, height),
                Color.FromArgb(16, 34, 70),
                MarketplaceTheme.Primary,
                25F))
            using (Font categoryFont = new Font("Segoe UI", 10F, FontStyle.Bold))
            using (Font titleFont = new Font("Segoe UI", 15F, FontStyle.Bold))
            using (SolidBrush categoryBrush = new SolidBrush(Color.FromArgb(205, 225, 255)))
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.FillRectangle(background, 0, 0, width, height);

                using (SolidBrush glow = new SolidBrush(Color.FromArgb(58, 255, 255, 255)))
                {
                    graphics.FillEllipse(glow, width - 130, -55, 190, 190);
                    graphics.FillEllipse(glow, -75, height - 80, 150, 150);
                }

                graphics.DrawString(
                    string.IsNullOrWhiteSpace(category) ? "SkillHub Service" : category,
                    categoryFont,
                    categoryBrush,
                    new RectangleF(22, 19, width - 44, 25));

                graphics.DrawString(
                    string.IsNullOrWhiteSpace(title) ? "Professional digital service" : title,
                    titleFont,
                    Brushes.White,
                    new RectangleF(22, 50, width - 44, height - 65));
            }

            return bitmap;
        }

        private static string GetInitials(string displayName)
        {
            string[] words = (displayName ?? string.Empty)
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (words.Length == 0)
            {
                return "SH";
            }

            if (words.Length == 1)
            {
                return words[0].Substring(0, 1).ToUpperInvariant();
            }

            return (words[0].Substring(0, 1) + words[words.Length - 1].Substring(0, 1))
                .ToUpperInvariant();
        }

        private static string MakeSafeFileName(string value)
        {
            string source = string.IsNullOrWhiteSpace(value) ? "skillhub-image" : value;

            foreach (char invalidCharacter in Path.GetInvalidFileNameChars())
            {
                source = source.Replace(invalidCharacter, '-');
            }

            return source.Replace(' ', '-').ToLowerInvariant();
        }
    }
}
