using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebCinema.Util;
using WebCinema.Models;

namespace WebCinema.Controllers
{
    public class UyeSablonYonetimiController : AdminBaseController
    {
        //
        // GET: /UyeSablonYonetimi/
        cinemaDB db = new cinemaDB();
        public void Index()
        {
            Response.Redirect("~/UyeSablonYonetimi/Anasayfa");
        }

        public ActionResult Anasayfa(string state)
        {
            List<HomeLayoutModule> layout_module = db.HomeLayoutModule.OrderBy(m => m.sira).ToList();
            ViewBag.Modules = layout_module;
            ViewBag.State = state;
            return View();
        }
        [HttpPost]
        public void HomeLayoutUpdate(FormCollection form)
        {
            string[] degerler = form["degerler"].Split('&');
            int sayi = 1;
            for (int i = 0; i < degerler.Length; i++)
            {
                string gelen_id = degerler[i].Substring(degerler[i].Length - 1);
                int id = int.Parse(gelen_id);
                HomeLayoutModule moduller = db.HomeLayoutModule.FirstOrDefault(m=>m.id==id);
                moduller.sira = sayi.ToString();
                db.SaveChanges();
                sayi++;
            }
            Response.Redirect("~/UyeSablonYonetimi/Anasayfa/?state=success");
        }

    }
}
