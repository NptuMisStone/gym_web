<%@ WebHandler Language="C#" Class="gym_web.Coach.Captcha" %>
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Web;
using System.Web.SessionState;

namespace gym_web.Coach
{
    public class Captcha : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "image/gif";
            VerifyNow(context);
        }

        public bool IsReusable
        {
            get { return false; }
        }

        private void VerifyNow(HttpContext context)
        {
            string randomString = context.Session["ImgText"] as string;
            CreateImage(context, randomString);
        }

        private void CreateImage(HttpContext context, string imageText)
        {
            Bitmap bmpImage = new Bitmap(1, 1);
            Font font = new Font("Verdana", 24, FontStyle.Bold, GraphicsUnit.Point);
            Graphics graphics = Graphics.FromImage(bmpImage);
            int width = (int)graphics.MeasureString(imageText, font).Width;
            int height = (int)graphics.MeasureString(imageText, font).Height;

            bmpImage = new Bitmap(bmpImage, new Size(width, height));
            Random r = new Random();
            graphics = Graphics.FromImage(bmpImage);
            graphics.Clear(NextColor(r));
            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

            int avgWidth = width / imageText.Length;
            for (int i = 0; i < imageText.Length; i++)
            {
                font = new Font("Verdana", r.Next(12, 24), FontStyle.Bold, GraphicsUnit.Point);
                graphics.DrawString(imageText.Substring(i, 1), font, new SolidBrush(NextColor(r)), avgWidth * i, 0);
            }

            graphics.Flush();

            for (int i = 0; i < 10; i++)
                DrawRandomLine(graphics, height, width, r);

            bmpImage.Save(context.Response.OutputStream, ImageFormat.Gif);

            graphics.Dispose();
            font.Dispose();
            bmpImage.Dispose();
        }

        private static Color NextColor(Random r)
        {
            return Color.FromArgb(r.Next(255), r.Next(255), r.Next(255));
        }

        private static void DrawRandomLine(Graphics graphics, int height, int width, Random random)
        {
            Pen pen = new Pen(NextColor(random));
            pen.Width = random.Next(3);
            Point p1 = new Point(random.Next(width), random.Next(height));
            Point p2 = new Point(random.Next(width), random.Next(height));
            graphics.DrawLine(pen, p1, p2);
        }
    }
}