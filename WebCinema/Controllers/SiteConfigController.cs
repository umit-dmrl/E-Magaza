using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebCinema.Models;
using WebCinema.ViewModel;

namespace WebCinema.Controllers
{
    public class SiteConfigController : Controller
    {
        //
        // GET: /SiteConfig/

        // config setting
        cinemaDB db = new cinemaDB();
        public ActionResult Index()
        {
            
            List<SiteAyarlari> dbSettingModel = db.SiteAyarlari.Where(m=>m.id==1).ToList();
            ViewBag.Config = dbSettingModel;
            return View();
        }

        [HttpPost]
        public JsonResult Index(FormCollection form)
        {
            JsonResultModel jModel = new JsonResultModel();
            try
            {
                SiteAyarlari dbModel = db.SiteAyarlari.FirstOrDefault(m=>m.id==1);

                dbModel.site_title = form["title"].ToString().Trim();
                dbModel.site_description = form["description"].ToString().Trim();
                dbModel.site_slogan = form["slogan"].ToString().Trim();
                dbModel.site_keyword = form["keyword"].ToString().Trim();

                db.SaveChanges();

                jModel.IsSuccess = true;
                jModel.UserMessage = "Site Ayarlarınız Güncellendi";
            }catch(Exception ex)
            {
                jModel.IsSuccess = false;
                jModel.UserMessage = "Kayıt İşlemi Başarısız!";
            }
            return Json(jModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Seo(string state)
        {
            ViewBag.State = state;
            List<SeoOptimizasyonu> google_code = db.SeoOptimizasyonu.Where(m=>m.id==1).ToList();
            ViewBag.GoogleCode = google_code;
            return View();
        }

        [HttpPost]
        public JsonResult Seo(FormCollection form)
        {
            SeoViewModel jModel = new SeoViewModel();
            try
            {
                SeoOptimizasyonu update_data = db.SeoOptimizasyonu.FirstOrDefault(m=>m.id==1);
                update_data.google_analytic_code = form["google_code"].ToString().Trim();
                db.SaveChanges();

                jModel.IsSuccess = true;
                jModel.AjaxMessage = "Google Analytic Kodu Başarıyla Eklendi";
            }
            catch
            {
                jModel.IsSuccess = false;
                jModel.AjaxMessage = "Bir Hata Nedeniyle İşlem Başarısız Oldu!";
            }
            return Json(jModel,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void SiteMapUpload(SeoViewModel model)
        {
            string uzanti = model.dosya.FileName.Substring(model.dosya.FileName.Length-3);
            if (uzanti == "xml")
            {
                model.dosya.SaveAs(Server.MapPath("~/Uploads/SiteMap/sitemap.xml"));
                Response.Redirect("~/SiteConfig/Seo/?state=success");
            }
            else
            {
                Response.Redirect("~/SiteConfig/Seo/?state=error_format");
            }
        }

        public ActionResult Contact(string state)
        {
            ViewBag.State = state;
            List<IletisimBilgileri> contact = db.IletisimBilgileri.Where(m => m.id == 1).ToList();
            ViewBag.Contact = contact;
            return View();
        }

        [HttpPost]
        public void Contact(FormCollection form)
        {
            try
            {
                IletisimBilgileri bilgiler = db.IletisimBilgileri.FirstOrDefault(m=>m.id==1);
                bilgiler.adres = form["adres"].ToString().Trim();
                bilgiler.tel1 = form["tel1"].ToString().Trim();
                bilgiler.tel2 = form["tel2"].ToString().Trim();
                bilgiler.tel3 = form["tel3"].ToString().Trim();
                bilgiler.gsm1 = form["gsm1"].ToString().Trim();
                bilgiler.gsm2 = form["gsm2"].ToString().Trim();
                bilgiler.gsm3 = form["gsm3"].ToString().Trim();
                bilgiler.google_maps = form["maps"].ToString().Trim();
                bilgiler.facebook_adress = form["facebook_adress"].ToString().Trim();
                bilgiler.twitter_adress = form["twitter_adress"].ToString().Trim();
                bilgiler.instagram_adress = form["instagram_adress"].ToString().Trim();
                db.SaveChanges();
                Response.Redirect("~/SiteConfig/Contact/?state=success");
            }
            catch
            {
                Response.Redirect("~/SiteConfig/Contact/?state=error");
            }
        }
    }
}
