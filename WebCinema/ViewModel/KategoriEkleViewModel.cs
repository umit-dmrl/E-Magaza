using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebCinema.ViewModel
{
    public class KategoriEkleViewModel
    {
        [Display(Name = "Kategori Adı")]
        [Required(ErrorMessage="Kategori Adı Girmediniz!")]
        [StringLength(50,ErrorMessage="En Az 3 En Fazla 50 Karakter Giriniz!",MinimumLength=3)]
        public string kategoriAdi { get; set; }

        public string onay
        {
            get;
            set;
        }

        public int id { get; set; }
        
    }
}