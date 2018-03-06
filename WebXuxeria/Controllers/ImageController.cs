using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.Drawing.Imaging;

namespace WebXuxeria.Controllers
{
    public class ImageController : Controller
    {
        [OutputCache(Duration = 86400, VaryByParam = "*")]
        public ActionResult Index(Guid Id)
        {
            using (var ctx = new WordsContext())
            {
                var pic = ctx.Picture.Where(x => x.PictureId == Id).SingleOrDefault();
                if (pic != null)
                {
                    if (pic.Content != null && pic.Content.Length > 0)
                        return File(pic.Content, "image/jpeg");

                }

            }
            return new FilePathResult(HttpContext.Server.MapPath("~/Content/blank.png"), "image/png");
        }

       
        public static string GetMimeType(ImageFormat imageFormat)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            return codecs.First(codec => codec.FormatID == imageFormat.Guid).MimeType;
        }

        public static ImageFormat GetImageFormat(string MimeType)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo ici = codecs.First(codec => codec.MimeType == MimeType);
            return new ImageFormat(ici.FormatID);
            
                
        }


        public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }
    }
}