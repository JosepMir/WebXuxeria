using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebXuxeria.Controllers
{
    public class PiecesController : Controller
    {
        // GET: Pieces
        public ActionResult Index()
        {
            using (var ctx = new WordsContext())
            {
                var model = new Models.MainModel()
                {
                    Words = ctx.Words.Where(x => x.Type == Word.TypeEnum.Piece && !string.IsNullOrEmpty(x.Part1.Trim())).ToList(),
                };

                return View(model);
            }
        }

      
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult ManagePieces()
        {
            try
            {
                using (var ctx = new WordsContext())
                {
                    var model = new Models.MainModel();
                    model.Words = ctx.Words.Where(x => x.Type == Word.TypeEnum.Piece).ToList();
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        //[HttpPost]
        //public ActionResult ManageWordsAjax(Models.MainModel model)
        //{
        //    if (!(Session["IsAdmin"] != null && Session["IsAdmin"].ToString().ToUpper() == "TRUE"))
        //        return Content("Not allowed", MediaTypeNames.Text.Plain);

        //    string[] wordid = Request.Form.GetValues("wordid");

        //    string[] part1 = Request.Form.GetValues("part1");
        //    string[] part2 = Request.Form.GetValues("part2");
        //    string[] part3 = Request.Form.GetValues("part3");
        //    string[] part4 = Request.Form.GetValues("part4");
        //    string[] part5 = Request.Form.GetValues("part5");
        //    string[] part6 = Request.Form.GetValues("part6");
        //    string[] part7 = Request.Form.GetValues("part7");

        //    using (var ctx = new WordsContext())
        //    {
        //        //var data = ctx.Words.Where(o=>wordid.ToList().Contains(o.WordId.ToString()));
        //        //ctx.Words.RemoveRange(data);

        //        for (int i = 0; i < part1.Length; i++)
        //        {
        //            if (wordid[i] == "-1")
        //                break;

        //            var w = new Word();
        //            w.Part1 = part1[i].Trim();
        //            w.Part2 = part2[i].Trim();
        //            w.Part3 = part3[i].Trim();
        //            w.Part4 = part4[i].Trim();

        //            var wordidval = wordid[i].ToString();
        //            var wordToChange = ctx.Words.Where(x => x.WordId.ToString() == wordidval).SingleOrDefault();
        //            wordToChange.Part1 = part1[i].Trim();
        //            wordToChange.Part2 = part2[i].Trim();
        //            wordToChange.Part3 = part3[i].Trim();
        //            wordToChange.Part4 = part4[i].Trim();


        //        }
        //        ctx.ChangeTracker.DetectChanges(); //alternative is to set  context.Configuration.AutoDetectChangesEnabled = true
        //        ctx.SaveChanges();
        //        //model.Words = ctx.Words.ToList();
        //    }
        //    Response.StatusCode = (int)HttpStatusCode.OK;
        //    return Content("Message sent!", MediaTypeNames.Text.Plain);
        //    //            return View(model);
        //}

    }
}