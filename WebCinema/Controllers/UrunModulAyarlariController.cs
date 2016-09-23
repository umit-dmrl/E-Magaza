using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebCinema.ViewModel;
using WebCinema.Models;

namespace WebCinema.Controllers
{
    public class UrunModulAyarlariController : Controller
    {
        //
        // GET: /Slider Modülü & Sizin İçin Seçtiklerimiz & Otomatik Popup/
        cinemaDB db = new cinemaDB();
        public ActionResult Index(string state)
        {
            List<SliderResimleri> slider_resimleri = db.SliderResimleri.ToList();
            foreach (SliderResimleri item in slider_resimleri)
            {
                ViewBag.SelectedImages = item.resimler.Split(',');
            }
            //ViewBag.SelectedImages = slider_resimleri;
            ViewBag.State = state;
            return View();
        }

        [HttpPost]
        public void SliderImageUpload(ResimYukleViewModel model)
        {
            Random rnd = new Random();
            string yuklenmeyenler = "";
            foreach (var item in model.resimler)
            {
                int rasgele = rnd.Next(99, 999999);
                string uzanti = item.FileName.Substring(item.FileName.Length - 3);
                if (uzanti == "png" || uzanti == "jpg" || uzanti == "jpeg" || uzanti == "gif")
                {
                    item.SaveAs(Server.MapPath("~/Uploads/Sliders/" + rasgele + "." + uzanti));
                }
                else
                {
                    yuklenmeyenler += item.FileName + "/";
                }
            }
            if (yuklenmeyenler.Length > 0)
            {
                Response.Redirect("~/UrunModulAyarlari/?state=error");
            }
            else
            {
                Response.Redirect("~/UrunModulAyarlari/?state=success");
            }
        }

        [HttpPost]
        public void SliderImagesUpdate(FormCollection form)
        {
            if (form["select_image"] == null)
            {
                Response.Redirect("~/UrunModulAyarlari/?state=error_update");
            }
            else
            {
                string resimler = "";
                foreach (var item in form["select_image"])
                {
                    resimler += item.ToString();
                }
                SliderResimleri slider = db.SliderResimleri.FirstOrDefault(m => m.id == 1);
                slider.resimler = resimler;
                db.SaveChanges();
                Response.Redirect("~/UrunModulAyarlari/?state=success_update");
            }
        }

        public void SliderImageRemove(string image)
        {
            if (image == "" || image == null)
            {
                Response.Redirect("~/UrunModulAyarlari/?state=error_image_notfound");
            }
            else
            {
                try
                {
                    System.IO.File.Delete(Server.MapPath("~/Uploads/Sliders/" + image));
                    Response.Redirect("~/UrunModulAyarlari/?state=success_image_remove");
                }catch(FileNotFoundException){
                    Response.Redirect("~/UrunModulAyarlari/?state=error_image_remove");
                }
            }
        }

    }
}
