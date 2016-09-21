using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebCinema.Util;
using WebCinema.Models;
using WebCinema.ViewModel;
using System.IO;

namespace WebCinema.Controllers
{
    public class UyeController : BaseController
    {
        //
        // GET: /Uye/
        cinemaDB db = new cinemaDB();

        public void OnYuklemeler()
        {
            List<kategoriler> kategoriler = db.kategoriler.Where(m => m.onay == "1").ToList();
            ViewBag.kategoriler = kategoriler;
        }

        public ActionResult Index()
        {
            OnYuklemeler();
            string eposta = Session["eposta"].ToString();
            List<uyeler> uye = db.uyeler.Where(m => m.eposta == eposta).ToList();
            ViewBag.Profil = uye;

            return View();
        }

        public ActionResult ProfilDuzenle(string islem)
        {
            OnYuklemeler();
            if (islem == "success")
            {
                ViewBag.State = "success";
            }
            else if (islem == "error")
            {
                ViewBag.State = "error";
            }
            string eposta = Session["eposta"].ToString();
            List<uyeler> uye = db.uyeler.Where(m => m.eposta == eposta).ToList();
            ViewBag.Profil = uye;
            return View();
        }
        [HttpPost]
        public ActionResult ProfilDuzenle(KayitFormuView model)
        {
            OnYuklemeler();
            string eposta = Session["eposta"].ToString();
            List<uyeler> profil = db.uyeler.Where(m => m.eposta == eposta).ToList();
            ViewBag.Profil = profil;

            uyeler uye = db.uyeler.FirstOrDefault(m => m.eposta == eposta);
            uye.adsoyad = model.adsoyad;
            uye.eposta = model.eposta;
            uye.parola = model.parola;

            if (model.ImageUpload != null && model.ImageUpload.ContentLength > 0)
            {
                //yeni bir resim seçilmiş eskisini sil yenisini kaydet
                Random rasgele = new Random();
                int sayi = rasgele.Next(100, 999999);
                string gelen_isim = model.ImageUpload.FileName.Substring(model.ImageUpload.FileName.Length - 3);
                if (gelen_isim == "png" || gelen_isim == "jpg" || gelen_isim == "jpeg" || gelen_isim == "gif")
                {
                    //önceki resmi silelim
                    System.IO.File.Delete(Server.MapPath("~/Uploads/Profils/" + uye.resim.ToString()));
                    model.ImageUpload.SaveAs(Server.MapPath("~/Uploads/Profils/" + sayi + "." + gelen_isim));
                    uye.resim = sayi + "." + gelen_isim;
                }
                else
                {
                    ViewBag.State = "format_error";
                    return View("ProfilDuzenle");
                }
            }

            db.SaveChanges();
            ViewBag.State = "success";
            return View();
        }

        public ActionResult MesajGonder()
        {
            OnYuklemeler();

            return View();
        }

        [HttpPost]
        public ActionResult MesajGonder(MesajViewModel model)
        {
            OnYuklemeler();

            mesajlar mesajModel = new mesajlar();
            mesajModel.gonderen = Session["eposta"].ToString();
            mesajModel.konu = model.konu;
            mesajModel.mesaj = model.mesaj;
            mesajModel.okunma = "0";
            mesajModel.tarih = System.DateTime.Now.Date.ToShortDateString();

            db.mesajlar.Add(mesajModel);
            db.SaveChanges();

            ViewBag.State = "success";
            ModelState.Clear();
            return View();
        }

        public ActionResult GelenKutusu()
        {
            OnYuklemeler();
            string eposta = Session["eposta"].ToString();
            List<cevaplar> gelenler = db.cevaplar.Where(m => m.kime == eposta).ToList();
            ViewBag.GelenMesajlar = gelenler;
            return View();
        }

        public ActionResult MesajOku(int? id)
        {
            OnYuklemeler();
            string eposta = Session["eposta"].ToString();
            string gelen_id = id.ToString();
            List<cevaplar> mesaj = db.cevaplar.Where(m => m.mesajID == gelen_id && m.kime==eposta).ToList();
            List<mesajlar> gonderilen = db.mesajlar.Where(m => m.id == id && m.gonderen == eposta).ToList();
            if (mesaj.Count() > 0)
            {
                ViewBag.Mesaj = mesaj;
                ViewBag.Gonderilen = gonderilen;
            }
            else
            {
                List<cevaplar> gelenler = db.cevaplar.Where(m => m.kime == eposta).ToList();
                ViewBag.GelenMesajlar = gelenler;
                ViewBag.State = "kayit_yok";
                return View("GelenKutusu");
            }
            return View();
        }

        public void Logout()
        {
            Session.Abandon();
            Response.Redirect("~/Anasayfa");
        }

    }
}
