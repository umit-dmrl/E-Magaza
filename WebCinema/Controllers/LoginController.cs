using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebCinema.Models;
using WebCinema.ViewModel;

namespace WebCinema.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        cinemaDB db = new cinemaDB();

        public void OnYuklemeler()
        {
            List<kategoriler> kategoriler = db.kategoriler.Where(m => m.onay == "1").ToList();
            ViewBag.kategoriler = kategoriler;
        }
        public ActionResult Index()
        {
            OnYuklemeler();
            LoginViewModel model = new LoginViewModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel model)
        {
            OnYuklemeler();
            List<uyeler> uye = db.uyeler.Where(m => m.eposta == model.eposta && m.parola == model.parola).ToList();
            if (uye.Count() > 0)
            {
                foreach (uyeler var in uye)
                {
                    Session["username"] = var.adsoyad.ToString();
                    Session["eposta"] = var.eposta.ToString();
                }
                Response.Redirect("~/Uye");
            }
            else
            {
                model.mesaj = "Giriş Başarısız!";
            }
            //ModelState.Clear();
            //model.mesaj = "Giriş Yapılıyor";
            return View(model);
        }

    }
}
