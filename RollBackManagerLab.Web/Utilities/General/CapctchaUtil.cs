using RollBackManagerLab.Web.Model;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace RollBackManagerLab.Web.Utilities.General
{
    public static class CaptchaUtil
    {
        public static CaptchaModel GenerateCaptcha()
        {
            string captchaText = GenerateRandomCaptchaText();
            using (var image = new Bitmap(200, 50))
            {
                using (var graphics = Graphics.FromImage(image))
                {
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    graphics.Clear(Color.LightGray);

                    // Rastgele renklerde çizgiler çizin
                    Random rand = new Random();
                    for (int i = 0; i < 10; i++)
                    {
                        int x1 = rand.Next(image.Width);
                        int y1 = rand.Next(image.Height);
                        int x2 = rand.Next(image.Width);
                        int y2 = rand.Next(image.Height);
                        Color randomColor = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
                        using (var pen = new Pen(randomColor, 2))
                        {
                            graphics.DrawLine(pen, x1, y1, x2, y2);
                        }
                    }

                    // CAPTCHA metnini çizin
                    using (var font = new Font("Arial", 20, FontStyle.Bold))
                    {
                        PointF textPosition = new PointF(10, 10);
                        using (var brush = new SolidBrush(Color.Black))
                        {
                            graphics.DrawString(captchaText, font, brush, textPosition);
                        }
                    }
                }

                // Resmi wwwroot/captcha klasörüne kaydedin
                string captchaFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "captcha");
                if (!Directory.Exists(captchaFolderPath))
                {
                    Directory.CreateDirectory(captchaFolderPath);
                }

                string imagePath = Path.Combine(captchaFolderPath, $"{captchaText}.png");

                if (!File.Exists(imagePath))
                {
                    image.Save(imagePath, ImageFormat.Png);
                }

                // CAPTCHA metni ve resminin dosya adını dönüş yapın
                return new CaptchaModel { Text = captchaText, Image = $"{captchaText}.png" };
            }
        }

        private static string GenerateRandomCaptchaText()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[new Random().Next(s.Length)]).ToArray());
        }
    }
}
