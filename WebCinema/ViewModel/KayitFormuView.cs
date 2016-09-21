using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebCinema.ViewModel
{
    public class KayitFormuView
    {
        [Display(Name="Ad Soyad")]
        [Required(ErrorMessage="Adınızı Ve Soyadınızı Giriniz!")]
        [StringLength(50, ErrorMessage = "Adınız En Fazla 50 En Az 6 Karakter Olabilir", MinimumLength = 6)]
        public string adsoyad { get; set; }

        [Display(Name = "E-Posta Adresiniz")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Geçersiz Bir E-Posta Adresi Girdiniz!")]
        [EmailAddress]
        [Required(ErrorMessage = "E-Posta Adresinizi Giriniz!")]
        [StringLength(100, ErrorMessage = "E-Posta Adresiniz En Fazla 100 En Az 15 Karakter Olabilir", MinimumLength = 15)]
        public string eposta { get; set; }

        [Display(Name = "Parolanızı Girin")]
        [Required(ErrorMessage = "Giriş Parolanızı Giriniz!")]
        [DataType(DataType.Password)]
        public string parola { get; set; }

        [Display(Name = "Parolanızı Tekrar Girin")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Giriş Parolanızı Tekrar Giriniz!")]
        [Compare("parola",ErrorMessage="Parola Uyuşmazlığı Var!")]
        public string parola_tekrar { get; set; }

        [Display(Name="Profil Resmi Yükleyiniz")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase ImageUpload { get; set; }

        public string mesaj { get; set; }


    }
}