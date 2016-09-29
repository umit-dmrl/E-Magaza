using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebCinema.ViewModel
{
    public class XmlUploadViewModel
    {
        [Display(Name="Xml Dosyanızı Bilgisayarınızdan Seçin")]
        [Required(ErrorMessage="Lütfen Bir Dosya Seçiniz")]
        public HttpPostedFileBase dosya { get; set; }
    }
}