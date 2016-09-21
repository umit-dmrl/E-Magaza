using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebCinema.ViewModel
{
    public class ResimYukleViewModel
    {
        [Display(Name="Resim Seçiniz")]
        [Required(ErrorMessage="Lütfen Yüklemek İstediğiniz Resimleri Seçiniz!")]
        public HttpPostedFileBase[] resimler { get; set; }
    }
}