using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebCinema.ViewModel;
using WebCinema.Models;
using WebCinema.Util;
using PagedList;
using PagedList.Mvc;

namespace WebCinema.Controllers
{
    public class UyeUrunYonetimiController : AdminBaseController
    {
        //
        // GET: /UyeUrunYonetimi/
        cinemaDB db = new cinemaDB();
        public ActionResult Index(UrunAraViewModel model,string state)
        {
            ViewBag.State = state;
            int pageIndex = model.Page ?? 1;
            model.Urunler = (from cat in db.urunler.Where(f =>
                                 (String.IsNullOrEmpty(model.UrunAdi) || f.UrunAdi.Contains(model.UrunAdi)) &&
                                 (String.IsNullOrEmpty(model.UrunKodu) || f.UrunKodu.Contains(model.UrunKodu))
                                 ).OrderBy(m => m.UrunAdi)

                             select new UrunListModel
                             {
                                 UrunID = cat.id,
                                 UrunKodu = cat.UrunKodu,
                                 UrunResmi = cat.UrunResmi,
                                 UrunAdi = cat.UrunAdi,
                                 UrunFiyati = cat.UrunFiyati,
                                 UrunStokAdeti = cat.UrunStokAdeti,
                                 UrunOnayi = cat.UrunOnayi
                             }
                                 ).ToPagedList(pageIndex, 2);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Urunler", model);
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult YeniUrunEkle()
        {
            UrunEkleViewModel model = GetModel();
            return View(model);
        }
        [HttpPost]
        public void YeniUrunEkleTest(FormCollection form)
        {
            string urunKodu = form["UrunKodu"].ToString();
            string urunAdi = form["UrunAdi"].ToString();
            string kategoriID = form["UrunKategoriID"].ToString();
            string urunStokAdeti = form["UrunStokAdeti"].ToString();
            string urunFiyati = form["UrunFiyati"].ToString();
            string urunAciklamasi = form["UrunAciklamasi"].ToString();
            string urunEtiketleri = form["UrunEtiketleri"].ToString();
            string urunOnayi = "";
            if (form["UrunOnayi"] != null)
            {
                urunOnayi = "1";
            }
            else
            {
                urunOnayi = "0";
            }
            //Ana resim işlemi
            string ana_resim = "";
            if (form["ana_resim"] != null)
            { 
                ana_resim = form["ana_resim"].ToString();
            }
            else
            {
                ana_resim = "default";
            }
            //Ana resim işlemi sonu

            //Çoklu ürün resim işlemleri
            string secilen_resimler = "";
            if (form["secilen_coklu_resimler"] != null)
            {
                foreach (var item in form["secilen_coklu_resimler"])
                {
                    secilen_resimler += item;
                }
            }
            else
            {
                secilen_resimler = "default";
            }
            //Çoklu ürün resim işlemleri sonu
            urunler dbModel = new urunler();
            dbModel.UrunKategoriID = kategoriID;
            dbModel.UrunKodu = urunKodu;
            dbModel.UrunAdi = urunAdi;
            dbModel.UrunResmi = ana_resim;
            dbModel.UrunResimleri = secilen_resimler;
            dbModel.UrunAciklamasi = urunAciklamasi;
            dbModel.UrunEtiketleri = urunEtiketleri;
            dbModel.UrunFiyati = urunFiyati;
            dbModel.UrunStokAdeti = urunStokAdeti;
            dbModel.UrunOnayi = urunOnayi;
            db.urunler.Add(dbModel);
            db.SaveChanges();
            Response.Redirect("~/UyeUrunYonetimi?state=success");
        }
        [HttpPost]
        public void YeniUrunEkle(UrunEkleViewModel model)
        {
            
            Random rnd = new Random();
            urunler dbModel = new urunler();
            dbModel.UrunKategoriID = model.UrunKategoriID;
            dbModel.UrunKodu = model.UrunKodu;
            dbModel.UrunAdi = model.UrunAdi;


            //Ürünün Ana Görseli için
            if (model.UrunResmi != null && model.UrunResmi.ContentLength > 0)
            {
                int rasgele = rnd.Next(999, 9999999);
                string gelen_resim_uzantisi = model.UrunResmi.FileName.Substring(model.UrunResmi.FileName.Length - 3);
                if (gelen_resim_uzantisi == "png" || gelen_resim_uzantisi == "jpg" || gelen_resim_uzantisi == "jpeg" || gelen_resim_uzantisi == "gif")
                {
                    model.UrunResmi.SaveAs(Server.MapPath("~/Uploads/Products/" + rasgele + "." + gelen_resim_uzantisi));
                    dbModel.UrunResmi = rasgele + "." + gelen_resim_uzantisi;
                }
            }
            else
            {
                dbModel.UrunResmi = "default";
            }

            //Ürünün diğer görselleri için
            string coklu_resimler = "";
            if (model.UrunResimleri.Length > 1)
            {
                foreach (var file in model.UrunResimleri)
                {

                    int rasgele = rnd.Next(100, 999999);
                    string gelen_resim_uzantisi = file.FileName.Substring(file.FileName.Length - 3);
                    if (gelen_resim_uzantisi == "png" || gelen_resim_uzantisi == "jpg" || gelen_resim_uzantisi == "jpeg" || gelen_resim_uzantisi == "gif")
                    {
                        file.SaveAs(Server.MapPath("~/Uploads/Products/" + rasgele + "." + gelen_resim_uzantisi));
                        coklu_resimler += rasgele + "." + gelen_resim_uzantisi + ",";
                    }
                }
            }
            else
            {
                coklu_resimler += "default";
            }
            dbModel.UrunResimleri = coklu_resimler;
            dbModel.UrunAciklamasi = model.UrunAciklamasi;
            dbModel.UrunEtiketleri = model.UrunEtiketleri;
            dbModel.UrunFiyati = model.UrunFiyati;
            dbModel.UrunStokAdeti = model.UrunStokAdeti;

            if (model.UrunOnayi == true)
            {
                dbModel.UrunOnayi = "1";
            }
            else
            {
                dbModel.UrunOnayi = "0";
            }

            db.urunler.Add(dbModel);
            db.SaveChanges();
            Response.Redirect("~/UyeUrunYonetimi/Index");
        }

        //Ürün Detayları
        public ActionResult UrunDetay(int? UrunID)
        {
            UrunEkleViewModel urunmodel = GetModel();
            List<urunler> urunler = db.urunler.Where(m => m.id == UrunID).ToList();
            if (urunler.Count > 0)
            {
                ViewBag.Urunler = urunler;
                return View(urunmodel);
            }
            else
            {
                UrunAraViewModel model = new UrunAraViewModel();
                int pageIndex = model.Page ?? 1;
                model.Urunler = (from cat in db.urunler.Where(f =>
                                     (String.IsNullOrEmpty(model.UrunAdi) || f.UrunAdi.Contains(model.UrunAdi)) &&
                                     (String.IsNullOrEmpty(model.UrunKodu) || f.UrunKodu.Contains(model.UrunKodu))
                                     ).OrderBy(m => m.UrunAdi)

                                 select new UrunListModel
                                 {
                                     UrunID = cat.id,
                                     UrunKodu = cat.UrunKodu,
                                     UrunResmi = cat.UrunResmi,
                                     UrunAdi = cat.UrunAdi,
                                     UrunFiyati = cat.UrunFiyati,
                                     UrunStokAdeti = cat.UrunStokAdeti,
                                     UrunOnayi = cat.UrunOnayi
                                 }
                                     ).ToPagedList(pageIndex, 2);
                return View("Index",model);
            }

            
        }
        [HttpPost]
        public void UrunGuncelle(FormCollection form)
        {
            try
            {
                int urunID = int.Parse(form["ProductID"].ToString());
                string urunKodu = form["UrunKodu"].ToString();
                string urunAdi = form["UrunAdi"].ToString();
                string kategoriID = form["Kategori"].ToString();
                string urunStokAdeti = form["UrunStokAdeti"].ToString();
                string urunFiyati = form["UrunFiyati"].ToString();
                string urunAciklamasi = form["UrunAciklamasi"].ToString();
                string urunEtiketleri = form["UrunEtiketleri"].ToString();
                string urunOnayi = "";
                if (form["UrunOnayi"] != null)
                {
                    urunOnayi = "1";
                }
                else
                {
                    urunOnayi = "0";
                }

                //Ana resimdeki değişimi algıla
                string guncellenecek_ana_resim = "";
                string mevcut_ana_resim = form["mevcut_ana_resim"].Trim().ToString();
                if (form["yeni_ana_resim"] != null)
                {

                    string secilen_ana_resim = form["yeni_ana_resim"].Trim().ToString();
                    guncellenecek_ana_resim = secilen_ana_resim;
                }
                else
                {
                    guncellenecek_ana_resim = mevcut_ana_resim;
                }
                //Ana resim işlemi bitti

                //Multi resim işlemleri
                string guncellenecek_multi_resimler = "";
                if (form["resimler"] != null)
                {
                    int mevcut_multi_resimler = form["resimler"].Length;
                    foreach (var val in form["resimler"])
                    {
                        guncellenecek_multi_resimler += val.ToString();
                    }
                }
                else
                {
                    guncellenecek_multi_resimler = "default";
                }
                if (form["yeni_secilen_coklu_resimler"] != null)
                {
                    guncellenecek_multi_resimler += ",";
                    foreach (var item in form["yeni_secilen_coklu_resimler"])
                    {
                        guncellenecek_multi_resimler += item;
                    }
                }
                //Multi resim işlemleri bitti

                urunler urun = db.urunler.FirstOrDefault(m => m.id == urunID);
                if (urun != null)
                {
                    urun.UrunKategoriID = kategoriID;
                    urun.UrunKodu = urunKodu;
                    urun.UrunAdi = urunAdi;
                    urun.UrunResmi = guncellenecek_ana_resim;
                    urun.UrunResimleri = guncellenecek_multi_resimler;
                    urun.UrunAciklamasi = urunAciklamasi;
                    urun.UrunEtiketleri = urunEtiketleri;
                    urun.UrunFiyati = urunFiyati;
                    urun.UrunStokAdeti = urunStokAdeti;
                    urun.UrunOnayi = urunOnayi;
                    db.SaveChanges();
                    Response.Redirect("~/UyeUrunYonetimi?state=success");
                }
                else
                {
                    Response.Redirect("~/UyeUrunYonetimi?state=urun_yok");
                }
            }
            catch (FormatException)
            {
                Response.Redirect("~/UyeUrunYonetimi?state=islem_uygun_degil");
            }
            
        }

        private UrunEkleViewModel GetModel()
        {
            UrunEkleViewModel model = new UrunEkleViewModel();
            model.KategoriListesi = (from cat in db.kategoriler.ToList()
                                     select new SelectListItem
                                     {
                                         Selected= false,
                                         Text = cat.kategoriAdi,
                                         Value = cat.id.ToString()
                                     }
                                     ).ToList();
            model.ResimSecimi = (from cat in db.resimler.ToList()
                                     select new SelectListItem
                                     {
                                         Selected = false,
                                         Text = cat.resim,
                                         Value = cat.id.ToString()
                                     }
                                     ).ToList();
            

            return model;
        }

        

    }
}
