using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebCinema.ViewModel;
using WebCinema.Models;

namespace WebCinema.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        cinemaDB db = new cinemaDB();
        public ActionResult Index()
        {
            if (Session["user_admin"] == null || Session["user_admin"] == "")
            {
                return View("Login");
            }
            else
            {
                return View("Panel");
            }
            
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(AdminLoginViewModel model)
        {
            List<uyeler> uye = db.uyeler.Where(m => m.eposta == model.eposta && m.parola == model.parola).ToList();
            if (uye.Count() > 0)
            {
                foreach (uyeler val in uye)
                {
                    Session["user_admin"] = val.eposta;
                    Session["user_adminName"] = val.adsoyad;
                }
                return View("Panel");
            }
            else
            {
                ViewBag.ErrorMessage = "E-Posta Adresiniz Veya Parolanız Hatalı!";
                return View("Login");
            }
            
        }
        public ActionResult Panel()
        {
            return View();
        }

    }
}
