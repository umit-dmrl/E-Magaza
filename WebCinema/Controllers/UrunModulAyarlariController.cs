using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebCinema.ViewModel;
using WebCinema.Models;
using System.Xml;
using System.Text;

using System.Net.Http;
using System.Web.ClientServices;
using System.Net;

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

        // XML Modülü
        public ActionResult XmlCreate()
        {
            return View();
        }
        [HttpPost]
        public ActionResult XmlCreate(FormCollection form)
        {
            try
            {
                List<urunler> urun = db.urunler.ToList();

                XmlTextWriter xml = new XmlTextWriter(Server.MapPath("~/XmlCreated/products.xml"), Encoding.UTF8);
                xml.Formatting = Formatting.Indented;
                xml.WriteStartDocument();
                xml.WriteStartElement("Products");
            
                foreach (urunler item in urun)
                {
                    xml.WriteStartElement("product");
                    xml.WriteElementString("ID", item.id.ToString());
                    xml.WriteElementString("UrunKodu", item.UrunKodu.ToString());
                    xml.WriteElementString("UrunAdi", item.UrunAdi.ToString());
                    xml.WriteElementString("UrunResmi", item.UrunResmi.ToString());
                    xml.WriteElementString("UrunResimleri", item.UrunResimleri.ToString());
                    xml.WriteElementString("UrunAciklamasi", item.UrunAciklamasi.ToString());
                    xml.WriteElementString("UrunEtiketleri", item.UrunEtiketleri.ToString());
                    xml.WriteElementString("UrunFiyati", item.UrunFiyati.ToString());
                    xml.WriteElementString("UrunOnayi", item.UrunOnayi.ToString());
                    xml.WriteElementString("UrunStokAdeti", item.UrunStokAdeti.ToString());
                    xml.WriteElementString("UrunKategoriID", item.UrunKategoriID.ToString());
                    xml.WriteEndElement();
                }
            
                xml.WriteEndElement();

                xml.Close();


                ViewBag.State = "success";
            }catch(XmlException)
            {
                ViewBag.State = "error";
            }

            return View();
        }

        public FileResult DownloadXml()
        {
            byte[] file_byte = System.IO.File.ReadAllBytes(Server.MapPath("~/XmlCreated/products.xml"));
            return File(file_byte, System.Net.Mime.MediaTypeNames.Application.Octet, "products.xml");
        }
        //Xml import action view
        public ActionResult XmlImport()
        {
            return View();
        }

        [HttpPost]
        public ActionResult XmlImport(XmlUploadViewModel model)
        {
            if (model.dosya != null && model.dosya.ContentLength>0)
            {
                string file_extension = model.dosya.FileName.ToString().Substring(model.dosya.FileName.Length - 3);
                if (file_extension == "xml")
                {
                    model.dosya.SaveAs(Server.MapPath("~/Uploads/Xml/uploaded_xml.xml"));
                    ViewBag.State = "success";

                    List<kategoriler> kategoriler = db.kategoriler.ToList();
                    ViewBag.Kategoriler = kategoriler;
                }
                else
                {
                    ViewBag.State = "error_format";
                }
            }
            else
            {
                ViewBag.State = "error_null";
            }
            return View();
        }

        [HttpPost]
        public void XmlProductAdd(FormCollection form)
        {
            

            string UrunKodu = form["UrunKodu"];
            string UrunAdi = form["UrunAdi"];
            string UrunResmi = form["UrunResmi"];
            string UrunResimleri = form["UrunResimleri"];
            string UrunAciklamasi = form["UrunAciklamasi"];
            string UrunEtiketleri = form["UrunEtiketleri"];
            string UrunFiyati = form["UrunFiyati"];
            string UrunOnayi = form["UrunOnayi"];
            string UrunStokAdeti = form["UrunStokAdeti"];
            string UrunKategoriID = form["UrunKategoriID"];

            XmlTextReader oku = new XmlTextReader(Server.MapPath("~/Uploads/Xml/uploaded_xml.xml"));
            string add_data = "";
            while (oku.Read())
            {
                string data = "";
                if (oku.NodeType == XmlNodeType.Element)
                {
                    if (oku.Name.ToString() == UrunKodu)
                    {
                        //Response.Write("Ürün Kodu : " + oku.ReadString().ToString() + "<br>");
                        data += oku.ReadString().ToString()+"{";
                    }
                    if (oku.Name.ToString() == UrunAdi)
                    {
                        //Response.Write("Ürün Adı : " + oku.ReadString().ToString() + "<br>");
                        data += oku.ReadString().ToString() + "{";
                    }
                    if (oku.Name.ToString() == UrunResmi)
                    {
                        //Response.Write("Ürün Resmi : " + oku.ReadString().ToString() + "<br>");
                        data += oku.ReadString().ToString() + "{";
                    }
                    if (oku.Name.ToString() == UrunResimleri)
                    {
                        //Response.Write("Ürün Resimleri : " + oku.ReadString().ToString() + "<br>");
                        data += oku.ReadString().ToString() + "{";
                    }
                    if (oku.Name.ToString() == UrunAciklamasi)
                    {
                        //Response.Write("Ürün Açıklaması : " + oku.ReadString().ToString() + "<br>");
                        data += oku.ReadString().ToString() + "{";
                    }
                    if (oku.Name.ToString() == UrunEtiketleri)
                    {
                        //Response.Write("Ürün Etiketleri : " + oku.ReadString().ToString() + "<br>");
                        data += oku.ReadString().ToString() + "{";
                    }
                    if (oku.Name.ToString() == UrunFiyati)
                    {
                        //Response.Write("Ürün Fiyatı : " + oku.ReadString().ToString() + "<br>");
                        data += oku.ReadString().ToString() + "{";
                    }
                    if (oku.Name.ToString() == UrunOnayi)
                    {
                        //Response.Write("Ürün Onayı : " + oku.ReadString().ToString() + "<br>");
                        data += oku.ReadString().ToString() + "{";
                    }
                    if (oku.Name.ToString() == UrunStokAdeti)
                    {
                        //Response.Write("Ürün Stok Adeti : " + oku.ReadString().ToString() + "<br>");
                        data += oku.ReadString().ToString() + "]";
                    }
                    
                }
                //data parse
                string[] data_item = data.Split('{');
                for (int i = 0; i < data_item.Length; i++)
                {
                    if (data_item[i] != null && data_item[i] != "")
                    {
                        Response.Write(data_item[i] + "/");
                        add_data += data_item[i];
                    }
                }
            }
        }

    }
}
