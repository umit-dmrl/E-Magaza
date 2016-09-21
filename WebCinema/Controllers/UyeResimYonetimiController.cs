using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebCinema.Util;
using WebCinema.ViewModel;
using WebCinema.Models;
using PagedList;
using PagedList.Mvc;

namespace WebCinema.Controllers
{
    public class UyeResimYonetimiController : AdminBaseController
    {
        //
        // GET: /UyeResimYonetimi/
        cinemaDB db = new cinemaDB();
        public ActionResult Index(int? SayfaNo)
        {
            int _sayfaNo = SayfaNo ?? 1;
            var resimler = db.resimler.OrderByDescending(m => m.id).ToPagedList<resimler>(_sayfaNo, 2);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Resimler",resimler);
            }
            return View(resimler);
        }
        public ActionResult ResimYukle()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResimYukle(ResimYukleViewModel model)
        {

            Random rnd = new Random();
            resimler dbModel = new resimler();
            String yuklenmeyenler = "";
            foreach (var file in model.resimler)
            {
                int rasgele = rnd.Next(100, 999999);
                string uzanti = file.FileName.Substring(file.FileName.Length - 3);
                if (uzanti == "png" || uzanti == "jpg" || uzanti == "jpeg" || uzanti == "gif")
                {
                    file.SaveAs(Server.MapPath("~/Uploads/Products/" + rasgele + "." + uzanti));
                    dbModel.resim = rasgele + "." + uzanti;
                    db.resimler.Add(dbModel);
                    db.SaveChanges();
                }
                else
                {
                    yuklenmeyenler += file.FileName+"/";
                }
            }
            ModelState.Clear();

            if (yuklenmeyenler.Length > 0)
            {
                ViewBag.Yuklenmeyenler = yuklenmeyenler;
                ViewBag.State = "error";
            }
            else
            {
                ViewBag.State = "success";
            }
            return View();
        }

        public ActionResult ImageDelete(string ImageID)
        {
            var resimler = db.resimler.OrderByDescending(m => m.id).ToPagedList<resimler>(1, 2);
            try
            {
                int id = int.Parse(ImageID);
                List<resimler> resim = db.resimler.Where(m => m.id == id).ToList();
                if (resim.Count > 0)
                {
                    string resim_adi = "";
                    foreach(resimler val in resim)
                    {
                        resim_adi = val.resim;
                    }
                    System.IO.File.Delete(Server.MapPath("~/Uploads/Products/"+resim_adi));
                    resimler silinecek = db.resimler.Find(id);
                    db.resimler.Remove(silinecek);
                    db.SaveChanges();
                    ViewBag.State = "Success";
                }
                else
                {
                    ViewBag.State = "ResimBulunamadi";
                }
                return View("Index",resimler);
            }catch(FormatException)
            {
                ViewBag.State = "FormatHatasi";
                return View("Index",resimler);
            }
            
        }
    }
}
