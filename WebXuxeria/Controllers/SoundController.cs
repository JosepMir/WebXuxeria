using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebXuxeria.Controllers
{
    public class SoundController : Controller
    {
        //[OutputCache(Duration = 3600, VaryByParam = "*")]
        public ActionResult Index(string WordId, int PartId)
        {
            using (var ctx = new WordsContext())
            {
                var word = ctx.Words.Where(x => x.WordId.ToString() == WordId).SingleOrDefault();
                if (word == null)
                    return null;

                byte[] data = null;
                switch (PartId)
                {
                    case 1:
                        data = word.Part1Sound;
                        break;
                    case 2:
                        data = word.Part2Sound;
                        break;
                    case 3:
                        data = word.Part3Sound;
                        break;
                    case 4:
                        data = word.Part4Sound;
                        break;
                    case 5:
                        data = word.Part5Sound;
                        break;
                    case 6:
                        data = word.Part6Sound;
                        break;
                    case 7:
                        data = word.Part7Sound;
                        break;
                }
                if (data != null)
                    return File(data, "audio/mpeg");
                else
                    return null;
            }
        }

        [HttpPost]
        public ActionResult Upload(string File, int WordId, int PartId)
        {
            if (!(Session["IsAdmin"] != null && Session["IsAdmin"].ToString().ToUpper() == "TRUE"))
                return Content("Not allowed", System.Net.Mime.MediaTypeNames.Text.Plain);


            var file = Request.Files[0];

        
            using (var ctx = new WordsContext())
            {
                var wordToChange = ctx.Words.Where(x => x.WordId == WordId).SingleOrDefault();
                var bytes = SoundController.ReadFully(file.InputStream); 
                //var data = ConvertWavToMp3(bytes);
                switch (PartId)
                {
                    case 1:
                        wordToChange.Part1Sound = bytes;
                        break;
                    case 2:
                        wordToChange.Part2Sound = bytes;
                        break;
                    case 3:
                        wordToChange.Part3Sound = bytes;
                        break;
                    case 4:
                        wordToChange.Part4Sound = bytes;
                        break;
                    case 5:
                        wordToChange.Part5Sound = bytes;
                        break;
                    case 6:
                        wordToChange.Part6Sound = bytes;
                        break;
                    case 7:
                        wordToChange.Part7Sound = bytes;
                        break;
                }

                ctx.ChangeTracker.DetectChanges();
                ctx.SaveChanges();
            }

            return Json(new { success = "Successfully uploaded file for Wordid:" + WordId +" PartId:" + PartId });
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
         

    }
}