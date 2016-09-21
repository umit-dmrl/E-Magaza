using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebCinema.ViewModel
{
    public class KullaniciViewModel
    {
        [Display(Name="Adınız")]
        [Required(ErrorMessage = "Adınızı Giriniz!")]
        public string ad { get; set; }

        [Display(Name = "Soyadınız")]
        [Required(ErrorMessage = "Soyadınızı Giriniz!")]
        public string soyad { get; set; }

        [Display(Name = "Yaşadığınız Şehir")]
        [Required(ErrorMessage = "Bir Şehir Seçiniz!")]
        public string sehir { get; set; }
    }
}