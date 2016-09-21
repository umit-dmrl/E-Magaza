using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebCinema.ViewModel
{
    public class MesajViewModel
    {
        [Display(Name="Mesajınızın Konusu")]
        [Required(ErrorMessage="Lütfen Mesajınızın Konusunu Giriniz!")]
        [StringLength(150,ErrorMessage="En Az 5 Karakter En Fazla 150 Karakter Giriniz.",MinimumLength=5)]
        public string konu { get; set; }

        [Display(Name = "Mesajınızı Yazınız")]
        [Required(ErrorMessage = "Mesajınızı Yazmadınız!")]
        [StringLength(500, ErrorMessage = "En Az 5 Karakter En Fazla 500 Karakter Giriniz.", MinimumLength = 5)]
        public string mesaj { get; set; }

        public string tarih { get; set; }
    }
}