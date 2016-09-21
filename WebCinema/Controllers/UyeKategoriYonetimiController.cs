using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebCinema.Util;
using WebCinema.Models;
using WebCinema.ViewModel;
using PagedList;
using PagedList.Mvc;

namespace WebCinema.ViewModel
{
    public class UyeKategoriYonetimiController : AdminBaseController
    {
        //
        // GET: /UyeKategoriYonetimi/
        cinemaDB db = new cinemaDB();
        //Kategorileri Listelemek için kullanılabilir.
        public ActionResult Index(KategoriModel model,string state)
        {
            ViewBag.State = state;
            int pageIndex = model.Page ?? 1;
            model.Kategoriler = (from cat in db.kategoriler.Where(f =>
                                     (String.IsNullOrEmpty(model.kategoriAdi) || f.kategoriAdi.Contains(model.kategoriAdi))

                                     ).OrderBy(m => m.kategoriAdi)
                                     select new KategoriListModel
                                     {
                                         kategoriAdi = cat.kategoriAdi,
                                         onay = cat.onay,
                                         kategoriID = cat.id
                                     }
                                     ).ToPagedList(pageIndex,2);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Kategoriler", model);
            }
            else
            {
                return View(model);
            }

            
        }
        public void KategoriSil(string kategoriID)
        {
            if (kategoriID != null)
            {
                try
                {
                    int gelen_id = int.Parse(kategoriID);
                    kategoriler kategori = db.kategoriler.FirstOrDefault(m => m.id == gelen_id);
                    if (kategori != null)
                    {
                        db.kategoriler.Remove(kategori);
                        db.SaveChanges();
                        Response.Redirect("~/UyeKategoriYonetimi/?state=success");
                    }
                    else
                    {
                        Response.Redirect("~/UyeKategoriYonetimi/?state=kategori_yok");
                    }
                }
                catch (FormatException)
                {
                    Response.Redirect("~/UyeKategoriYonetimi/?state=error");
                }
            }
            else
            {
                Response.Redirect("~/UyeKategoriYonetimi/?state=error");
            }
        }
        public ActionResult KategoriDuzenle(int? kategoriID)
        {
            List<kategoriler> kategori = db.kategoriler.Where(m=>m.id==kategoriID).ToList();
            if (kategori.Count > 0)
            {
                ViewBag.Kategori = kategori;
                return View();
            }
            else
            {
                KategoriModel model = new KategoriModel();
                int pageIndex = model.Page ?? 1;
                model.Kategoriler = (from cat in db.kategoriler.Where(f =>
                                         (String.IsNullOrEmpty(model.kategoriAdi) || f.kategoriAdi.Contains(model.kategoriAdi))

                                         ).OrderBy(m => m.kategoriAdi)
                                     select new KategoriListModel
                                     {
                                         kategoriAdi = cat.kategoriAdi,
                                         onay = cat.onay,
                                         kategoriID = cat.id
                                     }
                                         ).ToPagedList(pageIndex, 2);
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_Kategoriler", model);
                }
                else
                {
                    ViewBag.KayitBulunamadi = "KayitBulunamadi";
                    return View("Index",model);
                }
            }
            
        }

        [HttpPost]
        public ActionResult KategoriDuzenle(KategoriEkleViewModel kayitModel)
        {
            int id = kayitModel.id;
            List<kategoriler> kategori = db.kategoriler.Where(m => m.id == id).ToList();
            if (kategori.Count > 0)
            {
                //Güncelleme yapılabilir
                kategoriler dbModel = db.kategoriler.FirstOrDefault(m => m.id == id);
                dbModel.kategoriAdi = kayitModel.kategoriAdi;
                dbModel.onay = kayitModel.onay;

                db.SaveChanges();

                ViewBag.GuncellemeBasarili = "GuncellemeBasarili";
                KategoriModel model = new KategoriModel();
                int pageIndex = model.Page ?? 1;
                model.Kategoriler = (from cat in db.kategoriler.Where(f =>
                                         (String.IsNullOrEmpty(model.kategoriAdi) || f.kategoriAdi.Contains(model.kategoriAdi))

                                         ).OrderBy(m => m.kategoriAdi)
                                     select new KategoriListModel
                                     {
                                         kategoriAdi = cat.kategoriAdi,
                                         onay = cat.onay,
                                         kategoriID = cat.id
                                     }
                                         ).ToPagedList(pageIndex, 2);
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_Kategoriler", model);
                }
                else
                {
                    return View("Index", model);
                }
            }
            else
            {
                //Böyle bir id olmadığı için güncelleme yapılamaz.
                KategoriModel model = new KategoriModel();
                int pageIndex = model.Page ?? 1;
                model.Kategoriler = (from cat in db.kategoriler.Where(f =>
                                         (String.IsNullOrEmpty(model.kategoriAdi) || f.kategoriAdi.Contains(model.kategoriAdi))

                                         ).OrderBy(m => m.kategoriAdi)
                                     select new KategoriListModel
                                     {
                                         kategoriAdi = cat.kategoriAdi,
                                         onay = cat.onay,
                                         kategoriID = cat.id
                                     }
                                         ).ToPagedList(pageIndex, 2);
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_Kategoriler", model);
                }
                else
                {
                    ViewBag.KayitBulunamadi = "KayitBulunamadi";
                    return View("Index", model);
                }
            }
        }

        public ActionResult YeniKategoriEkle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult YeniKategoriEkle(KategoriEkleViewModel kayit_model)
        {
            List<kategoriler> kategori = db.kategoriler.Where(m => m.kategoriAdi == kayit_model.kategoriAdi).ToList();
            if (kategori.Count > 0)
            {
                //Aynı isimde kayıt var
                ViewBag.State = "IsimCakismasi";
                return View();
            }
            else
            {
                kategoriler dbModel = new kategoriler();
                dbModel.kategoriAdi = kayit_model.kategoriAdi;
                dbModel.onay = kayit_model.onay;
                db.kategoriler.Add(dbModel);
                db.SaveChanges();

                KategoriModel model = new KategoriModel();
                int pageIndex = model.Page ?? 1;
                model.Kategoriler = (from cat in db.kategoriler.Where(f =>
                                         (String.IsNullOrEmpty(model.kategoriAdi) || f.kategoriAdi.Contains(model.kategoriAdi))

                                         ).OrderBy(m => m.kategoriAdi)
                                     select new KategoriListModel
                                     {
                                         kategoriAdi = cat.kategoriAdi,
                                         onay = cat.onay,
                                         kategoriID = cat.id
                                     }
                                         ).ToPagedList(pageIndex, 2);
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_Kategoriler", model);
                }
                else
                {
                    ViewBag.KayitEklemeBasarili = "KayitEklemeBasarili";
                    return View("Index", model);
                }
            }
            
        }
        

    }
}
