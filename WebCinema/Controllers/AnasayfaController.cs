using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using WebCinema.Models;
using WebCinema.ViewModel;

namespace WebCinema.Controllers
{
    public class AnasayfaController : Controller
    {
        //
        // GET: /Anasayfa/
        cinemaDB db = new cinemaDB();
        public ActionResult Index(string islem)
        {
            if (islem == "success")
            {
                ViewBag.State = "success";
            }
            else if (islem == "error")
            {
                ViewBag.State = "error";
            }
            List<kategoriler> kategoriler = db.kategoriler.Where(m => m.onay == "1").ToList();
            List<SliderResimleri> selected_slider_image = db.SliderResimleri.ToList();
            foreach (SliderResimleri item in selected_slider_image)
            {
                ViewBag.SelectedSliderImages = item.resimler.Split(',');
            }
            ViewBag.kategoriler = kategoriler;
            return View();
        }
        public ActionResult Kategori(int? id)
        {
            int satir = db.kategoriler.Where(m => m.id == id).Count();
            List<kategoriler> kategoriler = db.kategoriler.Where(m => m.onay == "1").ToList();
            ViewBag.kategoriler = kategoriler;
            if (satir == 0)
            {
                Response.Redirect("~/Anasayfa/?islem=error");
                return View("Index");
            }
            else
            {
                string kategori_id = id.ToString(); 
                List<makaleler> makale = db.makaleler.Where(m => m.kategoriID == kategori_id && m.onay=="1").ToList();
                if (makale.Count() == 0)
                {
                    Response.Redirect("~/Anasayfa/?islem=error");
                    return View("Index");
                }
                else
                {
                    
                    ViewBag.Makale = makale;
                    return View();
                }
            }
        }
        public ActionResult KayitOl(string islem)
        {
            List<kategoriler> kategoriler = db.kategoriler.Where(m => m.onay == "1").ToList();
            ViewBag.kategoriler = kategoriler;
            if (islem == "success")
            {
                ViewBag.State = "success";
            }
            else if (islem == "error")
            {
                ViewBag.State = "error";
            }
            return View();
        }

        [HttpPost]
        public ActionResult KayitOl(KayitFormuView model)
        {
            List<kategoriler> kategoriler = db.kategoriler.Where(m => m.onay == "1").ToList();
            ViewBag.kategoriler = kategoriler;

            uyeler dbModel = new uyeler();
            dbModel.adsoyad = model.adsoyad;
            dbModel.eposta = model.eposta;
            dbModel.parola = model.parola;

            int uye_sorgusu = db.uyeler.Where(m => m.eposta == model.eposta).Count();
            if (uye_sorgusu > 0)
            {
                ViewBag.State = "kayitli_eposta";
                return View("KayitOl");
            }

            if (model.ImageUpload != null && model.ImageUpload.ContentLength > 0)
            {
                Random rasgele = new Random();
                int sayi = rasgele.Next(100, 999999);
                string gelen_isim = model.ImageUpload.FileName.Substring(model.ImageUpload.FileName.Length-3);

                if (gelen_isim == "png" || gelen_isim == "jpg" || gelen_isim == "jpeg" || gelen_isim == "gif")
                {
                    model.ImageUpload.SaveAs(Server.MapPath("~/Uploads/Profils/" + sayi + "." + gelen_isim));
                    dbModel.resim = sayi + "." + gelen_isim;
                }
                else
                {
                    ViewBag.State = "format_error";
                    return View("KayitOl");
                }
            }
            else
            {
                dbModel.resim = "default";
            }
            db.uyeler.Add(dbModel);
            db.SaveChanges();
            Response.Redirect("~/Anasayfa/KayitOl/?islem=success");
            return View(model);
        }
        public void ReadXMLProducts()
        {
            XmlTextReader read = new XmlTextReader(Server.MapPath("~/App_Data/Products.xml"));
            while (read.Read())
            {
                if (read.NodeType == XmlNodeType.Element)
                {
                    if (read.Name == "urun_kodu")
                    {
                        Response.Write("Ürün Kodu : "+read.ReadString().ToString()+"<br/>");
                    }
                    else if (read.Name == "urun_adi")
                    {
                        Response.Write("Ürün Adı : " + read.ReadString().ToString() + "<br/>");
                    }
                    else if (read.Name == "fiyat")
                    {
                        Response.Write("Ürün Fiyatı : " + read.ReadString().ToString() + "<br/>");
                    }
                    else if (read.Name == "kdv")
                    {
                        Response.Write("KDV : " + read.ReadString().ToString() + "<br/>");
                    }
                    else if (read.Name == "aciklama")
                    {
                        Response.Write("Açıklama : " + read.ReadString().ToString() + "<br/><hr/>");
                    }
                }
            }
            read.Close();
        }
        /*public ActionResult KullaniciKayit()
        {
            KullaniciViewModel model = new KullaniciViewModel();
            return View(model);
        }
        
        [HttpPost]
        public JsonResult KullaniciKayit(KullaniciViewModel model)
        {
            JsonResultModel jModel = new JsonResultModel();
            try
            {
                kullanicilar userModel = new kullanicilar();
                userModel.ad = model.ad;
                userModel.soyad = model.soyad;
                userModel.sehir = model.sehir;

                db.kullanicilar.Add(userModel);
                db.SaveChanges();

                jModel.IsSuccess = true;
                jModel.UserMessage = "Kayıt İşlemi Başarılı...";
            }
            catch(Exception ex)
            {
                jModel.IsSuccess = false;
                jModel.UserMessage = "Kayıt İşlemi Başarısız!";
            }
            return Json(jModel, JsonRequestBehavior.AllowGet);
        }

        public void KullaniciSil(int? userid)
        {
            int satir = db.kullanicilar.Where(m => m.id == userid).ToList().Count();
            if (satir == 0)
            {
                Response.Redirect("~/Anasayfa/?islem=error");
            }
            else
            {
                try
                {
                    var user = db.kullanicilar.FirstOrDefault(m=>m.id==userid);
                    db.kullanicilar.Remove(user);
                    db.SaveChanges();
                    Response.Redirect("~/Anasayfa/?islem=success");
                }
                catch (Exception ex)
                {
                    Response.Redirect("~/Anasayfa/?islem=error2");
                }
            }
            
        }
        */
    }
}
