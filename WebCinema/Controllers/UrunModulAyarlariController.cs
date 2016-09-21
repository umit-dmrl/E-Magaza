using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebCinema.ViewModel;

namespace WebCinema.Controllers
{
    public class UrunModulAyarlariController : Controller
    {
        //
        // GET: /Slider Modülü & Sizin İçin Seçtiklerimiz & Otomatik Popup/

        public ActionResult Index(string state)
        {
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

    }
}
