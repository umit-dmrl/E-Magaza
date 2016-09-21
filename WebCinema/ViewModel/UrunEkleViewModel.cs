using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebCinema.ViewModel
{
    public class UrunEkleViewModel
    {
        [Display(Name="Ürün Kategorisi Seçiniz")]
        public string UrunKategoriID { get; set; }

        [Display(Name="Ürün Kodu")]
        [StringLength(10,ErrorMessage="Ürün Kodu En Fazla 10 Haneli Bir Kod Olabilir")]
        public string UrunKodu { get; set; }

        [Display(Name="Ürün Adı")]
        [Required(ErrorMessage="*")]
        [StringLength(50,ErrorMessage="Ürün Adı En Fazla 50 En Az 3 Karakter Olmalı!",MinimumLength=3)]
        public string UrunAdi { get; set; }

        [Display(Name="Ana Ürün Resmi")]
        public HttpPostedFileBase UrunResmi { get; set; }

        [Display(Name="Diğer Ürün Resimleri")]
        public HttpPostedFileBase[] UrunResimleri { get; set; }

        [Display(Name="Ürün Açıklaması / Özellikleri")]
        public string UrunAciklamasi { get; set; }

        [Display(Name="Ürün Etiketleri (Kelimeleri ',' ile ayırınız)")]
        [StringLength(250,ErrorMessage="En Fazla 250 Karakter Giriniz!")]
        public string UrunEtiketleri { get; set; }

        [Display(Name="Ürün Fiyatı")]
        [StringLength(50,ErrorMessage="En Fazla 50 Haneli Bir Sayı Giriniz!")]
        public string UrunFiyati { get; set; }

        [Display(Name="Stok Adeti")]
        public string UrunStokAdeti { get; set; }

        [Display(Name="Ürünü Yayınla")]
        public bool UrunOnayi { get; set; }

        public List<SelectListItem> KategoriListesi { get; set; }

        public List<SelectListItem> ResimSecimi { get; set; }



    }
}