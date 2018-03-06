using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace WebXuxeria.Controllers
{
    public class DuplicatesController : Controller
    {
        public ActionResult Index(int? collectionid)
        {
            using (var ctx = new WordsContext())
            {

                if (collectionid==null)
                {
                    CreateNewDuplicateCollection();
                }

                var col = ctx.Collections.Where(x => x.CollectionId == collectionid).SingleOrDefault();
                var dup = ctx.Duplicates.Where(x=>x.Collection_CollectionId==col.CollectionId).OrderBy(x => x.Order).FirstOrDefault();
          
                return RedirectToAction("IndexByDupId", new { id = dup.DuplicateId });
            }
       
        }
        public ActionResult IndexByDupId(int id)
        {
            var model = new Models.DuplicatesModel();
            using (var ctx = new WordsContext())
            {
                //if (id == null)
                //{
                //    if (ctx.Duplicates.Count() == 0)
                //    {
                //        return CreateNewDuplicate();
                //    }
                //    model.Duplicate = ctx.Duplicates.Include(x => x.ImageQuestion).Include(x => x.Image1).Include(x => x.Image2).Include(x => x.Image3).Include(x => x.Image4).OrderBy(i => i.Order).Take(1).FirstOrDefault();
                //}
                //else
                //{
                    model.Duplicate = ctx.Duplicates.Include(x => x.ImageQuestion).Include(x => x.Image1).Include(x => x.Image2).Include(x => x.Image3).Include(x => x.Image4).Where(x => x.DuplicateId == id).FirstOrDefault(); //.OrderBy(x=>x.Order).ToList(),
                //}
                return View(model);
            }
        }

        public ActionResult Next(int id)
        {
            var model = new Models.DuplicatesModel();
            using (var ctx = new WordsContext())
            {
                model.Duplicate = ctx.Duplicates.Where(x => x.DuplicateId == id).FirstOrDefault(); //.OrderBy(x=>x.Order).ToList(),
                var order = model.Duplicate.Order;
                var oldDuplicate = model.Duplicate;

                model.Duplicate = ctx.Duplicates.Where(x => x.Order > order && x.Collection.CollectionId == oldDuplicate.Collection.CollectionId).OrderBy(x => x.Order).FirstOrDefault() ?? model.Duplicate;
                
                return RedirectToAction("IndexByDupId", "Duplicates", new { id = model.Duplicate.DuplicateId });
            }
        }

        public ActionResult Previous(int id)
        {
            var model = new Models.DuplicatesModel();
            using (var ctx = new WordsContext())
            {
                model.Duplicate = ctx.Duplicates.Where(x => x.DuplicateId == id).FirstOrDefault(); //.OrderBy(x=>x.Order).ToList(),
                var order = model.Duplicate.Order;
                var oldDuplicate = model.Duplicate;

                model.Duplicate = ctx.Duplicates.Where(x => x.Order < order && x.Collection.CollectionId == oldDuplicate.Collection.CollectionId).OrderByDescending(x => x.Order).FirstOrDefault() ?? model.Duplicate;

                return RedirectToAction("IndexByDupId", "Duplicates", new { id = model.Duplicate.DuplicateId });
            }
        }


        public ActionResult CreateNewDuplicateCollection()
        {
            try
            {
                using (var ctx = new WordsContext())
                {
                    //var model = new Models.DuplicatesModel();
                    var col = new Collection() { Type = collectionType.duplicates, Name = "xxx" };
                    ctx.Collections.Add(col);
                    ctx.SaveChanges();

                    //CREATE NEW DUPLICATE
                    for (int i = 0; i < 10; i++)
                    {
                        //int order = ctx.Duplicates.OrderByDescending(i => i.Order).Take(1).Select(x => x.Order).FirstOrDefault();
                        //order++;
                        var dup = new Duplicate() { Answer1 = "xxx", Answer2 = "xxx", Answer3 = "xxx", Answer4 = "xxx", Order = i+1, Collection = col };
                        ctx.Duplicates.Add(dup);
                        //col.Duplicates.Add(dup);
                    }
                    ctx.SaveChanges();
                    //modelDuplicate = dup;

                    return null;// View(model);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public ActionResult CreateNewDuplicate()
        //{
        //    try
        //    {
        //        using (var ctx = new WordsContext())
        //        {
        //            var model = new Models.DuplicatesModel();

        //            //CREATE NEW DUPLICATE
        //            int order = ctx.Duplicates.OrderByDescending(i => i.Order).Take(1).Select(x => x.Order).FirstOrDefault();
        //            order++;
        //            var dup = ctx.Duplicates.Add(new Duplicate() { Answer1 = "xxx", Answer2 = "xxx", Answer3 = "xxx", Answer4 = "xxx", Order = order });
        //            ctx.SaveChanges();
        //            model.Duplicate = dup;

        //            return View(model);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public ActionResult CreateCollection()
        {

            using (var ctx = new WordsContext())
            {
                var collection = new Collection()
                {
                    Name = "Empty" + DateTime.Now.Millisecond
                };

                var dup = new List<Duplicate>();
                for (int i = 0; i < 10; i++)
                {
                    dup.Add(new Duplicate()
                    { 
                        Order = i,
                        Collection = collection
                    });
                }
                //collection.Duplicates = dup;
                collection.Type = collectionType.duplicates;
                ctx.Collections.Add(collection);
                ctx.SaveChanges();
            }
            Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
            return RedirectToAction("Index", "Duplicates");
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult ManageDuplicates(int id)
        {
            try
            {
                using (var ctx = new WordsContext())
                {
                    var model = new Models.DuplicatesModel();

                    //GET EXISTING DUPLICATE
                    model.Duplicate = ctx.Duplicates.Include(x => x.ImageQuestion).Include(x=>x.Image1).Include(x => x.Image2).Include(x => x.Image3).Include(x => x.Image4).Where(x => x.DuplicateId == id).FirstOrDefault();
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        public ActionResult ManageDuplicatesAjax(string duplicateid, string question, string answer1, string answer2, string answer3, string answer4, bool answer1iscorrect, bool answer2iscorrect, bool answer3iscorrect, bool answer4iscorrect)
        {


            if (!(Session["IsAdmin"] != null && Session["IsAdmin"].ToString().ToUpper() == "TRUE"))
                return Content("Not allowed", MediaTypeNames.Text.Plain);


            using (var ctx = new WordsContext())
            {
                var dup = ctx.Duplicates.Where(x => x.DuplicateId.ToString() == duplicateid).SingleOrDefault();
                dup.Question = question;

                dup.Answer1 = answer1;
                dup.Answer2 = answer2;
                dup.Answer3 = answer3;
                dup.Answer4 = answer4;

                dup.Answer1IsCorrect = answer1iscorrect;
                dup.Answer2IsCorrect = answer2iscorrect;
                dup.Answer3IsCorrect = answer3iscorrect;
                dup.Answer4IsCorrect = answer4iscorrect;
                ctx.ChangeTracker.DetectChanges(); //alternative is to set  context.Configuration.AutoDetectChangesEnabled = true
                ctx.SaveChanges();

            }

            //  Response.StatusCode = (int)HttpStatusCode.OK;
            return Content("Message sent!", MediaTypeNames.Text.Plain);
            //  return View(model);
        }




        [HttpPost]
        public ActionResult UploadImage(string FileUpload, string DuplicateId, int Num)
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
                        scaledImage.Save(ms, ImageFormat.Jpeg);

                        var dup = ctx.Duplicates.Where(x => x.DuplicateId.ToString() == DuplicateId).SingleOrDefault();
                        switch (Num)
                        {
                            case 0:
                                dup.ImageQuestion = new Picture();
                                dup.ImageQuestion.Content = ms.ToArray();
                                break;
                            case 1:
                                dup.Image1 = new Picture();
                                dup.Image1.Content = ms.ToArray();
                                break;
                            case 2:
                                dup.Image2 = new Picture();
                                dup.Image2.Content = ms.ToArray();
                                break;
                            case 3:
                                dup.Image3 = new Picture();
                                dup.Image3.Content = ms.ToArray();
                                break;
                            case 4:
                                dup.Image4 = new Picture();
                                dup.Image4.Content = ms.ToArray();
                                break;
                        }


                        ctx.ChangeTracker.DetectChanges();
                        ctx.SaveChanges();
                    }
                }
            }

            return Json(new { success = "Valid" });
        }

        public ActionResult RemoveImage(string Id, int Num)
        {
            if (!(Session["IsAdmin"] != null && Session["IsAdmin"].ToString().ToUpper() == "TRUE"))
                return Content("Not allowed", System.Net.Mime.MediaTypeNames.Text.Plain);

            using (var ctx = new WordsContext())
            {
                var dup = ctx.Duplicates.Where(x => x.DuplicateId.ToString() == Id).SingleOrDefault();
                switch (Num)
                {
                    case 0:
                        dup.ImageQuestion = null;
                        break;
                    case 1:
                        dup.Image1 = null; 
                        break;
                    case 2:
                        dup.Image2 = null;
                        break;
                    case 3:
                        dup.Image3 = null;
                        break;
                    case 4:
                        dup.Image4 = null;
                        break;
                }

                ctx.ChangeTracker.DetectChanges();
                ctx.SaveChanges();


               return RedirectToAction("ManageDuplicates", new { id = dup.DuplicateId });
            }

        }
 
    }
}