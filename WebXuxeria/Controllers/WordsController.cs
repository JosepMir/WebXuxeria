using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;

namespace WebXuxeria.Controllers
{
    public class WordsController : Controller
    {

        public ActionResult Index(int? collectionid, bool ordenades = false)
        {

            using (var ctx = new WordsContext())
            {
                if (collectionid == null)
                    collectionid = ctx.Collections.Min(x => x.CollectionId);

                var model = new Models.MainModel()
                {
                    Words = ctx.Words.Include(x=>x.Image).Where(x => x.Collection.CollectionId == collectionid).ToList(),
                    CollectionId = collectionid
                };

                var c = ctx.Collections.Include(x => x.Image).Where(x => x.CollectionId == collectionid).FirstOrDefault();
                if (c != null)
                    model.CollectionName = c.Name;
                if (c.Image!=null)
                    model.CollectionPictureId = c.Image.PictureId;

                model.Ordenades = ordenades;
                return View(model);
            }

        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult ManageWords(int? collectionid)
        {
            try
            {
                using (var ctx = new WordsContext())
                {
                    if (collectionid == null)
                        collectionid = -1;

                    var model = new Models.MainModel();
                    model.Words = ctx.Words.Include(x=>x.Image).Where(x => x.Collection.CollectionId == collectionid).ToList();
                    model.CollectionId = collectionid;
                    var c = ctx.Collections.Include(x => x.Image).Where(x => x.CollectionId == collectionid).FirstOrDefault();
                    if (c != null)
                        model.CollectionName = c.Name;
                    if (c.Image != null)
                        model.CollectionPictureId = c.Image.PictureId;
                    return View(model);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



        public ActionResult CreateCollection()
        {

            using (var ctx = new WordsContext())
            {
                var collection = new Collection()
                {
                    Name = "Empty" + DateTime.Now.Millisecond
                };

                var words = new List<Word>();
                for (int i = 0; i < 10; i++)
                {
                    words.Add(new Word()
                    {
                        Part1 = "",
                        Part2 = "",
                        Part3 = "",
                        Part4 = "",
                        Collection = collection
                    });
                }
                //collection.Words = words;
                collection.Type = collectionType.words;
                ctx.Collections.Add(collection);
                ctx.SaveChanges();
            }
            Response.StatusCode = (int)HttpStatusCode.OK;
            return RedirectToAction("Index", "Words");
        }

        public ActionResult DeleteCollection(int collectionid)
        {
            using (var ctx = new WordsContext())
            {
                ctx.Words.RemoveRange(ctx.Words.Where(x => x.Collection.CollectionId == collectionid));
                ctx.Collections.RemoveRange(ctx.Collections.Where(x => x.CollectionId == collectionid));
                ctx.SaveChanges();
            }
            return RedirectToAction("Index", "Words");
        }

        [HttpPost]
        public ActionResult ChangeSkin(string skin)
        {
            try
            {
                Session["IsAdmin"] = (skin == "yellowish");
                if (this.HttpContext.Request.IsLocal)
                    Session["IsAdmin"] = true;
            } catch
            {
              
            }
            return null;
        }

        [HttpPost]
        public ActionResult ChangeCollectionName(string collectionname, int collectionid)
        {
            using (var ctx = new WordsContext())
            {
                var collectionToChange = ctx.Collections.Where(x => x.CollectionId == collectionid).SingleOrDefault();
                collectionToChange.Name = collectionname;
                ctx.ChangeTracker.DetectChanges();
                ctx.SaveChanges();
            }
            return Json(new { success = "Valid" });
        }


        [HttpPost]
        public ActionResult ManageWords(string wordid, string partid, string content)
        {
            if (!(Session["IsAdmin"]!=null && Session["IsAdmin"].ToString().ToUpper()=="TRUE"))
                return Content("Not allowed", MediaTypeNames.Text.Plain);

            string Partid = partid.First().ToString().ToUpper() + partid.Substring(1);
            using (var ctx = new WordsContext())
            {
                var wordToChange = ctx.Words.Include(x=>x.Image).Where(x => x.WordId.ToString() == wordid).SingleOrDefault();
                wordToChange.GetType().GetProperty(Partid).SetValue(wordToChange, content);

                ctx.ChangeTracker.DetectChanges(); //alternative is to set  context.Configuration.AutoDetectChangesEnabled = true
                ctx.SaveChanges();
            }
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Content("Message sent!", MediaTypeNames.Text.Plain);
        }

        [HttpPost]
        public ActionResult UploadImage(string FileUpload, string WordId, string CollectionId)
        {
            if (!(Session["IsAdmin"] != null && Session["IsAdmin"].ToString().ToUpper() == "TRUE"))
                return Content("Not allowed", System.Net.Mime.MediaTypeNames.Text.Plain);


            using (var ctx = new WordsContext())
            {

                using (var image = System.Drawing.Image.FromStream(Request.Files[0].InputStream))
                {
                    using (var scaledImage = ImageController.ScaleImage(image, 300, 300))
                    {

                        MemoryStream ms = new MemoryStream();
                        scaledImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        if (CollectionId == "undefined")
                        {
                            var word = ctx.Words.Where(x => x.WordId.ToString() == WordId).SingleOrDefault();
                            word.Image = new Picture();
                            word.Image.Content = ms.ToArray();
                        }
                        else
                        {
                            var collection = ctx.Collections.Where(x => x.CollectionId.ToString() == CollectionId).SingleOrDefault();
                            collection.Image = new Picture();
                            collection.Image.Content = ms.ToArray();
                        }

                        ctx.ChangeTracker.DetectChanges();
                        ctx.SaveChanges();
                    }
                }
            }

            return Json(new { success = "Valid" });
        }
    }
}
